using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Mono.Data.Sqlite;
namespace mytopsis
{
	public class MainWindow : Form
	{
		private Label lbAltAttr;
		private TextBox txtNumAlt;
		private TextBox txtNumAttr;
		private Button btnGenHdr;
		private Button btnLockHdr;
		private Button btnStep1;
		private Button btnStep2;
		private Button btnStep3;
		private Button btnStep4;
		private Button btnStep5;
		private List<TextBox> rowHeader;
		private List<TextBox> columnHeader;
		private List<TextBox> columnWeight;
		private List<CheckBox> columnStatus;
		private List<TextBox> rowPositive;
		private List<TextBox> rowNegative;
		private List<TextBox> columnSPositive;
		private List<TextBox> columnSNegative;
		private List<List<TextBox>> matrix;
		private List<TextBox> columnC;
		private Point offsetRowHeader;
		private Point offsetColumnHeader;
		private Point offsetColumnWeight;
		private Point offsetColumnStatus;
		private Point offsetMatrix;
		private bool isHeaderLocked;

		private float[,] matrix2;
		private float[,] matrixStep1;
		private float[,] matrixStep2;
		private float[,] matrixStep4Positive;
		private float[,] matrixStep4Negative;
		private float[] bigSpositive;
		private float[] bigSnegative;
		private float[] weights;
		private float[] positive;
		private	float[] negative;

		public MainWindow ()
		{
			Text = "mytopsis";
			AutoSize = true;
			MaximizeBox = false;
			rowHeader = new List<TextBox> ();
			columnHeader = new List<TextBox> ();
			columnWeight = new List<TextBox> ();
			columnStatus = new List<CheckBox> ();
			matrix = new List<List<TextBox>> ();
			rowNegative = new List<TextBox> ();
			rowPositive = new List<TextBox> ();
			columnSPositive = new List<TextBox> ();
			columnSNegative = new List<TextBox> ();
			columnC = new List<TextBox> ();
			offsetRowHeader = new Point (20, 150);
			offsetColumnWeight = new Point (100, 120);
			offsetColumnStatus = new Point (140, 120);
			offsetColumnHeader = new Point (100, 90);
			offsetMatrix = new Point (100, 150);
			isHeaderLocked = false;
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
			btnLockHdr.Text = "lock";
			btnLockHdr.Location = new Point (90, 50);
			btnLockHdr.Size = new Size (80, 30);
			btnLockHdr.Parent = this;
			btnStep1 = new Button ();
			btnStep1.Parent = this;
			btnStep1.Location = new Point (190, 20);
			btnStep1.Size = new Size (50, 30);
			btnStep1.Text = "step 1";
			btnStep1.Enabled = false;
			btnStep2 = new Button ();
			btnStep2.Parent = this;
			btnStep2.Location = new Point (250, 20);
			btnStep2.Size = new Size (50, 30);
			btnStep2.Text = "step 2";
			btnStep2.Enabled = false;
			btnStep3 = new Button ();
			btnStep3.Parent = this;
			btnStep3.Location = new Point (310, 20);
			btnStep3.Size = new Size (50, 30);
			btnStep3.Text = "step 3";
			btnStep3.Enabled = false;
			btnStep4 = new Button ();
			btnStep4.Parent = this;
			btnStep4.Location = new Point (370, 20);
			btnStep4.Size = new Size (50, 30);
			btnStep4.Text = "step 4";
			btnStep4.Enabled = false;
			btnStep5 = new Button ();
			btnStep5.Parent = this;
			btnStep5.Location = new Point (430, 20);
			btnStep5.Size = new Size (50, 30);
			btnStep5.Text = "step 5";
			btnStep5.Enabled = false;
		}

		void bindEvents ()
		{
			btnGenHdr.Click += (object sender, EventArgs e) => {
				int numcols=0, numrows= 0;
				try {
					numrows = int.Parse(txtNumAlt.Text);
					numcols = int.Parse(txtNumAttr.Text);
				} catch (Exception) {
					MessageBox.Show("invalid input");
					return;
				}
				if (rowHeader.Count != 0){
					MessageBox.Show("clear matrix not implemented yet");
					return;
				}
				Label lbWeight = new Label ();
				lbWeight.Text = "Weight";
				lbWeight.Location = new Point (50, 120);
				lbWeight.Size = new Size (40, 30);
				lbWeight.Parent = this;
				for(int i = 0; i < numrows; i++){
					TextBox newTxt = new TextBox();
					newTxt.Text = string.Format("R_{0}", i+1);
					newTxt.Parent = this;
					newTxt.Location = offsetRowHeader;
					newTxt.Size = new Size(70, 30);
					offsetRowHeader.Y += 40;
					rowHeader.Add(newTxt);
				}
				Label lbPositive = new Label();
				lbPositive.Text = "Ideal";
				lbPositive.Parent = this;
				lbPositive.Location = offsetRowHeader;
				lbPositive.Size = new Size(70,30);
				offsetRowHeader.Y += 40;
				Label lbNegative = new Label();
				lbNegative.Text = "Neg. Ideal";
				lbNegative.Parent = this;
				lbNegative.Location = offsetRowHeader;
				lbNegative.Size = new Size(70,30);

				for(int i = 0; i < numcols; i++){
					TextBox newTxt = new TextBox();
					newTxt.Parent = this;
					newTxt.Location = offsetColumnHeader;
					newTxt.Size = new Size(70, 30);
					newTxt.Text = string.Format("C_{0}", i+1);
					offsetColumnHeader.X += 80;
					columnHeader.Add(newTxt);
					TextBox newTxtw = new TextBox();
					newTxtw.Parent = this;
					newTxtw.Location = offsetColumnWeight;
					newTxtw.Size = new Size(40, 30);
					offsetColumnWeight.X += 80;
					columnWeight.Add(newTxtw);
					CheckBox ck = new CheckBox();
					ck.Parent = this;
					ck.Location = offsetColumnStatus;
					ck.Size = new Size(30,30);
					ck.Text = "J'";
					offsetColumnStatus.X += 80;
					columnStatus.Add(ck);
				}

				Point offsetBigSPositive = new Point(offsetColumnHeader.X, offsetMatrix.Y);
				Point offsetBigSNegative = new Point(offsetColumnHeader.X+80, offsetMatrix.Y);
				Point offsetBigC = new Point(offsetColumnHeader.X+160, offsetMatrix.Y);
				Label spositive = new Label();
				spositive.Parent = this;
				spositive.Text = "S ideal";
				spositive.Location = offsetColumnWeight;
				spositive.Size = new Size(40,30);
				offsetColumnWeight.X += 80;
				Label snegative = new Label();
				snegative.Parent = this;
				snegative.Text = "S neg. ideal";
				snegative.Location = offsetColumnWeight;
				snegative.Size = new Size(40,30);
				offsetColumnWeight.X += 80;
				Label lblC = new Label();
				lblC.Parent = this;
				lblC.Text = "C";
				lblC.Location = offsetColumnWeight;
				lblC.Size = new Size(40,30);

				for (int i = 0; i < numrows; i++) {
					TextBox newTxt = new TextBox();
					newTxt.Parent = this;
					newTxt.Location = offsetBigSPositive;
					newTxt.Size = new Size(70,30);
					columnSPositive.Add(newTxt);
					offsetBigSPositive.Y += 40;
					TextBox newTxt2 = new TextBox();
					newTxt2.Parent = this;
					newTxt2.Location = offsetBigSNegative;
					newTxt2.Size = new Size(70,30);
					columnSNegative.Add(newTxt2);
					offsetBigSNegative.Y += 40;
					TextBox newC = new TextBox();
					newC.Parent = this;
					newC.Location = offsetBigC;
					newC.Size = new Size(70,30);
					columnC.Add(newC);
					offsetBigC.Y += 40;
				}

				for(int i = 0; i < numrows; i++){
					List<TextBox> newRow = new List<TextBox>();
					for(int j = 0; j < numcols; j++){
						TextBox element = new TextBox();
						element.Parent = this;
						element.Size = new Size(70, 30);
						element.Location = offsetMatrix;
						offsetMatrix.X += 80;
						newRow.Add(element);
					}
					offsetMatrix.Y += 40;
					offsetMatrix.X = 100;
					matrix.Add(newRow);
				}
				offsetMatrix.X = 100;

				for(int i = 0; i < numcols; i++){
					TextBox newTxt = new TextBox();
					newTxt.Parent = this;
					newTxt.Location = offsetMatrix;
					newTxt.Size = new Size(70,30);
					rowPositive.Add(newTxt);
					offsetMatrix.X += 80;
				}
				offsetMatrix.Y += 40;
				offsetMatrix.X = 100;

				for(int i = 0; i < numcols; i++){
					TextBox newTxt = new TextBox();
					newTxt.Parent = this;
					newTxt.Location = offsetMatrix;
					newTxt.Size = new Size(70,30);
					rowNegative.Add(newTxt);
					offsetMatrix.X += 80;
				}


			};

			btnLockHdr.Click += (object sender, EventArgs e) => {
				isHeaderLocked = !isHeaderLocked;
				foreach (TextBox t in rowHeader)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in columnHeader)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in columnWeight)
					t.ReadOnly = !t.ReadOnly;

				foreach (CheckBox c in columnStatus)
					c.Enabled = !c.Enabled;

				foreach (TextBox t in rowPositive)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in rowNegative)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in columnC)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in columnSPositive)
					t.ReadOnly = !t.ReadOnly;

				foreach (TextBox t in columnSNegative)
					t.ReadOnly = !t.ReadOnly;

				btnLockHdr.Text = isHeaderLocked ? "unlock" : "lock";
				btnStep1.Enabled = !btnStep1.Enabled;
				btnStep2.Enabled = !btnStep2.Enabled;
				btnStep3.Enabled = !btnStep3.Enabled;
				btnStep4.Enabled = !btnStep4.Enabled;
				btnStep5.Enabled = !btnStep5.Enabled;
				foreach (List<TextBox> r in matrix)
					foreach(TextBox t in r)
						t.ReadOnly = !t.ReadOnly;
			};

			btnStep1.Click += (object sender, EventArgs e) => {
				getMatrixValues();
				matrixStep1 = new float[rowHeader.Count,columnHeader.Count];
				float[] divider = new float[columnHeader.Count];
				for (int i = 0; i < columnHeader.Count; i++){
					for (int j = 0; j < rowHeader.Count; j++)
						divider[i] += (float)Math.Pow(matrix2[j,i], 2);						
					divider[i] = (float)Math.Sqrt(divider[i]);
				}
				for (int i = 0; i < columnHeader.Count; i++){
					for (int j = 0; j < rowHeader.Count; j++){
						matrixStep1[j,i] = matrix2[j,i] / divider[i];
						matrix[j][i].Text = matrixStep1[j,i].ToString();
					}
				}
			};

			btnStep2.Click += (object sender, EventArgs e) => {
				matrixStep2 = new float[rowHeader.Count,columnHeader.Count];
				for (int i = 0; i < columnHeader.Count; i++){
					for (int j = 0; j < rowHeader.Count; j++){
						matrixStep2[j,i] = matrixStep1[j,i] * weights[i];
						matrix[j][i].Text = matrixStep2[j,i].ToString();
					}
				}
			};

			btnStep3.Click += (object sender, EventArgs e) => {
				positive = new float[columnHeader.Count];
				negative = new float[columnHeader.Count];
				float[] r;
				for (int i = 0; i < columnHeader.Count; i++){
					r = new float[rowHeader.Count];
					for (int j = 0; j < rowHeader.Count; j++)
						r[j] = matrixStep2[j,i];
					if (columnStatus[i].Checked){
						positive[i] = r.Min();
						negative[i] = r.Max();
					} else {
						positive[i] = r.Max();
						negative[i] = r.Min();
					}
					rowPositive[i].Text = positive[i].ToString();
					rowNegative[i].Text = negative[i].ToString();
				}
			};

			btnStep4.Click += (object sender, EventArgs e) => {
				matrixStep4Positive = new float[rowHeader.Count,columnHeader.Count];
				matrixStep4Negative = new float[rowHeader.Count,columnHeader.Count];
				for (int i = 0; i < rowHeader.Count; i++)
					for (int j = 0; j < columnHeader.Count; j++) {
					matrixStep4Positive[i,j] = (float)Math.Pow(positive[j] - matrixStep2[i,j], 2);
					matrixStep4Negative[i,j] = (float)Math.Pow(negative[j] - matrixStep2[i,j], 2);
					}

				bigSpositive = new float[rowHeader.Count];
				bigSnegative = new float[rowHeader.Count];
				for (int i = 0; i < rowHeader.Count; i++)
					for (int j = 0; j < columnHeader.Count; j++) {
					bigSpositive[i] += matrixStep4Positive[i,j];
					bigSnegative[i] += matrixStep4Negative[i,j];
				}

				for (int i = 0; i < rowHeader.Count; i++){
					bigSpositive[i] = (float)Math.Sqrt(bigSpositive[i]);
					columnSPositive[i].Text = bigSpositive[i].ToString();
					bigSnegative[i] = (float)Math.Sqrt(bigSnegative[i]);
					columnSNegative[i].Text = bigSnegative[i].ToString();
				}
			};

			btnStep5.Click += (object sender, EventArgs e) => {
				for (int i = 0; i < rowHeader.Count; i++)
					columnC[i].Text = (bigSnegative[i]/(bigSpositive[i] + bigSnegative[i])).ToString();
			};
		}

		private void getMatrixValues() 
		{
			try {
				weights = new float[columnWeight.Count];
				for (int i = 0; i < columnWeight.Count; i++)
					weights [i] = float.Parse (columnWeight[i].Text);

				matrix2 = new float[rowHeader.Count,columnHeader.Count];
				for (int i = 0; i < rowHeader.Count; i++)
					for (int j = 0; j < columnHeader.Count; j++)
						matrix2 [i,j] = float.Parse (matrix [i][j].Text);
			} catch (Exception) {
				MessageBox.Show ("invalid input detected");
			}
		}
	}
}

