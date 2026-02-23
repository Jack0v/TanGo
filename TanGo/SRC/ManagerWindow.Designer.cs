namespace TanGo
{
	partial class ManagerWindow
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
			if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerWindow));
			this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.TextBoxPath = new System.Windows.Forms.TextBox();
			this.ButtonAdd = new System.Windows.Forms.Button();
			this.ButtonDel = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// CheckedListBox
			// 
			this.CheckedListBox.CausesValidation = false;
			this.CheckedListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.CheckedListBox.FormattingEnabled = true;
			this.CheckedListBox.HorizontalScrollbar = true;
			this.CheckedListBox.Location = new System.Drawing.Point(12, 12);
			this.CheckedListBox.Name = "CheckedListBox";
			this.CheckedListBox.ScrollAlwaysVisible = true;
			this.CheckedListBox.Size = new System.Drawing.Size(120, 148);
			this.CheckedListBox.TabIndex = 1;
			this.CheckedListBox.ThreeDCheckBoxes = true;
			this.CheckedListBox.UseCompatibleTextRendering = true;
			this.CheckedListBox.SelectedIndexChanged += new System.EventHandler(this.CheckedListBoxSelectedIndexChanged);
			// 
			// ButtonOK
			// 
			this.ButtonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonOK.Location = new System.Drawing.Point(138, 105);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(95, 25);
			this.ButtonOK.TabIndex = 6;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.FileName = "OpenFileDialog";
			this.OpenFileDialog.Multiselect = true;
			this.OpenFileDialog.SupportMultiDottedExtensions = true;
			// 
			// TextBoxPath
			// 
			this.TextBoxPath.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TextBoxPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.TextBoxPath.Location = new System.Drawing.Point(12, 167);
			this.TextBoxPath.Name = "TextBoxPath";
			this.TextBoxPath.ReadOnly = true;
			this.TextBoxPath.ShortcutsEnabled = false;
			this.TextBoxPath.Size = new System.Drawing.Size(221, 23);
			this.TextBoxPath.TabIndex = 7;
			this.TextBoxPath.TabStop = false;
			this.TextBoxPath.WordWrap = false;
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonAdd.Location = new System.Drawing.Point(138, 12);
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.Size = new System.Drawing.Size(95, 25);
			this.ButtonAdd.TabIndex = 2;
			this.ButtonAdd.Text = "Добавить...";
			this.ButtonAdd.UseVisualStyleBackColor = true;
			this.ButtonAdd.Click += new System.EventHandler(this.ButtonAddClick);
			// 
			// ButtonDel
			// 
			this.ButtonDel.Enabled = false;
			this.ButtonDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonDel.Location = new System.Drawing.Point(138, 43);
			this.ButtonDel.Name = "ButtonDel";
			this.ButtonDel.Size = new System.Drawing.Size(95, 25);
			this.ButtonDel.TabIndex = 3;
			this.ButtonDel.Text = "Исключить";
			this.ButtonDel.UseVisualStyleBackColor = true;
			this.ButtonDel.Click += new System.EventHandler(this.ButtonDelClick);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonCancel.Location = new System.Drawing.Point(138, 136);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(95, 25);
			this.ButtonCancel.TabIndex = 5;
			this.ButtonCancel.Text = "Отмена";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// ButtonRefresh
			// 
			this.ButtonRefresh.Enabled = false;
			this.ButtonRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ButtonRefresh.Location = new System.Drawing.Point(138, 74);
			this.ButtonRefresh.Name = "ButtonRefresh";
			this.ButtonRefresh.Size = new System.Drawing.Size(95, 25);
			this.ButtonRefresh.TabIndex = 4;
			this.ButtonRefresh.Text = "Сбросить...";
			this.ButtonRefresh.UseVisualStyleBackColor = true;
			this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefreshClick);
			// 
			// ManagerWindow
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(244, 201);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonRefresh);
			this.Controls.Add(this.ButtonDel);
			this.Controls.Add(this.ButtonAdd);
			this.Controls.Add(this.TextBoxPath);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.CheckedListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ManagerWindow";
			this.Text = "Менеджер словарей";
			this.Load += new System.EventHandler(this.ManagerWindowLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.OpenFileDialog OpenFileDialog;
		private System.Windows.Forms.TextBox TextBoxPath;
		private System.Windows.Forms.Button ButtonAdd;
		private System.Windows.Forms.Button ButtonDel;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonRefresh;
		private System.Windows.Forms.CheckedListBox CheckedListBox;
	}
}