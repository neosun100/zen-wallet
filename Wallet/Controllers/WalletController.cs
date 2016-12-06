﻿using System;
using System.Threading;
using Wallet.Domain;
using Wallet.core;
using Infrastructure;

namespace Wallet
{
	public class WalletController
	{
		private static WalletController instance = null;

		private ActionBarView _actionBarView;
		public ActionBarView ActionBarView {
			get {
				return _actionBarView;
			}
			set {
				_actionBarView = value;
			//	ActionBarView.Asset = CurrencyEnum.Zen;
			}
		}

		public SendStub SendStub = new SendStub ();

		public TransactionsView TransactionsView { get; set; }
		public IWalletView WalletView { get; set; }

		//public CurrencyEnum currency;
		//public String currencyStr;

		private AssetType asset = AssetsManager.Assets["zen"];

		private Thread tempThread;
		private bool stopping = false;

		public static WalletController GetInstance() {
			if (instance == null) {
				instance = new WalletController ();
			}

			return instance;
		}

		public AssetType Asset { 
			set {
				asset = value;
				ActionBarView.Asset = value;
				WalletView.ActionBar = !(value is AssetTypeAll);
			} get {
				return asset;
			}
		}

		IDisposable messageListener;

		public WalletController()
		{
			//tempThread = new Thread (Reset);
			//tempThread.Start ();

			messageListener = MessageProducer<WalletManager.IMessage>.Instance.AddMessageListener(
				new EventLoopMessageListener<WalletManager.IMessage>(message =>
				{
					Gtk.Application.Invoke(delegate
					{
						if (message is WalletManager.TransactionReceivedMessage)
						{
							var tx = ((WalletManager.TransactionReceivedMessage)message).Transaction;

							DirectionEnum direcion = DirectionEnum.Recieved;

							Decimal amount = tx.outputs[0].spend.amount;
							Decimal fee = 0;
							DateTime date = DateTime.Now;

							TransactionsView.AddTransactionItem(new TransactionItem(amount, direcion, asset, date, Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), fee));
						}
					});
			}));
		}
	

		public void Send(Decimal amount) {
		}

		public void Quit() {
			messageListener.Dispose();
			stopping = true;
			//tempThread.Join ();
		}

		private void Reset() {
			int i = 0;

			while (!stopping) {
				UpdateUI ();

				if (i++ > 10) {
					break;
				}

				Thread.Sleep(100);
			}
		}

		Random random = new Random();

		public void UpdateUI() {
			Gtk.Application.Invoke(delegate {
				if (ActionBarView != null) {
					//Alternative: Runtime.DispatchService.GuiDispatch (new StatefulMessageHandler (UpdateGui), n);
					ActionBarView.Total = (decimal)random.Next(1, 1000) / 10000;
					ActionBarView.Rate = (decimal)random.Next(1, 1000) / 10000;
				}

				if (TransactionsView != null) {

					DirectionEnum direcion = random.Next(0, 10) > 5 ? DirectionEnum.Sent : DirectionEnum.Recieved;
					Decimal amount = (Decimal)random.Next(1, 100000) / 1000000;
					Decimal fee = (Decimal)random.Next(1, 100) / 1000000;
					DateTime date = DateTime.Now.AddDays(-1 * random.Next(0, 100));

					TransactionsView.AddTransactionItem(new TransactionItem(amount, direcion, asset, date, Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), fee));
				}
			});
		}
	}
}

