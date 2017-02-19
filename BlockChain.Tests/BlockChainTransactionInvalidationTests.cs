using Consensus;
using NUnit.Framework;
using Infrastructure.Testing;
using System.Linq;
using Wallet.core.Data;

namespace BlockChain
{
	[TestFixture()]
	public class BlockChainTransactionInvalidationTests : BlockChainTestsBase
	{
		[SetUp]
		public void SetUp()
		{
			OneTimeSetUp();
		}

		[Test]
		public void ShouldRejectDoubleSpendInBlock()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 100);
			var output1 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 1);
			var tx1 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output1).Sign(key.Private);
			var output2 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 2);
			var tx2 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output2).Sign(key.Private);

			var bk = _GenesisBlock.AddTx(genesisTx);
			Assert.That(_BlockChain.HandleBlock(bk), Is.True);
			Assert.That(_BlockChain.HandleBlock(bk.Child().AddTx(tx1).AddTx(tx2)), Is.False);
		}

		[Test]
		public void ShouldRejectDoubleSpendInOnNewBlock()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 100);
			var output1 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 1);
			var tx1 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output1).Sign(key.Private);
			var output2 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 2);
			var tx2 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output2).Sign(key.Private);

			var bk = _GenesisBlock.AddTx(genesisTx);
			Assert.That(_BlockChain.HandleBlock(bk), Is.True);
			bk = bk.Child().AddTx(tx1);
			Assert.That(_BlockChain.HandleBlock(bk.AddTx(tx2)), Is.False);
		}

		[Test]
		public void ShouldRemoveSameTxFromMempool()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 100);
			var bk = _GenesisBlock.AddTx(genesisTx);

			//TODO: key.Address
			var tx = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(Key.Create().Address, Consensus.Tests.zhash, 1).Sign(key.Private);
			var txHash = Merkle.transactionHasher.Invoke(tx);

			Assert.That(_BlockChain.HandleBlock(bk), Is.True);
			Assert.That(_BlockChain.HandleTransaction(tx), Is.EqualTo(BlockChain.TxResultEnum.Accepted));
			Assert.That(_BlockChain.memPool.TxPool.Contains(txHash), Is.True);
			Assert.That(_BlockChain.HandleBlock(bk.Child().AddTx(tx)), Is.True);
			Assert.That(_BlockChain.memPool.TxPool.Contains(txHash), Is.False);
		}

		[Test]
		public void ShouldRemoveInvalidatedTxFromMempoolOnNewBlock()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 100);
			var bk = _GenesisBlock.AddTx(genesisTx);

			//TODO: key.Address
			var txInvalidated  = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(Key.Create().Address, Consensus.Tests.zhash, 1).Sign(key.Private);
			var txInvalidating = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(Key.Create().Address, Consensus.Tests.zhash, 2).Sign(key.Private);
			var txHash = Merkle.transactionHasher.Invoke(txInvalidated);

			Assert.That(_BlockChain.HandleBlock(bk), Is.True);
			Assert.That(_BlockChain.HandleTransaction(txInvalidated), Is.EqualTo(BlockChain.TxResultEnum.Accepted));
			Assert.That(_BlockChain.memPool.TxPool.Contains(txHash), Is.True);
			Assert.That(_BlockChain.HandleBlock(bk.Child().AddTx(txInvalidating)), Is.True);

			Assert.That(_BlockChain.memPool.TxPool.Contains(txHash), Is.False);
		}

		[Test]
		public void ShouldUndoBlockOnReorganization()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 50).AddOutput(key.Address, Consensus.Tests.zhash, 50);
			_GenesisBlock = _GenesisBlock.AddTx(genesisTx);
			Assert.That(_BlockChain.HandleBlock(_GenesisBlock), Is.True);

			var output1_withconflict = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 5);
			var tx1_withconflict = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output1_withconflict).Sign(key.Private);
			var output1_withoutconflict = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 5);
			var tx1_withoutconflict = Utils.GetTx().AddInput(genesisTx, 1).AddOutput(output1_withoutconflict).Sign(key.Private);
			Assert.That(_BlockChain.HandleBlock(_GenesisBlock.Child().AddTx(tx1_withconflict).AddTx(tx1_withoutconflict)), Is.True);

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withconflict), Is.True);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withoutconflict), Is.True);

			var output2 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 5);
			var tx2_withconflict = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output2).Sign(key.Private);
			var sideChainBlock = _GenesisBlock.Child().AddTx(tx2_withconflict);
			Assert.That(_BlockChain.HandleBlock(sideChainBlock), Is.True);

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withconflict), Is.True);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withoutconflict), Is.True);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.False);

			Assert.That(_BlockChain.HandleBlock(sideChainBlock.Child()), Is.True);

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withconflict), Is.False);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1_withoutconflict), Is.False);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.True);

			Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx1_withconflict)), Is.False);
			Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx1_withoutconflict)), Is.True);
		}

		[Test]
		public void ShouldNotPutInvalidatedTxIntoMempoolWhenReorganizing()
		{
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 100);
			_GenesisBlock = _GenesisBlock.AddTx(genesisTx);

			Assert.That(_BlockChain.HandleBlock(_GenesisBlock), Is.True);

			var output1 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 1);
			var tx1 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output1).Sign(key.Private);

			var output2 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 2);
			var tx2 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output2).Sign(key.Private);

			Assert.That(_BlockChain.HandleBlock(_GenesisBlock.Child().AddTx(tx1)), Is.True);

			TestDelegate x = delegate
			{
				Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1), Is.True);
				Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx1)), Is.False);
				Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.False);
				Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx2)), Is.False);
			};

			x();

			var branch = _GenesisBlock.Child().AddTx(tx2);
			Assert.That(_BlockChain.HandleBlock(branch), Is.True);

			x();

			Assert.That(_BlockChain.HandleBlock(branch.Child()), Is.True); // reorganize

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1), Is.False);
			Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx1)), Is.False);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.True);
			Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx2)), Is.False);
		}

		[Test]
		public void ShouldUndoReorganization()
		{ 
			var key = Key.Create();

			var genesisTx = Utils.GetTx().AddOutput(key.Address, Consensus.Tests.zhash, 50).AddOutput(key.Address, Consensus.Tests.zhash, 50);
			_GenesisBlock = _GenesisBlock.AddTx(genesisTx);
			Assert.That(_BlockChain.HandleBlock(_GenesisBlock), Is.True);

			var output1 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 5);
			var tx1 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output1).Sign(key.Private);

			var output2 = Utils.GetOutput(Key.Create().Address, Consensus.Tests.zhash, 5);
			var tx2 = Utils.GetTx().AddInput(genesisTx, 0).AddOutput(output2).Sign(key.Private);

			Assert.That(_BlockChain.HandleBlock(_GenesisBlock.Child().AddTx(tx1)), Is.True);

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1), Is.True);

			var sideChainBlock = _GenesisBlock.Child().AddTx(tx2);

			Assert.That(_BlockChain.HandleBlock(sideChainBlock.Child()), Is.True); //TODO: assert: orphan

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output1), Is.True);
			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.False);

			Assert.That(_BlockChain.HandleBlock(sideChainBlock), Is.True);

			_BlockChain.WaitDbTxs();

			Assert.That(_BlockChain.GetUTXOSet(null).Values.Contains(output2), Is.True);
			Assert.That(_BlockChain.memPool.TxPool.Contains(Merkle.transactionHasher.Invoke(tx1)), Is.False);
		}
	}
}
