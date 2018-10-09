using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace CtrlCloneTst
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button BtnCloneCB;
		private System.Windows.Forms.Button BtnCopyCB;
		private System.Windows.Forms.Button BtnClonePB;
		private System.Windows.Forms.Button BtnCopyPB;
		private System.Windows.Forms.Button BtnPaste;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.BtnCloneCB = new System.Windows.Forms.Button();
			this.BtnCopyCB = new System.Windows.Forms.Button();
			this.BtnClonePB = new System.Windows.Forms.Button();
			this.BtnCopyPB = new System.Windows.Forms.Button();
			this.BtnPaste = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(64, 24);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(160, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.Text = "comboBox1";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(488, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// BtnCloneCB
			// 
			this.BtnCloneCB.Location = new System.Drawing.Point(64, 456);
			this.BtnCloneCB.Name = "BtnCloneCB";
			this.BtnCloneCB.Size = new System.Drawing.Size(128, 23);
			this.BtnCloneCB.TabIndex = 2;
			this.BtnCloneCB.Text = "Clone comboBox1";
			this.BtnCloneCB.Click += new System.EventHandler(this.BtnCloneCB_Click);
			// 
			// BtnCopyCB
			// 
			this.BtnCopyCB.Location = new System.Drawing.Point(64, 208);
			this.BtnCopyCB.Name = "BtnCopyCB";
			this.BtnCopyCB.Size = new System.Drawing.Size(128, 23);
			this.BtnCopyCB.TabIndex = 3;
			this.BtnCopyCB.Text = "Copy comboBox1";
			this.BtnCopyCB.Click += new System.EventHandler(this.BtnCopyCB_Click);
			// 
			// BtnClonePB
			// 
			this.BtnClonePB.Location = new System.Drawing.Point(480, 456);
			this.BtnClonePB.Name = "BtnClonePB";
			this.BtnClonePB.Size = new System.Drawing.Size(128, 23);
			this.BtnClonePB.TabIndex = 4;
			this.BtnClonePB.Text = "Clone pictureBox1";
			this.BtnClonePB.Click += new System.EventHandler(this.BtnClonePB_Click);
			// 
			// BtnCopyPB
			// 
			this.BtnCopyPB.Location = new System.Drawing.Point(480, 208);
			this.BtnCopyPB.Name = "BtnCopyPB";
			this.BtnCopyPB.Size = new System.Drawing.Size(128, 23);
			this.BtnCopyPB.TabIndex = 5;
			this.BtnCopyPB.Text = "Copy pictureBox1";
			this.BtnCopyPB.Click += new System.EventHandler(this.BtnCopyPB_Click);
			// 
			// BtnPaste
			// 
			this.BtnPaste.Location = new System.Drawing.Point(296, 272);
			this.BtnPaste.Name = "BtnPaste";
			this.BtnPaste.Size = new System.Drawing.Size(128, 23);
			this.BtnPaste.TabIndex = 6;
			this.BtnPaste.Text = "Paste ";
			this.BtnPaste.Click += new System.EventHandler(this.BtnPaste_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(728, 526);
			this.Controls.Add(this.BtnPaste);
			this.Controls.Add(this.BtnCopyPB);
			this.Controls.Add(this.BtnClonePB);
			this.Controls.Add(this.BtnCopyCB);
			this.Controls.Add(this.BtnCloneCB);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.comboBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void BtnCloneCB_Click(object sender, System.EventArgs e)
		{
			Control ctrl = ControlFactory.CloneCtrl(this.comboBox1);
			
			this.Controls.Add(ctrl);
			ctrl.Text = "created by clone";	
			ctrl.SetBounds(ctrl.Bounds.X,ctrl.Bounds.Y+350,ctrl.Bounds.Width,ctrl.Bounds.Height);	
			ctrl.Show();
		}

		private void BtnCopyCB_Click(object sender, System.EventArgs e)
		{
			ControlFactory.CopyCtrl2ClipBoard(this.comboBox1);
		}

		private void BtnPaste_Click(object sender, System.EventArgs e)
		{
			Control ctrl = ControlFactory.GetCtrlFromClipBoard();
			
			this.Controls.Add(ctrl);
			ctrl.Text = "created by copy&paste";	
			ctrl.SetBounds(ctrl.Bounds.X,ctrl.Bounds.Y+100,ctrl.Bounds.Width,ctrl.Bounds.Height);	
			ctrl.Show();
		}

		private void BtnCopyPB_Click(object sender, System.EventArgs e)
		{
			ControlFactory.CopyCtrl2ClipBoard(this.pictureBox1);
		
		}

		private void BtnClonePB_Click(object sender, System.EventArgs e)
		{
			Control ctrl = ControlFactory.CloneCtrl(this.pictureBox1);
			
			this.Controls.Add(ctrl);
			ctrl.Text = "created by clone";	
			ctrl.SetBounds(ctrl.Bounds.X,ctrl.Bounds.Y+350,ctrl.Bounds.Width,ctrl.Bounds.Height);	
			ctrl.Show();
		
		}
	}
}
