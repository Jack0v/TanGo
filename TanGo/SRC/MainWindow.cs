using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.IO;
using System.Diagnostics;
using static TanGo.ManagerWindow;
using System.Threading;

namespace TanGo
{    public partial class MainWindow : Form
	{	public MainWindow()
        {	InitializeComponent();
			//проверка на запуск второй копии
			bool CreatedNew;
			Mutex = new Mutex(true, "単語タンゴ", out CreatedNew);
			if(!CreatedNew)
			{	NotifyIcon.ShowBalloonTip(1000, "Программа TanGo уже запущена в системном трее", "Нельзя запустить две копии", ToolTipIcon.Info);
				Thread.Sleep(500);
				Environment.Exit(0);
			}
			SettingsWin		= new SettingsWindow	();
			ManagerWin		= new ManagerWindow		();
			BackgroundWin	= new BackgroundWindow	();
			AboutWin		= new AboutWindow		();
			//подключение к событию нажатия кнопки Сбросить в менеджере словарей
			ManagerWin.EventButtonRefreshClick		+= EventButtonRefreshHandler;
			//подключение к событию нажатия кнопки ОК в менеджере словарей
			ManagerWin.EventButtonOKClick			+= EventButtonOKPressHandler;
			ManagerWin.EventButtonOKClick			+= SettingsWin.SaveSettings;
			//подключение менеджера словарей к событию загрузки настроек из файла в окне настроек
			SettingsWin.EventLoadSettingsFromFile	+= ManagerWin.EventLoadSettingsFromFileHandler;
			//подключение менеджера словарей к событию нажатия кнопки ОК в окне настроек
			SettingsWin.EventButtonOKClick			+= ManagerWin.EventButtonOKHandler;
			//подключение к событию нажатия кнопки ОК и Отмена в окне настроек
			SettingsWin.EventButtonOKCancelClick	+= EventButtonOKCancelHandler;
			//подключение к событию нажатия кнопки именения размера в окне настроек
			SettingsWin.EventSettingsMode			+= EventSettingsModeHandler;
			//подключение окна настроек к событию выхода из режима настроек
			EventEscFromSettingsMode				+= SettingsWin.EscFromSettingsModeHandler;
			this.Owner = BackgroundWin;				//чтобы главное окно было поверх фонового
			SettingsWin.ReadSettingsFile();
			SetSettings();
			//автоматическая перерисовка
			this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
		}

		event Action<bool, Rectangle, PointF, PointF, PointF, PointF> EventEscFromSettingsMode;

		SettingsWindow		SettingsWin;
		ManagerWindow		ManagerWin;
		BackgroundWindow	BackgroundWin;
		AboutWindow			AboutWin;
	
		Random Random = new Random();

		Mutex Mutex;
		int NumberRecord;
		int NumberDictionary	= -2;
		bool DialogMode			= false;
		bool ShowValue			= false;
		bool Work				= false;
		public const char Tilda	= '~';
		public const char Marker= '@';
		Rectangle WordBounds  , ValueBounds;
		PointF	  WordPosition, ValuePosition;
		PointF	  WordBind	  , ValueBind;
		
		public class Record
		{	public int NumberLine;
			public string Word;
			public string Value;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{	base.OnFormClosed(e);
			if (Mutex != null)
			{	Mutex.ReleaseMutex();
				Mutex = null;
			}
		}

		void SetSettings()
		{	
//https://stackoverflow.com/questions/4448771/c-sharp-form-transparencykey-working-different-for-different-colors-why
			//окно становится прозрачным для мыши когда красный==синий
			//цвет прозрачности для главного окна не должен совпадать с другими цветами
			int Blue = 1;
			int WindowColor = unchecked((int)0xFF000001);
						
			for(bool IncDec = true; IncDec;)
			{	IncDec = false;
				if(WindowColor == SettingsWin.SettingsCurrent.SeparatorColor)
				{	Blue++;
					WindowColor = WindowColor & ~0xFF | Blue;
					IncDec = true;
				}
				if(WindowColor == SettingsWin.SettingsCurrent.WordFontColor)
				{	Blue++;
					WindowColor = WindowColor & ~0xFF | Blue;
					IncDec = true;
				}
				if(WindowColor == SettingsWin.SettingsCurrent.WordLineColor)
				{	Blue++;
					WindowColor = WindowColor & ~0xFF | Blue;
					IncDec = true;
				}
				if(WindowColor == SettingsWin.SettingsCurrent.ValueFontColor)
				{	Blue++;
					WindowColor = WindowColor & ~0xFF | Blue;
					IncDec = true;
				}
				if(WindowColor == SettingsWin.SettingsCurrent.ValueLineColor)
				{	Blue++;
					WindowColor = WindowColor & ~0xFF | Blue;
					IncDec = true;
				}
			}
			this.BackColor			= Color.FromArgb(WindowColor);
			this.TransparencyKey	= this.BackColor;
			this.Bounds				= SettingsWin.SettingsCurrent.WindowBounds;	
			WordBind				= SettingsWin.SettingsCurrent.WordBind;
			ValueBind				= SettingsWin.SettingsCurrent.ValueBind;
			WordPosition			= SettingsWin.SettingsCurrent.WordLocation;
			ValuePosition			= SettingsWin.SettingsCurrent.ValueLocation;
			BackgroundWin.Bounds	= this.Bounds;
			BackgroundWin.Radius	= SettingsWin.SettingsCurrent.WindowRadius;
			BackgroundWin.Color		= Color.FromArgb(SettingsWin.SettingsCurrent.WindowColor);
			BackgroundWin.Opacity	= SettingsWin.SettingsCurrent.WindowVisibility / 100f;
			Blue					= SettingsWin.SettingsCurrent.WindowColor & 0xFF;
			Blue					+= Blue==255? -1 : +1;
			WindowColor				= SettingsWin.SettingsCurrent.WindowColor & ~0xFF | Blue;
			BackgroundWin.BackColor	= Color.FromArgb(WindowColor);
			BackgroundWin.TransparencyKey = BackgroundWin.BackColor;
			Work = SettingsWin.SettingsCurrent.Work;
			if(Work)	IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пауза";
			else		IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пуск";
			TimerStart();
		}

		Rectangle DrawText(PaintEventArgs e, string WordOrValue, string Text)
		{	PointF TextPosition, Bind;
			Font Font;
			Color TextColor, OutlineColor;
			float OutlineWidth;
			switch(WordOrValue)
			{	case "Word":
					TextPosition = WordPosition;
					Bind		 = WordBind;
					Font		 = SettingsWin.SettingsCurrent.WordFont;
					TextColor	 = Color.FromArgb(SettingsWin.SettingsCurrent.WordFontColor);
					OutlineColor = Color.FromArgb(SettingsWin.SettingsCurrent.WordLineColor);
					OutlineWidth = SettingsWin.SettingsCurrent.WordLineSize;
					break;
				case "Value":
					TextPosition = ValuePosition;
					Bind		 = ValueBind;
					Font		 = SettingsWin.SettingsCurrent.ValueFont;
					TextColor	 = Color.FromArgb(SettingsWin.SettingsCurrent.ValueFontColor);
					OutlineColor = Color.FromArgb(SettingsWin.SettingsCurrent.ValueLineColor);
					OutlineWidth = SettingsWin.SettingsCurrent.ValueLineSize;
					break;
				default: throw new InvalidOperationException($"Значение {WordOrValue} для \"WordValue\", недопустимо и должно быть или \"Word\" или \"Vaue\"");
			}
			//по каким-то странным причинам текст рисуется с некоторым смещением в плюс относительно указанной позиции которое зависит от размера шрифта
			StringFormat StringFormat = StringFormat.GenericTypographic;
			StringFormat.Alignment = StringAlignment.Center;
			RectangleF BoundsF;
			Size Amendment = new Size();
			using(GraphicsPath Path = new GraphicsPath())
			{	Path.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, new Point(0, 0), StringFormat);
				BoundsF = Path.GetBounds();
				//округление
				Amendment.Width	 = (BoundsF.Location.X >= (int)BoundsF.Location.X+0.5f)? (int)BoundsF.Location.X+1 : (int)BoundsF.Location.X;
				Amendment.Height = (BoundsF.Location.Y >= (int)BoundsF.Location.Y+0.5f)? (int)BoundsF.Location.Y+1 : (int)BoundsF.Location.Y;
			}
			Point Position = new Point();
			Position.X = (int)(TextPosition.X * this.Bounds.Width  - BoundsF.Width  * Bind.X);
			Position.Y = (int)(TextPosition.Y * this.Bounds.Height - BoundsF.Height * Bind.Y);
			//поправка
			Position -= Amendment;
			//путь для текста
			using(GraphicsPath Path = new GraphicsPath())
			{   //текст в путь
				Path.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, Position, StringFormat);
				//сглаживание
				e.Graphics.SmoothingMode	= SmoothingMode.AntiAlias;
				e.Graphics.PixelOffsetMode	= PixelOffsetMode.HighQuality;
				//обводка текста
				using(Pen OutlinePen = new Pen(OutlineColor, OutlineWidth))
				{	OutlinePen.LineJoin = LineJoin.Round; // скругление
					e.Graphics.DrawPath(OutlinePen, Path);
				}
				//заливка текста
				using(Brush TextBrush = new SolidBrush(TextColor))
				{	e.Graphics.FillPath(TextBrush, Path);
				}
				//положение и размер текста
				BoundsF = Path.GetBounds();
				//округление
				Rectangle Bounds = new Rectangle();
				Bounds.X		= (BoundsF.Location.X >= (int)BoundsF.Location.X+0.5f)? (int)BoundsF.Location.X+1 : (int)BoundsF.Location.X;
				Bounds.Y		= (BoundsF.Location.Y >= (int)BoundsF.Location.Y+0.5f)? (int)BoundsF.Location.Y+1 : (int)BoundsF.Location.Y;
				Bounds.Width	= (BoundsF.Width	  >= (int)BoundsF.Width		+0.5f)? (int)BoundsF.Width	   +1 : (int)BoundsF.Width;
				Bounds.Height	= (BoundsF.Height	  >= (int)BoundsF.Height	+0.5f)? (int)BoundsF.Height	   +1 : (int)BoundsF.Height;
				if(SettingsMode)
				{	Color Color = Color.FromArgb(~SettingsWin.SettingsCurrent.WindowColor | unchecked((int)0xFF000000));
					e.Graphics.DrawRectangle(new Pen(Color, 1), Bounds);
				}
				StringFormat?.Dispose();
				return Bounds;
			}
		}
		void DrawSeparator(PaintEventArgs e)
		{	int Center = (WordBounds.Bottom + ValueBounds.Top) / 2;
			int Y = Center - SettingsWin.SettingsCurrent.SeparatorSize / 2;
			float AbsRadius = BackgroundWin.AbsRadius / 2;
			int X0, X1;
			if(Y < AbsRadius)
			{	float Sin = (AbsRadius-Y) / AbsRadius;
				float Cos = (float)Math.Sqrt(1 - Math.Pow(Sin, 2));
				Cos *= AbsRadius;
				X0 = (int)(AbsRadius - Cos);
				X1 = (int)(BackgroundWin.Bounds.Width - AbsRadius + Cos);
			}
			else if(Y > BackgroundWin.Bounds.Height-AbsRadius)
			{
				float Sin = (Y-(BackgroundWin.Bounds.Height-AbsRadius)) / AbsRadius;
				float Cos = (float)Math.Sqrt(1 - Math.Pow(Sin, 2));
				Cos *= AbsRadius;
				X0 = (int)(AbsRadius - Cos);
				X1 = (int)(BackgroundWin.Bounds.Width - AbsRadius + Cos);

			}
			else
			{	X0 = 0;
				X1 = this.Bounds.Width;
			}
			using(Pen Pen = new Pen(Color.FromArgb(SettingsWin.SettingsCurrent.SeparatorColor), SettingsWin.SettingsCurrent.SeparatorSize))
				e.Graphics.DrawLine(Pen, new Point(X0, Y), new Point(X1, Y));
		}

		protected override void OnPaint(PaintEventArgs e)
		{	if(NumberDictionary>=0 || SettingsMode)  //на случай если удалить слвоарь во время показа
			{	string WordText, ValueText;
				if(SettingsMode)
				{	WordText		= "{Слово}";
					ValueText		= "{Значение}";
				}
				else
				{	WordText		= ManagerWin.Dictionaries[NumberDictionary].Records[NumberRecord].Word;
					ValueText		= ManagerWin.Dictionaries[NumberDictionary].Records[NumberRecord].Value;
				}
				//слово
					WordBounds	= DrawText(e: e, WordOrValue: "Word" , Text: WordText );
				//значение
				if(ShowValue || SettingsMode)
				{	ValueBounds = DrawText(e: e, WordOrValue: "Value", Text: ValueText);
					DrawSeparator(e);
				}
			}
		}

		void HideToTray()
		{	this.Hide();
			BackgroundWin.Hide();
			ShowValue = false;
		}

		void TimerStart()
		{	if(!Work	 ) return;
			if(DialogMode) return;
			int Interval;
			if(this.Visible)
			{	Interval = (SettingsWin.SettingsCurrent.AutoHideM*60 + SettingsWin.SettingsCurrent.AutoHideS) * 1000;
				if(Interval==0)
				{	Timer.Stop();
					return;
				}
			}
			else
			{	Interval = (SettingsWin.SettingsCurrent.IntervalM*60 + SettingsWin.SettingsCurrent.IntervalS) * 1000;
				Interval = Interval!=0? Interval : 1;
			}
			Timer.Interval = Interval;
			Timer.Start();
		}

		bool RestoreFromTray()
		{	if(!SettingsMode)
			{	if(ManagerWin.Dictionaries.Count == 0 || NumberDictionary == -2)
				{	NotifyIcon.ShowBalloonTip(1000, "Нет словарей", "Словари не подключены или имеют неверный формат", ToolTipIcon.Info);
					return false;
				}
				if(NumberDictionary == -1)
				{	NotifyIcon.ShowBalloonTip(1000, "Нет активных словарей", "Ни один словарь не отмечен", ToolTipIcon.Info);	
					return false;
				}
			}
			BackgroundWin.Show();
			BackgroundWin.WindowState	= FormWindowState.Normal;
			this.Show();
			this.WindowState			= FormWindowState.Normal;
			this.BringToFront();
			if(SettingsMode)
			{	this.Activate();
				this.Focus();
			}
			this.Refresh();
			TimerStart();
			return true;
		}

		//добавление записи
		bool AddRecords(List<Record> Records, string [] Lines)
		{	bool TrueTilda = false;
			for(int i=0; i<Lines.Length; i++)
			{	int TildaPointer = Lines[i].IndexOf(Tilda);
				//если тильда ненайдена или в начале или в конце строки - неверный формат строки
				if(TildaPointer<=0 || TildaPointer==Lines[i].Length-1 || (TildaPointer==Lines[i].Length-2 && Lines[i][Lines[i].Length-1] == Marker))
					continue;
				TrueTilda = true;
				if(Lines[i][Lines[i].Length-1] == Marker) continue;	//если в конце строки есть признак - не добавлять
				string Word  = Lines[i].Substring(0, TildaPointer);
				Word = Word.Replace("`", "\r\n");
				string Value = Lines[i].Substring(TildaPointer+1, Lines[i].Length-(TildaPointer+1));
				Value = Value.Replace("`", "\r\n");
				Record Record = new Record	{	Word    = Word.Trim(),
												Value   = Value.Trim(),
												NumberLine  = i
											};
				Records.Add(Record);
			}
			return TrueTilda;
		}

		//обнуление словаря
		public void RefreshDictionary(Dictionary Dictionary)
		{	for(int i=0; i<Dictionary.Lines.Length; i++)
			{	if(Dictionary.Lines[i].Length != 0)
				{	if(Dictionary.Lines[i][Dictionary.Lines[i].Length-1] == Marker && Dictionary.Lines[i][0] != Tilda)
						Dictionary.Lines[i] = Dictionary.Lines[i].Substring(0, Dictionary.Lines[i].Length-1);
				}
			}
			AddRecords(Dictionary.Records, Dictionary.Lines);
			try		{ File.WriteAllLines(Dictionary.FileName, Dictionary.Lines, Encoding.UTF8); }
			catch	{ NotifyIcon.ShowBalloonTip(1000, "Невозможно получить доступ к файлу словаря", $"\"{Path.GetFileName(Dictionary.FileName)}\"", ToolTipIcon.Error); }
		}

		//подготовка следующего слова
		void NextWord()
		{	int N = NumberDictionary;
			//выдавать только помеченные словари
			for(;;)
			{	NumberDictionary++;
				if(NumberDictionary == ManagerWin.Dictionaries.Count)
					NumberDictionary = 0;
				if(!ManagerWin.Dictionaries[NumberDictionary].Checked)
				{	if(NumberDictionary == N)
					{	NumberDictionary = -1;
						return;
					}
					continue;
				}
				break;
			}
			if(ManagerWin.Dictionaries[NumberDictionary].Records.Count == 0)
				RefreshDictionary(ManagerWin.Dictionaries[NumberDictionary]);
			//генерация случайного числа в диапазоне от (включительно) до (исключительно)
			NumberRecord = Random.Next(0, ManagerWin.Dictionaries[NumberDictionary].Records.Count);
			return;
		}


		//обработка нажатия кнопки Сбросить в менеджере словарей
		void EventButtonRefreshHandler(Dictionary Dictionary)
		{	RefreshDictionary(Dictionary);
		}

		//обработка нажатия кнопки ОК в менеджере словарей
		void EventButtonOKPressHandler(List<Dictionary> Dictionaries, bool Button, bool NotAllFound)
		{	HideToTray();
			if(!Button && NotAllFound)
				NotifyIcon.ShowBalloonTip(1000, "Не все словари загружены", "Отсутствует доступ", ToolTipIcon.Warning);
			NumberDictionary = -2;
			foreach(Dictionary Dictionary in Dictionaries)
			{	Dictionary.Records = null;
				string [] Lines = null;
				DialogResult Result = DialogResult.Retry;
				for(; Result==DialogResult.Retry; )
				{	try
					{	Lines = File.ReadAllLines(Dictionary.FileName, Encoding.UTF8);
						Result = DialogResult.OK;
					}
					catch(Exception ex)
					{	if(Button)	Result = MessageBox.Show($"Невозможно получить доступ к словарю\r\nИсключение: " + ex.GetType().Name + "\r\n" + ex.Message,
																"Ошибка", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
						else		Result = DialogResult.Cancel;
					}
				}
				if(Result == DialogResult.Cancel)
					continue;
				List<Record> Records = new List<Record>();
				bool TrueFormat = AddRecords(Records, Lines);
				if(TrueFormat)
				{	Dictionary.Lines = Lines;
					Dictionary.Records = Records;
					if(Dictionary.Records.Count == 0)
						RefreshDictionary(Dictionary);
					if(NumberDictionary < -1)
					{	NumberDictionary = -1;
						if(Dictionary.Checked)
							NumberDictionary = 0;
					}
				}
			}
			if(NumberDictionary > -1)
				NextWord();
		}

		void WindowClick(MouseEventArgs e)
		{	if(NumberDictionary < 0)	 //на случай если удалить словарь во время показа
			{	HideToTray();
				Timer.Stop();
				IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пуск";
				Work = !Work;
				return;
			}
			if(!ShowValue)
			{	ShowValue = true;
				this.Refresh();
			}
			else
			{	HideToTray();
				if(	 SettingsWin.SettingsCurrent.GuessLeft && e.Button == MouseButtons.Left ||
					!SettingsWin.SettingsCurrent.GuessLeft && e.Button == MouseButtons.Right)
				{	//пометка слова как использованое
					ManagerWin.Dictionaries[NumberDictionary].Lines[ManagerWin.Dictionaries[NumberDictionary].Records[NumberRecord].NumberLine] =
					ManagerWin.Dictionaries[NumberDictionary].Lines[ManagerWin.Dictionaries[NumberDictionary].Records[NumberRecord].NumberLine] + Marker;
					try		{ File.WriteAllLines(ManagerWin.Dictionaries[NumberDictionary].FileName, ManagerWin.Dictionaries[NumberDictionary].Lines, Encoding.UTF8); }
					catch	{ NotifyIcon.ShowBalloonTip(1000, "Невозможно получить доступ к файлу словаря", $"\"{ManagerWin.Dictionaries[NumberDictionary].FileName}\"", ToolTipIcon.Error); }
					//удаление записи
					ManagerWin.Dictionaries[NumberDictionary].Records.RemoveAt(NumberRecord);
				}
				//подготовка следующего слова
				NextWord();
				TimerStart();
			}
		}

		void DisableMenu()
		{	DialogMode = true;
			Timer.Stop();
			IconMenu.Items["ToolStripMenuItemLaunch"  ].Enabled = false;
			IconMenu.Items["ToolStripMenuItemManager" ].Enabled = false;
			IconMenu.Items["ToolStripMenuItemSettings"].Enabled = false;
			IconMenu.Items["ToolStripMenuItemAbout"	  ].Enabled = false;
		}
		void EnableMenu()
		{	DialogMode = false;
			TimerStart();
			IconMenu.Items["ToolStripMenuItemLaunch"  ].Enabled = true;
			IconMenu.Items["ToolStripMenuItemManager" ].Enabled = true;
			IconMenu.Items["ToolStripMenuItemSettings"].Enabled = true;
			IconMenu.Items["ToolStripMenuItemAbout"	  ].Enabled = true;
		}
		
		private void MainWindowShown(object sender, EventArgs e) => HideToTray();

		private void MainWindowClick(object sender, EventArgs e)
		{	if(SettingsMode)
				return;
			if(!SettingsWin.SettingsCurrent.ClickDouble)
				WindowClick((MouseEventArgs)e);
		}
		private void MainWindowDoubleClick(object sender, EventArgs e)
		{	if(SettingsMode)
			{	EscFromSettingsMode(true);
				return;
			}
			if(SettingsWin.SettingsCurrent.ClickDouble)
				WindowClick((MouseEventArgs)e);
		}
		
		void NotifyIconMouseClick(object sender, MouseEventArgs e)
		{	if(DialogMode)
				return;
			if(e.Button == MouseButtons.Left)
				if(!SettingsWin.SettingsCurrent.ClickDouble)
					RestoreFromTray();
		}
		void NotifyIconDoubleClick(object sender, EventArgs e)
        {	if(DialogMode)
				return;
			if(((MouseEventArgs)e).Button == MouseButtons.Right)
				return;
			if(SettingsWin.SettingsCurrent.ClickDouble)
				RestoreFromTray();
		}
		
		void ToolStripMenuItemLaunchClick(object sender, EventArgs e)
		{	if(!Work)
			{	if(!RestoreFromTray())
					return;
				IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пауза";
				Work = !Work;
			}
			else
			{	Timer.Stop();
				HideToTray();
				IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пуск";
				Work = !Work;
			}
			SettingsWin.SettingsCurrent.Work = Work;
			SettingsWin.SaveSettings(ManagerWin.Dictionaries, true, false);
		}
		void ToolStripMenuItemManagerClick	(object sender, EventArgs e)
		{	DisableMenu();
			ManagerWin.ShowDialog();
			EnableMenu();
		}
		void ToolStripMenuItemSettingsClick	(object sender, EventArgs e)
		{	DisableMenu();
			SettingsWin.ShowDialog();
		}
		void ToolStripMenuItemAboutClick	(object sender, EventArgs e)
		{	DisableMenu();
			AboutWin.ShowDialog();
			EnableMenu();
		}
		void ToolStripMenuItemExitClick		(object sender, EventArgs e)
		{	NotifyIcon.Visible = false;
			NotifyIcon.Dispose();
			Application.Exit();
		}

		void TimerFinish(object sender, EventArgs e)
		{	if(this.Visible)
			{	HideToTray();
				NextWord();
				TimerStart();
			}
			else
				if(!RestoreFromTray())
				{	Timer.Stop();
					IconMenu.Items["ToolStripMenuItemLaunch"].Text = "Пуск";
					Work = !Work;
				}
		}

		///обработчики режима настроек
		bool SettingsMode = false;
		bool CursorOverWin = true;
		int Grab;
		Point MouseLocation;
		//возможность изменения размера окна и блокирование выхода за пределы рабочего стола
		[StructLayout(LayoutKind.Sequential)]
		private struct WindowPos	//структура WindowPos из WinAPI
		{	public IntPtr Hwnd;
			public IntPtr HwndInsertAfter;
			public int X;
			public int Y;
			public int CX;
			public int CY;
			public uint Flags;
		}
		protected override void WndProc(ref Message Message)
		{	base.WndProc(ref Message);
			if(!SettingsMode ) return;
			if(!CursorOverWin) return;
			//блокирование выхода окна за пределы рабочего стола
			const int WM_WINDOWPOSCHANGING = 0x46;
    		const int SWP_NOMOVE		   = 0x02; 
			if(Message.Msg==WM_WINDOWPOSCHANGING && this.WindowState==FormWindowState.Normal)
			{	//структура WindowPos из LParam
				WindowPos windowPos = (WindowPos)Marshal.PtrToStructure(Message.LParam, typeof(WindowPos));
				if ((windowPos.Flags & SWP_NOMOVE) == 0)
				{	//объединенная область всех мониторов
					Rectangle AllBounds = SystemInformation.VirtualScreen;
            		//ограничиние позиции
					windowPos.X = Math.Max(AllBounds.Left,	Math.Min(windowPos.X, AllBounds.Right  - windowPos.CX));
					windowPos.Y = Math.Max(AllBounds.Top,	Math.Min(windowPos.Y, AllBounds.Bottom - windowPos.CY));
            		//возврат структуры
					Marshal.StructureToPtr(windowPos, Message.LParam, true);
				}
			}
			//возможность изменения размера окна
			const int BorderWidth	= 5;
			const int WM_NCHITTEST	= 0x0084;
			const int HTCLIENT		= 1;
			const int HTLEFT		= 10;
			const int HTRIGHT		= 11;
			const int HTTOP			= 12;
			const int HTTOPLEFT		= 13;
			const int HTTOPRIGHT	= 14;
			const int HTBOTTOM		= 15;
			const int HTBOTTOMLEFT	= 16;
			const int HTBOTTOMRIGHT	= 17;
			if(Message.Msg==WM_NCHITTEST && (int)Message.Result==HTCLIENT)
			{	int X = (short)( Message.LParam.ToInt64()		& 0xFFFF);
				int Y = (short)((Message.LParam.ToInt64()>>16)	& 0xFFFF);
				Point Point = this.PointToClient(new Point(X, Y));
				if (Point.X<=BorderWidth)
				{	if		(Point.Y<=BorderWidth)					Message.Result = (IntPtr)HTTOPLEFT;
					else if (Point.Y>=ClientSize.Height-BorderWidth)Message.Result = (IntPtr)HTBOTTOMLEFT;
					else											Message.Result = (IntPtr)HTLEFT;
				}
				else if(Point.X>=ClientSize.Width-BorderWidth)
				{	if		(Point.Y<=BorderWidth)					Message.Result = (IntPtr)HTTOPRIGHT;
					else if (Point.Y>=ClientSize.Height-BorderWidth)Message.Result = (IntPtr)HTBOTTOMRIGHT;
					else											Message.Result = (IntPtr)HTRIGHT;
				}
				else if(Point.Y<=BorderWidth)
				{	Message.Result = (IntPtr)HTTOP;
				}
				else if(Point.Y>=ClientSize.Height-BorderWidth)
				{	Message.Result = (IntPtr)HTBOTTOM;
				}
			}
		}

		void EventSettingsModeHandler(object sender, EventArgs e)
		{	this.KeyUp		+= new KeyEventHandler	(this.MainWindowKeyUp);
			this.MouseDown	+= new MouseEventHandler(this.MainWindowMouseDown);
			this.MouseMove	+= new MouseEventHandler(this.MainWindowMouseMove);
			this.MouseUp	+= new MouseEventHandler(this.MainWindowMouseUp);
			HideToTray();
			SettingsWin.Hide();
			SettingsMode = true;
			SetSettings();
			RestoreFromTray();
			Cursor.Clip = this.Bounds;
		}

		void EscFromSettingsMode(bool Apply)
		{	SettingsMode = false;
			this.KeyUp		-= new KeyEventHandler	(this.MainWindowKeyUp);
			this.MouseDown	-= new MouseEventHandler(this.MainWindowMouseDown);
			this.MouseMove	-= new MouseEventHandler(this.MainWindowMouseMove);
			this.MouseUp	-= new MouseEventHandler(this.MainWindowMouseUp);
			Cursor.Clip = Rectangle.Empty;
			this.Cursor = Cursors.Default;
			HideToTray();
			EventEscFromSettingsMode?.Invoke(Apply, this.Bounds, WordPosition, ValuePosition, WordBind, ValueBind);
			SettingsWin.ShowDialog();
		}

		void EventButtonOKCancelHandler(object sender, EventArgs e)
		{	SetSettings();
			EnableMenu();
		}

		private void MainWindowKeyUp(object sender, KeyEventArgs e)
		{	if(e.KeyCode==Keys.Escape)
			{	EscFromSettingsMode(false);
				return;
			}
			if(e.KeyCode==Keys.Space || e.KeyCode==Keys.Enter)
			{	EscFromSettingsMode(true);
				return;
			}
		}

		private void MainWindowMouseDown(object sender, MouseEventArgs e)
		{	if (e.Button == MouseButtons.Left)
			{	Cursor.Clip = this.Bounds;
				this.Cursor	= Cursors.NoMove2D;
				double DistanceToWord = 0, DistanceToValue = 0;
				Grab = 0;		//указатьель попал на окно
				if(WordBounds.Contains(e.Location))
				{	Grab = 1;	//указатьель попал на слово
					Point Center = new Point((WordBounds.Left + WordBounds.Right) / 2, (WordBounds.Top + WordBounds.Bottom) / 2);
					int X = Center.X - e.X;
					int Y = Center.Y - e.Y;
					DistanceToWord = Math.Sqrt(X*X + Y*Y);
					WordBind.X = (float)(e.X - WordBounds.X) / (float)WordBounds.Width;
					WordBind.Y = (float)(e.Y - WordBounds.Y) / (float)WordBounds.Height;
				}
				if(ValueBounds.Contains(e.Location))
				{	Grab = 2;	//указатьель попал на значение
					Point Center = new Point((ValueBounds.Left + ValueBounds.Right) / 2, (ValueBounds.Top + ValueBounds.Bottom) / 2);
					int X = Center.X - e.X;
					int Y = Center.Y - e.Y;
					DistanceToValue = Math.Sqrt(X*X + Y*Y);
					ValueBind.X = (float)(e.X - ValueBounds.X) / (float)ValueBounds.Width;
					ValueBind.Y = (float)(e.Y - ValueBounds.Y) / (float)ValueBounds.Height;
				}
				if(WordBounds.Contains(e.Location) && ValueBounds.Contains(e.Location))
					Grab = DistanceToWord<DistanceToValue? 1 : 2;
				if(Grab == 0)
					MouseLocation = new Point(-e.X, -e.Y);
			}
		}

		private void MainWindowMouseMove(object sender, MouseEventArgs e)
		{	if(WordBounds.Contains(e.Location) || ValueBounds.Contains(e.Location))
				CursorOverWin = false;
			else
				CursorOverWin = true;
			if(e.Button==MouseButtons.Left)
			{	switch(Grab)
				{	case 0:	//если схвачено окно
						Point MousePosition = Control.MousePosition;
						MousePosition.Offset(MouseLocation);
						this.Location = MousePosition;
						break;
					case 1:	//если схвачено слово
						WordPosition.X = (float)e.Location.X / (float)this.Bounds.Width;
						WordPosition.Y = (float)e.Location.Y / (float)this.Bounds.Height;
						this.Invalidate();
						break;
					case 2:	//если схвачено значение
						ValuePosition.X = (float)e.Location.X / (float)this.Bounds.Width;
						ValuePosition.Y = (float)e.Location.Y / (float)this.Bounds.Height;
						this.Invalidate();
						break;
					default:
						return;
				}
			}
		}

		private void MainWindowMouseUp(object sender, MouseEventArgs e)
		{	if(e.Button==MouseButtons.Left)
				this.Cursor = Cursors.Default;
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{

		}

		///удержание курсора в пределах окна
		private void MainWindowResize(object sender, EventArgs e)
		{	Cursor.Clip = Rectangle.Empty;			//временное отключение удержания, т.к. при изменении размера срабатывает и MainWindowMove
			BackgroundWin.Bounds = this.Bounds;
		}
		private void MainWindowResizeEnd(object sender, EventArgs e) => Cursor.Clip = this.Bounds;
		private void MainWindowMove(object sender, EventArgs e)
		{	if(SettingsMode)
				Cursor.Clip = this.Bounds;
			if(BackgroundWin != null)
				BackgroundWin.Bounds = this.Bounds;
		}
		//\удержание курсора в пределах окна
		//\обработчики режима настроек
	}
}
