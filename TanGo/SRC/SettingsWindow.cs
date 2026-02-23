using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static TanGo.ManagerWindow;

namespace TanGo
{	public partial class SettingsWindow : Form
	{	public SettingsWindow()
		{	InitializeComponent();

			//проверка на содержание неизвестных типов в классе Settings
			FieldInfo [][] AllFields = new FieldInfo[4][];
			AllFields[0] = typeof(Settings		).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			AllFields[1] = typeof(Bounds		).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			AllFields[2] = typeof(PointFloat	).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			AllFields[3] = typeof(FontSettings	).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			foreach(FieldInfo [] Fields in  AllFields)
			{	foreach (FieldInfo Field in Fields)
				{	if(!(	Field.FieldType == typeof(int)		||
							Field.FieldType == typeof(float)	||
							Field.FieldType == typeof(bool)		||
							Field.FieldType == typeof(string)	||
							Field.FieldType == typeof(Font)		||
							Field.FieldType == typeof(Rectangle)||
							Field.FieldType == typeof(PointF)))
					{	throw new InvalidOperationException($"Тип {Field.FieldType.Name} поля {Field.Name} класса {Field.DeclaringType.Name} в методах SaveSettings и RestoreSettings не обрабатывается");
						//Если в классы Settings, FontSettings, Bounds или PointFloat добавил новый тип - добавь и обработку в методах TextToStructSettings и StructSettingsToText!
					}
				}
			}
			SettingsCurrent = SettingsMain;
			SettingsToWindow(SettingsMain);
			//чтобы небыло ворнингов
			Bounds			Empty1 = new Bounds(); 
			PointFloat		Empty2 = new PointFloat();
			FontSettings	Empty3 = new FontSettings();
			ПользовательскийТипСостоящийИзПростыхТипов Empty4 = new ПользовательскийТипСостоящийИзПростыхТипов();
		}

		public event Action<string [], bool []>	EventLoadSettingsFromFile;
		public event Func<List<Dictionary>>		EventButtonOKClick;
		public event EventHandler				EventSettingsMode;
		public event EventHandler				EventButtonOKCancelClick;


		class ПользовательскийТипСостоящийИзПростыхТипов
		{	public int		Int		= 0;
			public bool		Bool	= false;
			public float	Float	= 0;
		}

		class FontSettings
		{	public string	Name			= "";
			public float	Size			= 0;
			public int		Style			= 0;
			public int		Unit			= 0;
			public int		GdiCharSet		= 0;
			public bool		GdiVerticalFont	= false;
		}
		new class Bounds
		{	public int X = 0;
			public int Y = 0;
			public int W = 0;
			public int H = 0;
		}
		class PointFloat
		{	public float X = 0;
			public float Y = 0;
		}

		public class Settings
		{	public bool			Work			= false;
			public int			WindowColor		= unchecked((int)0xFFFF8000);
			public float		WindowRadius	= 0.5f;
			public int			WindowVisibility= 100;
			public Rectangle	WindowBounds	= new Rectangle(50, 40, 640, 480);
			public int			SeparatorSize	= 3;
			public int			SeparatorColor	= unchecked((int)0xFF0000FF);
			public int			IntervalM		= 1;
			public int			IntervalS		= 2;
			public int			AutoHideM		= 3;
			public int			AutoHideS		= 4;
			public bool			ClickDouble		= true;
			public bool			GuessLeft		= false;
			public Font			WordFont		= new Font("Times New Roman", 50, FontStyle.Regular, GraphicsUnit.Point, 204, false);
			public int			WordFontColor	= unchecked((int)0xFF00FF00);
			public int			WordLineColor	= unchecked((int)0xFFFF0000);
			public int			WordLineSize	= 5;
			public PointF		WordLocation	= new PointF(0.2f, 0.2f);
			public PointF		WordBind		= new PointF(0.5f, 0.5f);
			public Font			ValueFont		= new Font("Times New Roman", 50, FontStyle.Regular, GraphicsUnit.Point, 204, false);
			public int			ValueFontColor	= unchecked((int)0xFFFF0000);
			public int			ValueLineColor	= unchecked((int)0xFF00FF00);
			public int			ValueLineSize	= 5;
			public PointF		ValueLocation	= new PointF(0.8f, 0.8f);
			public PointF		ValueBind		= new PointF(0.5f, 0.5f);

		}

		Settings SettingsMain = new Settings();
		Settings SettingsTemp = new Settings();
		public Settings SettingsCurrent;
		int CloseMethod = 0;
		
		//хранение шрифтов
		Font WordFont;
		Font ValueFont;
			
		//восстановление настроек в окне настроек
		void SettingsToWindow(Settings Settings)
		{	ButtonWindowColor.BackColor		= Color.FromArgb(Settings.WindowColor);
			NumericWindowRadius.Value		= (decimal)Settings.WindowRadius;
			TrackBarWindowVisibility.Value	= Settings.WindowVisibility;
			NumericWindowSizeX.Value		= Settings.WindowBounds.Width;
			NumericWindowSizeY.Value		= Settings.WindowBounds.Height;
			NumericWindowLocationX.Value	= Settings.WindowBounds.X;
			NumericWindowLocationY.Value	= Settings.WindowBounds.Y;
			NumericSeparatorSize.Value		= Settings.SeparatorSize;
			ButtonSeparatorColor.BackColor	= Color.FromArgb(Settings.SeparatorColor);
			NumericIntervalM.Value			= Settings.IntervalM;
			NumericIntervalS.Value			= Settings.IntervalS;
			NumericAutoHideM.Value			= Settings.AutoHideM;
			NumericAutoHideS.Value			= Settings.AutoHideS;
			RadioButtonClick1.Checked		=!Settings.ClickDouble;
			RadioButtonClick2.Checked		= Settings.ClickDouble;
			RadioButtonLBMGuess.Checked		= Settings.GuessLeft;
			RadioButtonLBMNotGuess.Checked	=!Settings.GuessLeft;
			RadioButtonRBMGuess.Checked		=!Settings.GuessLeft;
			RadioButtonRBMNotGuess.Checked	= Settings.GuessLeft;
			WordFont						= Settings.WordFont;
			ButtonWordFontColor.BackColor	= Color.FromArgb(Settings.WordFontColor);
			ButtonWordLineColor.BackColor	= Color.FromArgb(Settings.WordLineColor);
			NumericWordLineSize.Value		= Settings.WordLineSize;
			NumericWordLocationX.Value		= (decimal)Settings.WordLocation.X;
			NumericWordLocationY.Value		= (decimal)Settings.WordLocation.Y;
			NumericWordBindX.Value			= (decimal)Settings.WordBind.X;
			NumericWordBindY.Value			= (decimal)Settings.WordBind.Y;
			ValueFont						= Settings.ValueFont;
			ButtonValueFontColor.BackColor	= Color.FromArgb(Settings.ValueFontColor);
			ButtonValueLineColor.BackColor	= Color.FromArgb(Settings.ValueLineColor);
			NumericValueLineSize.Value		= Settings.ValueLineSize;
			NumericValueLocationX.Value		= (decimal)Settings.ValueLocation.X;
			NumericValueLocationY.Value		= (decimal)Settings.ValueLocation.Y;
			NumericValueBindX.Value			= (decimal)Settings.ValueBind.X;
			NumericValueBindY.Value			= (decimal)Settings.ValueBind.Y;
		}

		void SettingsFromWindow(Settings Settings)
		{	Settings.WindowColor		=		ButtonWindowColor.BackColor.ToArgb();
			Settings.WindowRadius		=(float)NumericWindowRadius.Value;	
			Settings.WindowVisibility	=		TrackBarWindowVisibility.Value;
			Settings.WindowBounds.Width =  (int)NumericWindowSizeX.Value;
			Settings.WindowBounds.Height=  (int)NumericWindowSizeY.Value;
			Settings.WindowBounds.X		=  (int)NumericWindowLocationX.Value;
			Settings.WindowBounds.Y		=  (int)NumericWindowLocationY.Value;
			Settings.SeparatorSize		=  (int)NumericSeparatorSize.Value;
			Settings.SeparatorColor		= 		ButtonSeparatorColor.BackColor.ToArgb();
			Settings.IntervalM			=  (int)NumericIntervalM.Value;
			Settings.IntervalS			=  (int)NumericIntervalS.Value;
			Settings.AutoHideM			=  (int)NumericAutoHideM.Value;
			Settings.AutoHideS			=  (int)NumericAutoHideS.Value;
			Settings.ClickDouble		= 		RadioButtonClick2.Checked;
			Settings.GuessLeft			= 		RadioButtonLBMGuess.Checked;
			Settings.WordFont			= 		WordFont;
			Settings.WordFontColor		= 		ButtonWordFontColor.BackColor.ToArgb();
			Settings.WordLineColor		= 		ButtonWordLineColor.BackColor.ToArgb();
			Settings.WordLineSize		=  (int)NumericWordLineSize.Value;
			Settings.WordLocation.X		=(float)NumericWordLocationX.Value;
			Settings.WordLocation.Y		=(float)NumericWordLocationY.Value;
			Settings.WordBind.X			=(float)NumericWordBindX.Value;
			Settings.WordBind.Y			=(float)NumericWordBindY.Value;
			Settings.ValueFont			= 		ValueFont;
			Settings.ValueFontColor		= 		ButtonValueFontColor.BackColor.ToArgb();
			Settings.ValueLineColor		= 		ButtonValueLineColor.BackColor.ToArgb();
			Settings.ValueLineSize		=  (int)NumericValueLineSize.Value;
			Settings.ValueLocation.X	=(float)NumericValueLocationX.Value;
			Settings.ValueLocation.Y	=(float)NumericValueLocationY.Value;
			Settings.ValueBind.X		=(float)NumericValueBindX.Value;
			Settings.ValueBind.Y		=(float)NumericValueBindY.Value;
		}

		//восстановление структуры настроек
		int TextToStructSettings(string [] Lines, int Offset, Type Type, object Object)
		 {	FieldInfo[] Fields = Type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			//поиск каждого имени из файла среди полей класса
			for(; Offset<Lines.Length; Offset++)
			{	//если достигнут конец сложного типа
				if(Lines[Offset] == "}") return Offset;
				//перебор полей класса
				foreach (FieldInfo Field in Fields)
				{	if(Lines[Offset].Trim() == Field.Name)
					{	//если поле статическое то нул
						Object = Field.IsStatic? null : Object;
						Offset++;
						try
						{	if(Field.FieldType == typeof(int))		Field.SetValue(Object, Convert.ToInt32	(Lines[Offset].Trim()));
							if(Field.FieldType == typeof(uint))		Field.SetValue(Object, Convert.ToUInt32	(Lines[Offset].Trim()));
							if(Field.FieldType == typeof(float))	Field.SetValue(Object, Convert.ToSingle (Lines[Offset].Trim()));
							if(Field.FieldType == typeof(bool))		Field.SetValue(Object, Convert.ToBoolean(Lines[Offset].Trim()));
							if(Field.FieldType == typeof(string))	Field.SetValue(Object,					 Lines[Offset].Trim());
						}
						catch
						{	continue;	}
						if(Field.FieldType == typeof(ПользовательскийТипСостоящийИзПростыхТипов))
						{	object ComplexObject = Activator.CreateInstance(Field.FieldType);
							Field.SetValue(Object, ComplexObject);
                    		Offset = TextToStructSettings(Lines, Offset+1, typeof(ПользовательскийТипСостоящийИзПростыхТипов), ComplexObject);
						}
						if(Field.FieldType == typeof(Font))
						{	FontSettings FontSettings = new FontSettings();
							Offset = TextToStructSettings(Lines, Offset+1, typeof(FontSettings), FontSettings);
							try
							{	Font Font = Field.GetValue(Object) as Font;
								Font?.Dispose();
								Font = new Font(	FontSettings.Name,
													FontSettings.Size,
										 (FontStyle)FontSettings.Style,
									  (GraphicsUnit)FontSettings.Unit,
											  (byte)FontSettings.GdiCharSet,
													FontSettings.GdiVerticalFont);
								Field.SetValue(Object, Font);
							}
							catch{}
						}
						if(Field.FieldType == typeof(Rectangle))
						{	Bounds Bounds = new Bounds();
							Offset = TextToStructSettings(Lines, Offset+1, typeof(Bounds), Bounds);
							Rectangle Rectangle = new Rectangle(Bounds.X, Bounds.Y, Bounds.W, Bounds.H);
							Field.SetValue(Object, Rectangle);
						}
						if(Field.FieldType == typeof(PointF))
						{	PointFloat PointFloat = new PointFloat();
							Offset = TextToStructSettings(Lines, Offset+1, typeof(PointFloat), PointFloat);
							PointF PointF = new PointF(PointFloat.X, PointFloat.Y);
							Field.SetValue(Object, PointF);
						}
						break;
					}
				}
			}
			return 0;
		}

		//загрузка настроек
		void LoadSettings(string [] Lines)
		{	TextToStructSettings(Lines, 0, typeof(Settings), SettingsMain);
			SettingsToWindow(SettingsMain);
			//восстановление списка словарей
			int Offset;
			string [] Dictionaries;
			bool   [] Checked;
			List<string> ListDictionaries	= new List<string>();
			List<bool>	 ListChecked		= new List<bool  >();
			for(int i=0; i<Lines.Length; i++)
			{	if(Lines[i] != "[Dictionaries]")
					continue;
				Offset = i + 2;
				for(; Offset<Lines.Length; Offset++)
				{	if(Lines[Offset] == "}")
						break;
					string File = Lines[Offset].Trim();
					Offset++;
					bool Check;
					try		{ Check = Convert.ToBoolean(Lines[Offset].Trim()); }
					catch	{ continue; }
					ListDictionaries.Add(File);
					ListChecked.Add(Check);
				}
				break;
			}
			Dictionaries = ListDictionaries.ToArray();
			Checked = ListChecked.ToArray();
			//передача менеджеру словарей путей к словарям
			EventLoadSettingsFromFile?.Invoke(Dictionaries, Checked);
		}
		
		//считывание структуры настроек из файла
		public void ReadSettingsFile()
		{	try
			{	string [] Lines = File.ReadAllLines("Settings.ini");
				LoadSettings(Lines);
			}
			catch{}
		}

		//формирование структуры настроек
		public string StructSettingsToText(Type Type, object Object, int Level)
		{	string String = null;
			FieldInfo [] Fields = Type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo Field in Fields)
			{	//если поле статическое то нул
				Object = Field.IsStatic? null : Object;
				if(Field.FieldType.IsPrimitive | Field.FieldType==typeof(String))
					String += $"{(Level>0? "\t" : "")}{Field.Name}\r\n{(Level>0? "\t" : "")}{Field.GetValue(Object)}\r\n";
				if(Field.FieldType == typeof(ПользовательскийТипСостоящийИзПростыхТипов))
				{	String += $"{Field.Name}\r\n" + "{\r\n";
					object ComplexObject = Field.GetValue(Object);
					String += StructSettingsToText(typeof(ПользовательскийТипСостоящийИзПростыхТипов), ComplexObject, ++Level);
					Level--;
					String += "}\r\n";
				}
				if(Field.FieldType == typeof(Font))
				{	++Level;
					String += $"{Field.Name}\r\n" + "{\r\n";
					Font Font = (Font)Field.GetValue(Object);
					String += $"{(Level>0? "\t" : "")}Name\r\n{				(Level>0? "\t" : "")}{	   Font.Name			}\r\n";
					String += $"{(Level>0? "\t" : "")}Size\r\n{				(Level>0? "\t" : "")}{	   Font.Size			}\r\n";
					String += $"{(Level>0? "\t" : "")}Style\r\n{			(Level>0? "\t" : "")}{(int)Font.Style			}\r\n";
					String += $"{(Level>0? "\t" : "")}Unit\r\n{				(Level>0? "\t" : "")}{(int)Font.Unit			}\r\n";
					String += $"{(Level>0? "\t" : "")}GdiCharSet\r\n{		(Level>0? "\t" : "")}{	   Font.GdiCharSet		}\r\n";
					String += $"{(Level>0? "\t" : "")}GdiVerticalFont\r\n{	(Level>0? "\t" : "")}{	   Font.GdiVerticalFont	}\r\n";
					Level--;								
					String += "}\r\n";
				}
				if(Field.FieldType == typeof(Rectangle))
				{	++Level;
					String += $"{Field.Name}\r\n" + "{\r\n";
					Rectangle Rectangle = (Rectangle)Field.GetValue(Object);
					String += $"{(Level>0? "\t" : "")}X\r\n{(Level>0? "\t" : "")}{Rectangle.X		}\r\n";
					String += $"{(Level>0? "\t" : "")}Y\r\n{(Level>0? "\t" : "")}{Rectangle.Y		}\r\n";
					String += $"{(Level>0? "\t" : "")}W\r\n{(Level>0? "\t" : "")}{Rectangle.Width	}\r\n";
					String += $"{(Level>0? "\t" : "")}H\r\n{(Level>0? "\t" : "")}{Rectangle.Height	}\r\n";
					Level--;								
					String += "}\r\n";
				}
				if(Field.FieldType == typeof(PointF))
				{	++Level;
					String += $"{Field.Name}\r\n" + "{\r\n";
					PointF PointF = (PointF)Field.GetValue(Object);
					String += $"{(Level>0? "\t" : "")}X\r\n{(Level>0? "\t" : "")}{PointF.X}\r\n";
					String += $"{(Level>0? "\t" : "")}Y\r\n{(Level>0? "\t" : "")}{PointF.Y}\r\n";
					Level--;								
					String += "}\r\n";
				}
			}
			return String;
		}

		//запись структуры настроек и списка словарей в файл
		public void SaveSettings(List<Dictionary> Dictionaries, bool Enable, bool _NotUse_)
		{	if(!Enable)
				return;
			string String = StructSettingsToText(typeof(Settings), SettingsMain, 0);
			String += "\r\n[Dictionaries]\r\n{\r\n";
			foreach(Dictionary Dictionary in Dictionaries)
				String += $"\t{Dictionary.FileName}\r\n\t{Dictionary.Checked}\r\n";
			String += "}";
			DialogResult Result = DialogResult.Retry;
			for(; Result==DialogResult.Retry; )
			{	try
				{	File.WriteAllText("Settings.ini", String);
					Result = DialogResult.OK;
				}
				catch(Exception ex)
				{ 	Result = MessageBox.Show($"Невозможно получить доступ к файлу настроек\r\nИсключение: " + ex.GetType().Name + "\r\n" + ex.Message,
												"Ошибка", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
				}
			}
		}

		public void EscFromSettingsModeHandler(bool Apply, Rectangle Bounds, PointF WordLocation, PointF ValueLocation, PointF WordBind, PointF ValueBind)
		{	if(Apply)
			{	NumericWindowSizeX.Value		= Bounds.Size.Width;
				NumericWindowSizeY.Value		= Bounds.Size.Height;
				NumericWindowLocationX.Value	= Bounds.X;
				NumericWindowLocationY.Value	= Bounds.Y;
				NumericWordLocationX.Value		= (decimal)WordLocation.X;
				NumericWordLocationY.Value		= (decimal)WordLocation.Y;
				NumericValueLocationX.Value		= (decimal)ValueLocation.X;
				NumericValueLocationY.Value		= (decimal)ValueLocation.Y;
				NumericWordBindX.Value			= (decimal)WordBind.X;
				NumericWordBindY.Value			= (decimal)WordBind.Y;
				NumericValueBindX.Value			= (decimal)ValueBind.X;
				NumericValueBindY.Value			= (decimal)ValueBind.Y;
			}
		}

		private void ButtonOkClick(object sender, EventArgs e)
		{	CloseMethod = 1;
		}
		private void ButtonCancelClick(object sender, EventArgs e)
		{	CloseMethod = 0;
		}
		private void SettingsWindowFormClosed(object sender, FormClosedEventArgs e)
		{	switch(CloseMethod)
			{	case 0:	//"отмена"
				{	SettingsToWindow(SettingsMain);
					SettingsCurrent = SettingsMain;
					EventButtonOKCancelClick?.Invoke(null, EventArgs.Empty);
					break;
				}
				case 1:	//"ОК"
				{	SettingsFromWindow(SettingsMain);
					SaveSettings(EventButtonOKClick?.Invoke(), true, false);
					SettingsCurrent = SettingsMain;
					EventButtonOKCancelClick?.Invoke(null, EventArgs.Empty);
					break;
				}
				case 2:	//"определить"
				{	SettingsFromWindow(SettingsTemp);
					SettingsCurrent = SettingsTemp;
					EventSettingsMode?.Invoke(null, EventArgs.Empty);
					break;
				}
			}
			CloseMethod = 0;
		}
		private void ButtonWindowColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonWindowColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonWindowColor.BackColor = ColorDialog.Color;
		}
		private void ButtonSeparatorColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonSeparatorColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonSeparatorColor.BackColor = ColorDialog.Color;
		}
		private void RadioButtonLBMGuessClick(object sender, EventArgs e)
		{	RadioButtonLBMGuess	  .Checked	= true;	RadioButtonRBMGuess	  .Checked = false;
			RadioButtonLBMNotGuess.Checked = false;	RadioButtonRBMNotGuess.Checked = true;
		}
		private void RadioButtonLBMNotGuessClick(object sender, EventArgs e)
		{	RadioButtonLBMGuess	  .Checked = false;	RadioButtonRBMGuess	  .Checked = true;
			RadioButtonLBMNotGuess.Checked = true;	RadioButtonRBMNotGuess.Checked = false;
		}
		private void RadioButtonRBMGuessClick(object sender, EventArgs e)
		{	RadioButtonLBMGuess	  .Checked = false;	RadioButtonRBMGuess	  .Checked = true;
			RadioButtonLBMNotGuess.Checked = true;	RadioButtonRBMNotGuess.Checked = false;
		}
		private void RadioButtonRBMNotGuessClick(object sender, EventArgs e)
		{	RadioButtonLBMGuess	  .Checked = true;	RadioButtonRBMGuess	  .Checked = false;
			RadioButtonLBMNotGuess.Checked = false;	RadioButtonRBMNotGuess.Checked = true;
		}
		private void ButtonWordFontClick(object sender, EventArgs e)
		{	FontDialog.Font = WordFont;
			if(FontDialog.ShowDialog() == DialogResult.OK)
			{	WordFont?.Dispose();
				WordFont = FontDialog.Font;
			}
		}
		private void ButtonWordFontColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonWordFontColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonWordFontColor.BackColor = ColorDialog.Color;
		}
		private void ButtonWordLineColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonWordLineColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonWordLineColor.BackColor = ColorDialog.Color;
		}
		private void ButtonValueFontClick(object sender, EventArgs e)
		{	FontDialog.Font = ValueFont;
			if(FontDialog.ShowDialog() == DialogResult.OK)
			{	ValueFont?.Dispose();
				ValueFont = FontDialog.Font;
			}
		}
		private void ButtonValueFontColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonValueFontColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonValueFontColor.BackColor = ColorDialog.Color;
		}
		private void ButtonValueLineColorClick(object sender, EventArgs e)
		{	ColorDialog.Color = ButtonValueLineColor.BackColor;
			if(ColorDialog.ShowDialog() == DialogResult.OK) ButtonValueLineColor.BackColor = ColorDialog.Color;
		}
		private void ButtonSpecifyClick(object sender, EventArgs e)
		{	CloseMethod = 2;
		}
	}
}
