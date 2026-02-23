namespace TanGo
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.IconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ToolStripMenuItemLaunch = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemManager = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
			this.Timer = new System.Windows.Forms.Timer(this.components);
			this.IconMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// NotifyIcon
			// 
			this.NotifyIcon.ContextMenuStrip = this.IconMenu;
			this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
			this.NotifyIcon.Text = "単語";
			this.NotifyIcon.Visible = true;
			this.NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIconDoubleClick);
			this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIconMouseClick);
			// 
			// IconMenu
			// 
			this.IconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLaunch,
            this.ToolStripMenuItemManager,
            this.ToolStripMenuItemSettings,
            this.ToolStripMenuItemAbout,
            this.ToolStripMenuItemExit});
			this.IconMenu.Name = "ContextMenuStrip1";
			this.IconMenu.ShowImageMargin = false;
			this.IconMenu.Size = new System.Drawing.Size(191, 124);
			// 
			// ToolStripMenuItemLaunch
			// 
			this.ToolStripMenuItemLaunch.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.ToolStripMenuItemLaunch.Name = "ToolStripMenuItemLaunch";
			this.ToolStripMenuItemLaunch.Size = new System.Drawing.Size(190, 24);
			this.ToolStripMenuItemLaunch.Text = "Пуск";
			this.ToolStripMenuItemLaunch.Click += new System.EventHandler(this.ToolStripMenuItemLaunchClick);
			// 
			// ToolStripMenuItemManager
			// 
			this.ToolStripMenuItemManager.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.ToolStripMenuItemManager.Name = "ToolStripMenuItemManager";
			this.ToolStripMenuItemManager.Size = new System.Drawing.Size(190, 24);
			this.ToolStripMenuItemManager.Text = "Менеджер словарей...";
			this.ToolStripMenuItemManager.Click += new System.EventHandler(this.ToolStripMenuItemManagerClick);
			// 
			// ToolStripMenuItemSettings
			// 
			this.ToolStripMenuItemSettings.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.ToolStripMenuItemSettings.Name = "ToolStripMenuItemSettings";
			this.ToolStripMenuItemSettings.Size = new System.Drawing.Size(190, 24);
			this.ToolStripMenuItemSettings.Text = "Настройки...";
			this.ToolStripMenuItemSettings.Click += new System.EventHandler(this.ToolStripMenuItemSettingsClick);
			// 
			// ToolStripMenuItemAbout
			// 
			this.ToolStripMenuItemAbout.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.ToolStripMenuItemAbout.Name = "ToolStripMenuItemAbout";
			this.ToolStripMenuItemAbout.Size = new System.Drawing.Size(190, 24);
			this.ToolStripMenuItemAbout.Text = "О программе...";
			this.ToolStripMenuItemAbout.Click += new System.EventHandler(this.ToolStripMenuItemAboutClick);
			// 
			// ToolStripMenuItemExit
			// 
			this.ToolStripMenuItemExit.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
			this.ToolStripMenuItemExit.Size = new System.Drawing.Size(190, 24);
			this.ToolStripMenuItemExit.Text = "Выход";
			this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExitClick);
			// 
			// Timer
			// 
			this.Timer.Interval = 5000;
			this.Timer.Tick += new System.EventHandler(this.TimerFinish);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Tan;
			this.ClientSize = new System.Drawing.Size(500, 200);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(100, 50);
			this.Name = "MainWindow";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "TanGo";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Tan;
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.Shown += new System.EventHandler(this.MainWindowShown);
			this.ResizeEnd += new System.EventHandler(this.MainWindowResizeEnd);
			this.Click += new System.EventHandler(this.MainWindowClick);
			this.DoubleClick += new System.EventHandler(this.MainWindowDoubleClick);
			this.Move += new System.EventHandler(this.MainWindowMove);
			this.Resize += new System.EventHandler(this.MainWindowResize);
			this.IconMenu.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.ContextMenuStrip IconMenu;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemManager;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLaunch;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSettings;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbout;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
		private System.Windows.Forms.Timer Timer;
		public System.Windows.Forms.NotifyIcon NotifyIcon;
	}
}

