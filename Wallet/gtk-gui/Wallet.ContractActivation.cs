
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet
{
	public partial class ContractActivation
	{
		private global::Gtk.VBox vbox3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TextView txtContent;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label3;

		private global::Gtk.Label label1;

		private global::Gtk.SpinButton spinBlocks;

		private global::Gtk.Label labelKalapas;

		private global::Gtk.Label label5;

		private global::Gtk.HBox hboxAsset1;

		private global::Gtk.Label labelSelectSecureToken;

		private global::Gtk.ComboBox comboboxSecureToken;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Button buttonApply;

		private global::Gtk.HBox hboxStatus;

		private global::Gtk.Label labelStatus;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Wallet.ContractActivation
			global::Stetic.BinContainer.Attach(this);
			this.WidthRequest = 400;
			this.Name = "Wallet.ContractActivation";
			// Container child Wallet.ContractActivation.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.txtContent = new global::Gtk.TextView();
			this.txtContent.CanFocus = true;
			this.txtContent.Name = "txtContent";
			this.txtContent.Editable = false;
			this.GtkScrolledWindow.Add(this.txtContent);
			this.vbox3.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.GtkScrolledWindow]));
			w2.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.hbox1.Add(this.label3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label3]));
			w3.Position = 0;
			w3.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Blocks:");
			this.hbox1.Add(this.label1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label1]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.spinBlocks = new global::Gtk.SpinButton(0, 100, 1);
			this.spinBlocks.CanFocus = true;
			this.spinBlocks.Name = "spinBlocks";
			this.spinBlocks.Adjustment.PageIncrement = 10;
			this.spinBlocks.ClimbRate = 1;
			this.spinBlocks.Numeric = true;
			this.spinBlocks.Value = 1;
			this.hbox1.Add(this.spinBlocks);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.spinBlocks]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.labelKalapas = new global::Gtk.Label();
			this.labelKalapas.Name = "labelKalapas";
			this.hbox1.Add(this.labelKalapas);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.labelKalapas]));
			w6.Position = 3;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.hbox1.Add(this.label5);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label5]));
			w7.Position = 4;
			w7.Fill = false;
			this.vbox3.Add(this.hbox1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox1]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hboxAsset1 = new global::Gtk.HBox();
			this.hboxAsset1.Name = "hboxAsset1";
			this.hboxAsset1.BorderWidth = ((uint)(10));
			// Container child hboxAsset1.Gtk.Box+BoxChild
			this.labelSelectSecureToken = new global::Gtk.Label();
			this.labelSelectSecureToken.Name = "labelSelectSecureToken";
			this.labelSelectSecureToken.LabelProp = global::Mono.Unix.Catalog.GetString("Select secure token:");
			this.hboxAsset1.Add(this.labelSelectSecureToken);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hboxAsset1[this.labelSelectSecureToken]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			w9.Padding = ((uint)(10));
			// Container child hboxAsset1.Gtk.Box+BoxChild
			this.comboboxSecureToken = global::Gtk.ComboBox.NewText();
			this.comboboxSecureToken.Name = "comboboxSecureToken";
			this.hboxAsset1.Add(this.comboboxSecureToken);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hboxAsset1[this.comboboxSecureToken]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			w10.Padding = ((uint)(10));
			this.vbox3.Add(this.hboxAsset1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hboxAsset1]));
			w11.Position = 2;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Homogeneous = true;
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.buttonApply = new global::Gtk.Button();
			this.buttonApply.CanFocus = true;
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.UseUnderline = true;
			this.buttonApply.Label = global::Mono.Unix.Catalog.GetString("Button");
			this.hbox2.Add(this.buttonApply);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.buttonApply]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			this.vbox3.Add(this.hbox2);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox2]));
			w13.Position = 3;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hboxStatus = new global::Gtk.HBox();
			this.hboxStatus.Name = "hboxStatus";
			this.hboxStatus.Homogeneous = true;
			this.hboxStatus.Spacing = 6;
			// Container child hboxStatus.Gtk.Box+BoxChild
			this.labelStatus = new global::Gtk.Label();
			this.labelStatus.Name = "labelStatus";
			this.hboxStatus.Add(this.labelStatus);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hboxStatus[this.labelStatus]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			this.vbox3.Add(this.hboxStatus);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hboxStatus]));
			w15.Position = 4;
			w15.Expand = false;
			w15.Fill = false;
			this.Add(this.vbox3);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.hboxStatus.Hide();
			this.Hide();
		}
	}
}
