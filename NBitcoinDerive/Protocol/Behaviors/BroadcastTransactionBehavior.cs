﻿#if !NOSOCKET
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using System.Threading;
using Consensus;

namespace NBitcoin.Protocol.Behaviors
{
	public delegate void TransactionBroadcastedDelegate(Types.Transaction transaction);
	public delegate void TransactionRejectedDelegate(Types.Transaction transaction, RejectPayload reject);
	public class TransactionBroadcast
	{
		public BroadcastState State
		{
			get;
			internal set;
		}
		public Types.Transaction Transaction
		{
			get;
			internal set;
		}
		internal ulong PingValue
		{
			get;
			set;
		}
		public DateTime AnnouncedTime
		{
			get;
			internal set;
		}
	}
	public enum BroadcastState
	{
		NotSent,
		Announced,
		Broadcasted,
		Rejected,
		Accepted
	}

	public class BroadcastHub
	{
		public static BroadcastHub GetBroadcastHub(Node node)
		{
			return GetBroadcastHub(node.Behaviors);
		}
		public static BroadcastHub GetBroadcastHub(NodeConnectionParameters parameters)
		{
			return GetBroadcastHub(parameters.TemplateBehaviors);
		}
		public static BroadcastHub GetBroadcastHub(NodeBehaviorsCollection behaviors)
		{
			return behaviors.OfType<BroadcastHubBehavior>().Select(c => c.BroadcastHub).FirstOrDefault();
		}

		internal ConcurrentDictionary<byte[], Types.Transaction> BroadcastedTransaction = new ConcurrentDictionary<byte[], Types.Transaction>();
		internal ConcurrentDictionary<Node, Node> Nodes = new ConcurrentDictionary<Node, Node>();
		public event TransactionBroadcastedDelegate TransactionBroadcasted;
		public event TransactionRejectedDelegate TransactionRejected;

		public IEnumerable<Types.Transaction> BroadcastingTransactions
		{
			get
			{
				return BroadcastedTransaction.Values;
			}
		}

		internal void OnBroadcastTransaction(Types.Transaction transaction)
		{
			var nodes = Nodes
						.Select(n => n.Key.Behaviors.Find<BroadcastHubBehavior>())
						.Where(n => n != null)
						.ToArray();
			foreach (var node in nodes)
			{
				node.BroadcastTransactionCore(transaction);
			}
		}

		internal void OnTransactionRejected(Types.Transaction tx, RejectPayload reject)
		{
			var evt = TransactionRejected;
			if (evt != null)
				evt(tx, reject);
		}

		internal void OnTransactionBroadcasted(Types.Transaction tx)
		{
			var evt = TransactionBroadcasted;
			if (evt != null)
				evt(tx);
		}

		//demo
		private byte[] GetHash(Types.Transaction transaction)
		{
			try
			{
				return Merkle.transactionHasher.Invoke(transaction);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}

		/// <summary>
		/// Broadcast a transaction on the hub
		/// </summary>
		/// <param name="transaction">The transaction to broadcast</param>
		/// <returns>The cause of the rejection or null</returns>
		public Task<RejectPayload> BroadcastTransactionAsync(Types.Transaction transaction)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");

			TaskCompletionSource<RejectPayload> completion = new TaskCompletionSource<RejectPayload>();
			var hash = GetHash(transaction);
			if (BroadcastedTransaction.TryAdd(hash, transaction))
			{
				TransactionBroadcastedDelegate broadcasted = null;
				TransactionRejectedDelegate rejected = null;
				broadcasted = (t) =>
				{
					if (GetHash(t) == hash)
					{
						completion.SetResult(null);
						TransactionRejected -= rejected;
						TransactionBroadcasted -= broadcasted;
					}
				};
				TransactionBroadcasted += broadcasted;
				rejected = (t, r) =>
				{
					if (r.Hash == hash)
					{
						completion.SetResult(r);
						TransactionRejected -= rejected;
						TransactionBroadcasted -= broadcasted;
					}
				};
				TransactionRejected += rejected;
				OnBroadcastTransaction(transaction);
			}
			return completion.Task;
		}

		public BroadcastHubBehavior CreateBehavior()
		{
			return new BroadcastHubBehavior(this);
		}
	}

	public class BroadcastHubBehavior : NodeBehavior
	{
		ConcurrentDictionary<byte[], TransactionBroadcast> _HashToTransaction = new ConcurrentDictionary<byte[], TransactionBroadcast>(new ByteArrayComparer());
		ConcurrentDictionary<ulong, TransactionBroadcast> _PingToTransaction = new ConcurrentDictionary<ulong, TransactionBroadcast>();

		public BroadcastHubBehavior()
		{
			_BroadcastHub = new BroadcastHub();
		}
		public BroadcastHubBehavior(BroadcastHub hub)
		{
			_BroadcastHub = hub ?? new BroadcastHub();
			foreach (var tx in _BroadcastHub.BroadcastedTransaction)
			{
				_HashToTransaction.TryAdd(tx.Key, new TransactionBroadcast()
				{
					State = BroadcastState.NotSent,
					Transaction = tx.Value
				});
			}
		}

		private readonly BroadcastHub _BroadcastHub;
		public BroadcastHub BroadcastHub
		{
			get
			{
				return _BroadcastHub;
			}
		}

		TransactionBroadcast GetTransaction(byte[] hash, bool remove)
		{
			TransactionBroadcast result;

			if (remove)
			{
				if (_HashToTransaction.TryRemove(hash, out result))
				{
					TransactionBroadcast unused;
					_PingToTransaction.TryRemove(result.PingValue, out unused);
				}
			}
			else
			{
				_HashToTransaction.TryGetValue(hash, out result);
			}
			return result;
		}
		TransactionBroadcast GetTransaction(ulong pingValue, bool remove)
		{
			TransactionBroadcast result;

			if (remove)
			{
				if (_PingToTransaction.TryRemove(pingValue, out result))
				{
					TransactionBroadcast unused;
					_HashToTransaction.TryRemove(GetHash(result.Transaction), out unused);
				}
			}
			else
			{
				_PingToTransaction.TryGetValue(pingValue, out result);
			}
			return result;
		}
		void AttachedNode_StateChanged(Node node, NodeState oldState)
		{
			if (node.State == NodeState.HandShaked)
			{
				_BroadcastHub.Nodes.TryAdd(node, node);
				AnnounceAll();
			}
		}



		private void AnnounceAll()
		{
			foreach (var broadcasted in _HashToTransaction)
			{
				if (broadcasted.Value.State == BroadcastState.NotSent ||
				   (DateTime.UtcNow - broadcasted.Value.AnnouncedTime) < TimeSpan.FromMinutes(5.0))
					Announce(broadcasted.Value, broadcasted.Key);
			}
		}


		internal void BroadcastTransactionCore(Types.Transaction transaction)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			var tx = new TransactionBroadcast();
			tx.Transaction = transaction;
			tx.State = BroadcastState.NotSent;
			var hash = GetHash(transaction);
			if (_HashToTransaction.TryAdd(hash, tx))
			{
				Announce(tx, hash);
			}
		}

		private void Announce(TransactionBroadcast tx, byte[] hash)
		{
			var node = AttachedNode;
			if (node != null && node.State == NodeState.HandShaked)
			{
				tx.State = BroadcastState.Announced;
				tx.AnnouncedTime = DateTime.UtcNow;
				node.SendMessageAsync(new InvPayload(InventoryType.MSG_TX, hash)).ConfigureAwait(false);
			}
		}

		Timer _Flush;
		protected override void AttachCore()
		{
			AttachedNode.StateChanged += AttachedNode_StateChanged;
			AttachedNode.MessageReceived += AttachedNode_MessageReceived;
			_Flush = new Timer(o =>
			{
				AnnounceAll();
			}, null, 0, (int)TimeSpan.FromMinutes(10).TotalMilliseconds);
		}

		protected override void DetachCore()
		{
			AttachedNode.StateChanged -= AttachedNode_StateChanged;
			AttachedNode.MessageReceived -= AttachedNode_MessageReceived;

			Node unused;
			_BroadcastHub.Nodes.TryRemove(AttachedNode, out unused);
			_Flush.Dispose();
		}

		void AttachedNode_MessageReceived(Node node, IncomingMessage message)
		{
			message.IfPayloadIs<InvPayload>(invPayload =>
			{
				foreach (var hash in invPayload.Where(i => i.Type == InventoryType.MSG_TX).Select(i => i.Hash))
				{
					var tx = GetTransaction(hash, true);
					if (tx != null)
						tx.State = BroadcastState.Accepted;
					Types.Transaction unused;
					if (_BroadcastHub.BroadcastedTransaction.TryRemove(hash, out unused))
					{
						_BroadcastHub.OnTransactionBroadcasted(tx.Transaction);
					}
				}
			});

			message.IfPayloadIs<RejectPayload>(reject =>
			{
				if (reject.Message == "tx")
				{
					var tx = GetTransaction(reject.Hash, true);
					if (tx != null)
						tx.State = BroadcastState.Rejected;
					Types.Transaction tx2;
					if (_BroadcastHub.BroadcastedTransaction.TryRemove(reject.Hash, out tx2))
					{
						_BroadcastHub.OnTransactionRejected(tx2, reject);
					}

				}
			});

			message.IfPayloadIs<GetDataPayload>(getData =>
			{
				foreach (var inventory in getData.Inventory.Where(i => i.Type == InventoryType.MSG_TX))
				{
					var tx = GetTransaction(inventory.Hash, false);
					if (tx != null)
					{
						tx.State = BroadcastState.Broadcasted;
						var ping = new PingPayload();
						tx.PingValue = ping.Nonce;
						_PingToTransaction.TryAdd(tx.PingValue, tx);
						node.SendMessageAsync(tx.Transaction);
						node.SendMessageAsync(ping);
					}
				}
			});

			message.IfPayloadIs<PongPayload>(pong =>
			{
				var tx = GetTransaction(pong.Nonce, true);
				if (tx != null)
				{
					tx.State = BroadcastState.Accepted;
					Types.Transaction unused;
					if (_BroadcastHub.BroadcastedTransaction.TryRemove(GetHash(tx.Transaction), out unused))
					{
						_BroadcastHub.OnTransactionBroadcasted(tx.Transaction);
					}
				}
			});
		}

		public override object Clone()
		{
			return new BroadcastHubBehavior(_BroadcastHub);
		}

		public IEnumerable<TransactionBroadcast> Broadcasts
		{
			get
			{
				return _HashToTransaction.Values;
			}
		}

		//demo
		private byte[] GetHash(Types.Transaction transaction)
		{
			return Merkle.transactionHasher.Invoke(transaction);
		}
	}
}
#endif