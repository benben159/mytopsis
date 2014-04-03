using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace mytopsis
{
	public class MainWindow : Form
	{
		private Label lbAltAttr;
		private TextBox txtNumAlt;
		private TextBox txtNumAttr;
		private Button btnGenHdr;
		private Button btnLockHdr;
		private List<TextBox> rowHeader;
		private List<TextBox> columnHeader;
		private List<List<TextBox>> matrix;
		public MainWindow ()
		{
			Text = "mytopsis";
			AutoSize = true;
			MaximizeBox = false;
			rowHeader = new List<TextBox> ();
			columnHeader = new List<TextBox> ();
			matrix = new List<List<TextBox>> ();
			initializeComponents ();
			bindEvents ();
			CenterToScreen ();
		}

		void initializeComponents ()
		{
			lbAltAttr = new Label ();
			lbAltAttr.Text = "alt x attr";
			lbAltAttr.Location = new Point (10, 10);
			lbAltAttr.Size = new Size (70, 30);
			lbAltAttr.Parent = this;
			txtNumAlt = new TextBox ();
			txtNumAlt.Size = new Size (30, 30);
			txtNumAlt.Location = new Point (10, 40);
			txtNumAlt.Parent = this;
			txtNumAttr = new TextBox ();
			txtNumAttr.Location = new Point (50, 40);
			txtNumAttr.Size = new Size (30, 30);
			txtNumAttr.Parent = this;
			btnGenHdr = new Button ();
			btnGenHdr.Text = "generate header";
			btnGenHdr.Location = new Point (90, 10);
			btnGenHdr.Size = new Size (80, 30);
			btnGenHdr.Parent = this;
			btnLockHdr = new Button ();
			btnLockHdr.Text = "lock header";
			btnLockHdr.Location = new Point (90, 50);
			btnLockHdr.Size = new Size (80, 30);
			btnLockHdr.Parent = this;
		}

		void bindEvents ()
		{
			btnGenHdr.Click += (object sender, EventArgs e) => 
			{
				int numcols=0, numrows= 0;
				try {
					numrows = int.Parse(txtNumAlt.Text);
					numcols = int.Parse(txtNumAttr.Text);
				} catch (Exception) {
					MessageBox.Show("invalid input");
					return;
				}
				for(int i = 0; i < numrows; i++){
					// TODO generate text box
				}
			};
		}



	}
}

