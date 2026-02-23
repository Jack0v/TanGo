namespace TanGo
{
	partial class AboutWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
			this.ButtonOK = new System.Windows.Forms.Button();
			this.Label5 = new System.Windows.Forms.Label();
			this.PictureBox = new System.Windows.Forms.PictureBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.TextBox1 = new System.Windows.Forms.TextBox();
			this.TextBox2 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// ButtonOK
			// 
			this.ButtonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonOK.Location = new System.Drawing.Point(217, 249);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(100, 30);
			this.ButtonOK.TabIndex = 1;
			this.ButtonOK.Text = "ОК";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// Label5
			// 
			this.Label5.AutoSize = true;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this.Label5.Location = new System.Drawing.Point(12, 363);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(281, 24);
			this.Label5.TabIndex = 1;
			this.Label5.Text = "Для связи с общественостью:";
			// 
			// PictureBox
			// 
			this.PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox.Image")));
			this.PictureBox.Location = new System.Drawing.Point(40, 12);
			this.PictureBox.Name = "PictureBox";
			this.PictureBox.Size = new System.Drawing.Size(277, 134);
			this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PictureBox.TabIndex = 2;
			this.PictureBox.TabStop = false;
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("beautiful font", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Label1.Location = new System.Drawing.Point(12, 149);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(330, 97);
			this.Label1.TabIndex = 1;
			this.Label1.Text = "タンゴ";
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this.Label2.Location = new System.Drawing.Point(12, 246);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(99, 24);
			this.Label2.TabIndex = 1;
			this.Label2.Text = "v. 26.02.23";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this.Label3.Location = new System.Drawing.Point(12, 272);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(155, 24);
			this.Label3.TabIndex = 1;
			this.Label3.Text = "© Jack0v Co.LTD";
			// 
			// Label4
			// 
			this.Label4.AutoSize = true;
			this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.Label4.Location = new System.Drawing.Point(13, 307);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(278, 39);
			this.Label4.TabIndex = 1;
			this.Label4.Text = "Автор, ни при каких условиях, ни за какие последствия\r\nответственности не несёт.\r" +
    "\nИспользуй программу на свой страх и риск.\r\n";
			// 
			// TextBox1
			// 
			this.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this.TextBox1.Location = new System.Drawing.Point(17, 418);
			this.TextBox1.Name = "TextBox1";
			this.TextBox1.ReadOnly = true;
			this.TextBox1.ShortcutsEnabled = false;
			this.TextBox1.Size = new System.Drawing.Size(150, 22);
			this.TextBox1.TabIndex = 0;
			this.TextBox1.TabStop = false;
			this.TextBox1.Text = "Jack0v@mail.ru";
			this.TextBox1.WordWrap = false;
			// 
			// TextBox2
			// 
			this.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this.TextBox2.Location = new System.Drawing.Point(16, 390);
			this.TextBox2.Name = "TextBox2";
			this.TextBox2.ReadOnly = true;
			this.TextBox2.ShortcutsEnabled = false;
			this.TextBox2.Size = new System.Drawing.Size(160, 22);
			this.TextBox2.TabIndex = 0;
			this.TextBox2.TabStop = false;
			this.TextBox2.Text = "github.com/Jack0v";
			this.TextBox2.WordWrap = false;
			// 
			// AboutWindow
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(354, 451);
			this.Controls.Add(this.TextBox2);
			this.Controls.Add(this.TextBox1);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.PictureBox);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.Label5);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutWindow";
			this.Text = "О программе";
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Label Label5;
		private System.Windows.Forms.PictureBox PictureBox;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Label Label2;
		private System.Windows.Forms.Label Label3;
		private System.Windows.Forms.Label Label4;
		private System.Windows.Forms.TextBox TextBox1;
		private System.Windows.Forms.TextBox TextBox2;
	}
}