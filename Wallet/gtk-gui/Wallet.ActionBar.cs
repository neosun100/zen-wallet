
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet
{
	public partial class ActionBar
	{
		private global::Gtk.HBox hboxMain;
		
		private global::Gtk.HBox hboxCurrency;
		
		private global::Gtk.Image image2;
		
		private global::Gtk.HBox hboxCurrencySub;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.Label label3;
		
		private global::Gtk.Image image4;
		
		private global::Gtk.HBox hbox3;
		
		private global::Gtk.Label label4;
		
		private global::Gtk.Label label5;
		
		private global::Gtk.HBox hboxActions;
		
		private global::Gtk.Image image1;
		
		private global::Gtk.Image image3;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Wallet.ActionBar
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Wallet.ActionBar";
			// Container child Wallet.ActionBar.Gtk.Container+ContainerChild
			this.hboxMain = new global::Gtk.HBox ();
			this.hboxMain.Name = "hboxMain";
			// Container child hboxMain.Gtk.Box+BoxChild
			this.hboxCurrency = new global::Gtk.HBox ();
			this.hboxCurrency.Name = "hboxCurrency";
			this.hboxCurrency.Spacing = 10;
			// Container child hboxCurrency.Gtk.Box+BoxChild
			this.image2 = new global::Gtk.Image ();
			this.image2.WidthRequest = 100;
			this.image2.Name = "image2";
			this.image2.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Wallet.Assets.misc.Bitcoin.png");
			this.hboxCurrency.Add (this.image2);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hboxCurrency [this.image2]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			w1.Padding = ((uint)(10));
			// Container child hboxCurrency.Gtk.Box+BoxChild
			this.hboxCurrencySub = new global::Gtk.HBox ();
			this.hboxCurrencySub.Name = "hboxCurrencySub";
			this.hboxCurrencySub.Spacing = 6;
			// Container child hboxCurrencySub.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
			this.hbox1.Add (this.label1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Yalign = 0.6F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("label2");
			this.hbox1.Add (this.label3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label3]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			this.hboxCurrencySub.Add (this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hboxCurrencySub [this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hboxCurrencySub.Gtk.Box+BoxChild
			this.image4 = new global::Gtk.Image ();
			this.image4.Name = "image4";
			this.image4.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Wallet.Assets.misc.arrows.png");
			this.hboxCurrencySub.Add (this.image4);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hboxCurrencySub [this.image4]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hboxCurrencySub.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("label3");
			this.hbox3.Add (this.label4);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.label4]));
			w6.Position = 0;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Yalign = 0.6F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("label4");
			this.hbox3.Add (this.label5);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.label5]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			this.hboxCurrencySub.Add (this.hbox3);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hboxCurrencySub [this.hbox3]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			this.hboxCurrency.Add (this.hboxCurrencySub);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hboxCurrency [this.hboxCurrencySub]));
			w9.Position = 1;
			this.hboxMain.Add (this.hboxCurrency);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hboxMain [this.hboxCurrency]));
			w10.Position = 0;
			// Container child hboxMain.Gtk.Box+BoxChild
			this.hboxActions = new global::Gtk.HBox ();
			this.hboxActions.Name = "hboxActions";
			this.hboxActions.Spacing = 6;
			// Container child hboxActions.Gtk.Box+BoxChild
			this.image1 = new global::Gtk.Image ();
			this.image1.Name = "image1";
			this.image1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Wallet.Assets.misc.send.png");
			this.hboxActions.Add (this.image1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hboxActions [this.image1]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child hboxActions.Gtk.Box+BoxChild
			this.image3 = new global::Gtk.Image ();
			this.image3.Name = "image3";
			this.image3.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Wallet.Assets.misc.receive.png");
			this.hboxActions.Add (this.image3);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hboxActions [this.image3]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			this.hboxMain.Add (this.hboxActions);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hboxMain [this.hboxActions]));
			w13.PackType = ((global::Gtk.PackType)(1));
			w13.Position = 2;
			w13.Expand = false;
			w13.Fill = false;
			this.Add (this.hboxMain);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
