using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TanGo
{	public partial class ManagerWindow : Form
	{	public ManagerWindow()
		{	InitializeComponent();
			
			//настройка окна открытия файла
			OpenFileDialog.Title = "Выбор словаря (*.txt)";
			OpenFileDialog.Filter = "Текстовые файлы UTF-8(без bom)(*.txt)|*.txt";
			OpenFileDialog.FileName = "*.txt";
			OpenFileDialog.FilterIndex = 1;
		}

		public event Action<List<Dictionary>, bool, bool>	EventButtonOKClick;
		public event Action<Dictionary>						EventButtonRefreshClick;

		public List<Dictionary> Dictionaries = new List<Dictionary>();

		public class Dictionary
		{	public bool Checked;
			public string FileName;
			public string [] Lines = null;
			public List<MainWindow.Record> Records = null;
			public override string ToString() => Path.GetFileName(FileName);
		}

		public List<Dictionary> EventButtonOKHandler()
		{	return Dictionaries;
		}

		void RefreshDictionary(string FileName)
		{	string [] Lines;
			DialogResult Result = DialogResult.Retry;
			for(; Result==DialogResult.Retry; )
			{	try
				{	Lines = File.ReadAllLines(FileName, Encoding.UTF8);
					for(int i=0; i<Lines.Length; i++)
					{	if(Lines[i].Length != 0)
						{	if(Lines[i][Lines[i].Length-1] == MainWindow.Marker && Lines[i][0] != MainWindow.Tilda)
								Lines[i] = Lines[i].Substring(0, Lines[i].Length-1);
						}
					}
					File.WriteAllLines(FileName, Lines, Encoding.UTF8);
					Result = DialogResult.OK;
				}
				catch(Exception ex)
				{	Result = MessageBox.Show("Невозможно получить доступ к файлу\r\nИсключение: " + ex.GetType().Name + "\r\n" + ex.Message,
									"Ошибка", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
				}
			}
		}

		bool AddDictionaries(string [] Dictionaries, bool [] Checking)
		{	bool NotAllFound = false;
			for(int i=0; i<Dictionaries.Length; i++)
			{	if(!File.Exists(Dictionaries[i]))
				{	NotAllFound = true;
					continue;
				}
				for(int j=0; j<CheckedListBox.Items.Count; j++)
				{	if(((Dictionary)CheckedListBox.Items[j]).FileName == Dictionaries[i])
					{	CheckedListBox.SelectedIndex = j;
						goto LABEL_CONTINUE;
					}
				}
				bool Check = Checking==null? true : Checking[i];
				CheckedListBox.SelectedIndex = CheckedListBox.Items.Add(new Dictionary { FileName = Dictionaries[i] }, Check);
LABEL_CONTINUE:;
			}
			return NotAllFound;
		}

		public void EventLoadSettingsFromFileHandler(string [] Dictionaries, bool [] Checked)
		{	object Obj = AddDictionaries(Dictionaries, Checked)? this : null;
			ButtonOKClick(Obj, EventArgs.Empty);
		}

		private void ManagerWindowLoad(object sender, EventArgs e)
		{	if(CheckedListBox.SelectedIndex > -1)
				ButtonRefresh.Enabled = true;
		}

		private void ButtonAddClick(object sender, EventArgs e)
		{	DialogResult Result;
			try
			{	Result = OpenFileDialog.ShowDialog();
			}
			catch(Exception ex)
			{	MessageBox.Show("Ошибка открытия файла\r\nИсключение: " + ex.GetType().Name + "\r\n" + ex.Message,
								"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if(Result == DialogResult.OK)
				AddDictionaries(OpenFileDialog.FileNames, null);
		}

		private void ButtonDelClick(object sender, EventArgs e)
		{	CheckedListBox.Items.RemoveAt(CheckedListBox.SelectedIndex);
			TextBoxPath.Text = "";
			if(CheckedListBox.SelectedIndex == -1)
			{	ButtonDel	 .Enabled = false;
				ButtonRefresh.Enabled = false;
			}
		}

		private void ButtonRefreshClick(object sender, EventArgs e)
		{	DialogResult Result = MessageBox.Show("Точно сбросить словарь?\r\nДействие необратимо!", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
			if(Result == DialogResult.Cancel)
				return;
			RefreshDictionary(((Dictionary)CheckedListBox.Items[CheckedListBox.SelectedIndex]).FileName);
			if(CheckedListBox.SelectedIndex < Dictionaries.Count)
				if(((Dictionary)CheckedListBox.Items[CheckedListBox.SelectedIndex]).FileName == Dictionaries[CheckedListBox.SelectedIndex].FileName)
					EventButtonRefreshClick?.Invoke(Dictionaries[CheckedListBox.SelectedIndex]);
		}
		
		private void ButtonOKClick(object sender, EventArgs e)
		{	Dictionaries.Clear();
			for(int i=0; i<CheckedListBox.Items.Count; i++)
			{	((Dictionary)CheckedListBox.Items[i]).Checked = CheckedListBox.GetItemCheckState(i) == CheckState.Checked;
				Dictionaries.Add((Dictionary)CheckedListBox.Items[i]);
			}
			bool NotAllFound = sender==null? false : true;
			EventButtonOKClick?.Invoke(Dictionaries, this.Visible, NotAllFound);
			if(this.Visible)
				this.Close();
		}

		private void ButtonCancelClick(object sender, EventArgs e)
		{	CheckedListBox.Items.Clear();
			foreach(Dictionary  Dictionary in Dictionaries)
				CheckedListBox.Items.Add(Dictionary, Dictionary.Checked);
			this.Close();
		}

		private void CheckedListBoxSelectedIndexChanged(object sender, EventArgs e)
		{	if (CheckedListBox.SelectedIndex != -1)
			{	TextBoxPath.Text = ((Dictionary)CheckedListBox.Items[CheckedListBox.SelectedIndex]).FileName;
				TextBoxPath.TextAlign = HorizontalAlignment.Left;
				TextBoxPath.SelectionStart = TextBoxPath.Text.Length;
				ButtonDel	 .Enabled = true;
				ButtonRefresh.Enabled = true;
			}
		}
	}

}
