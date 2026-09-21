using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using System.Xml.Linq;
using CustomControlLibrary;
using Mouse_Drive_Beta;
using Mouse_Drive_Beta.FileManager;

namespace FileManager;

public class LanguageFile
{
	public string[] LanguageType;

	public int LanguageIndex;

	public int LanguageCount;

	public static string[] FilePaths;

	public string[] FromSettingString = new string[3];

	public List<LabelStruct> LabelStructs = new List<LabelStruct>();

	public List<LabelStruct> ToolStripMenuItemLabelStructs = new List<LabelStruct>();

	public static List<ComboBoxStruct> ComboBoxStructs = new List<ComboBoxStruct>();

	public List<int> ComboxIndexList = new List<int>();

	public List<List<KeyFunctionStruct>> KeyFunctionSubStructs = new List<List<KeyFunctionStruct>>();

	public List<KeyFunctionStruct> KeyFunctionStructs = new List<KeyFunctionStruct>();

	public List<KeyFunctionStruct> AllKeyFunctionStructs = new List<KeyFunctionStruct>();

	public List<KeyFunctionStruct> DebounceTimeList = new List<KeyFunctionStruct>();

	private float FontSize = 10.5f;

	public int MultiFuncionIndex;

	public int KeyFunctionMaxType;

	public static List<string> Dialogs = new List<string>();

	public int GetLanguageFileCount()
	{
		LanguageCount = 0;
		string path = Application.StartupPath + "\\Language";
		if (!Directory.Exists(path))
		{
			return LanguageCount;
		}
		FilePaths = Directory.GetFiles(path, "*.xml");
		if (FilePaths.Count() == 0)
		{
			return LanguageCount;
		}
		LanguageType = new string[FilePaths.Count()];
		for (int i = 0; i < FilePaths.Count(); i++)
		{
			XElement root = XDocument.Load(FilePaths[i]).Root;
			LanguageType[LanguageCount] = root.Name.ToString();
			LanguageCount++;
		}
		int index = 0;
		if (RegeditManager.GetLanguageIndex(out index))
		{
			LanguageIndex = index;
		}
		else
		{
			if (LanguageCount > 1)
			{
				LanguageIndex = 1;
			}
			RegeditManager.SaveLanguageIndex(LanguageIndex);
		}
		return LanguageCount;
	}

	public static float GetFontSize(int index)
	{
		float result = 10.5f;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FilePaths[index]);
		XmlNode xmlNode = xmlDocument.GetElementsByTagName("FontSize")[0];
		if (xmlNode != null)
		{
			result = float.Parse(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
		return result;
	}

	public static void ForeachSetColor(Control father, DriveConfig.DriveParam driveParam)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			if (val is CustomTrackBar)
			{
				((CustomTrackBar)(object)val).SliderColor = driveParam.SliderClr;
			}
			if (val is TextBox)
			{
				TextBox val2 = (TextBox)val;
				if (((Control)val2).Name.Contains("Dlg"))
				{
					((Control)val2).BackColor = driveParam.DlgTxtBgClr;
					((Control)val2).ForeColor = driveParam.DlgTxtForeClr;
				}
				else
				{
					((Control)val2).BackColor = driveParam.TxtBgClr;
					((Control)val2).ForeColor = driveParam.TxtForeClr;
				}
			}
			if (val is ListView)
			{
				((Control)(ListView)val).BackColor = driveParam.LstViewBgClr;
			}
			if (val is CustomListView)
			{
				((Control)(CustomListView)(object)val).BackColor = driveParam.LstViewBgClr;
			}
			if (val is Label)
			{
				((Control)(Label)val).ForeColor = driveParam.LblForeClr;
			}
			if (val is CustomButton)
			{
				((Control)(CustomButton)(object)val).ForeColor = driveParam.BtnForeClr;
			}
			if (val.HasChildren)
			{
				if (val is CustomComboBox)
				{
					CustomComboBox obj = (CustomComboBox)(object)val;
					Color unselectItemColor = (((Control)obj).BackColor = driveParam.CbbBgClr);
					obj.UnselectItemColor = unselectItemColor;
					obj.SelectItemColor = driveParam.CbbItemClr;
					((Control)obj).ForeColor = driveParam.CbbForeClr;
					obj.ArrowImageNoraml = (Image)(object)ResourcesFile.GetComboxBoxBitmap();
				}
				else
				{
					ForeachSetColor(val, driveParam);
				}
			}
		}
	}

	public static void FormSetColor(Control father)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Form val = (Form)father;
		val.Icon = new Icon(Application.StartupPath + "\\res\\logo.ico");
		((Control)val).Text = DriveConfig.GetDriveName();
		DriveConfig.DriveParam driveParam = DriveConfig.GetDriveParam();
		ForeachSetColor(father, driveParam);
	}

	public void ForeachSetControl(Control father)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			if (val is Label)
			{
				Label val2 = (Label)val;
				for (int i = 0; i < LabelStructs.Count; i++)
				{
					if (((Control)val2).Name == LabelStructs[i].Name)
					{
						((Control)val2).Text = LabelStructs[i].Text;
						break;
					}
				}
			}
			else if (val is CustomButton)
			{
				CustomButton customButton = (CustomButton)(object)val;
				for (int j = 0; j < LabelStructs.Count; j++)
				{
					if (((Control)customButton).Name == LabelStructs[j].Name)
					{
						((Control)customButton).Text = LabelStructs[j].Text;
						break;
					}
				}
			}
			else if (val is CustomRecordMacro)
			{
				CustomRecordMacro customRecordMacro = (CustomRecordMacro)(object)val;
				for (int k = 0; k < LabelStructs.Count; k++)
				{
					if (LabelStructs[k].Name == ((Control)customRecordMacro).Name + "_start")
					{
						customRecordMacro.StartText = LabelStructs[k].Text;
					}
					else if (LabelStructs[k].Name == ((Control)customRecordMacro).Name + "_stop")
					{
						customRecordMacro.StopText = LabelStructs[k].Text;
					}
				}
			}
			else
			{
				if (!val.HasChildren)
				{
					continue;
				}
				if (val is CustomComboBox)
				{
					CustomComboBox customComboBox = (CustomComboBox)(object)val;
					for (int l = 0; l < ComboBoxStructs.Count; l++)
					{
						if (((Control)customComboBox).Name == ComboBoxStructs[l].Name)
						{
							customComboBox.Item.Clear();
							for (int m = 0; m < ComboBoxStructs[l].Items.Count; m++)
							{
								customComboBox.Item.Add((object)ComboBoxStructs[l].Items[m]);
							}
							customComboBox.SelectIndex = ComboxIndexList[l];
							break;
						}
					}
				}
				else
				{
					ForeachSetControl(val);
				}
			}
		}
	}

	public void ForeachGetControl(Control father)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (Control item6 in (ArrangedElementCollection)father.Controls)
		{
			Control val = item6;
			if (val is Label)
			{
				Label val2 = (Label)val;
				LabelStruct item = new LabelStruct
				{
					Name = ((Control)val2).Name,
					Text = ((Control)val2).Text
				};
				LabelStructs.Add(item);
			}
			else if (val is CustomButton)
			{
				CustomButton customButton = (CustomButton)(object)val;
				if (((Control)customButton).Text != "")
				{
					LabelStruct item2 = new LabelStruct
					{
						Name = ((Control)customButton).Name,
						Text = ((Control)customButton).Text
					};
					LabelStructs.Add(item2);
				}
			}
			else if (val is CustomRadioButton)
			{
				CustomRadioButton customRadioButton = (CustomRadioButton)(object)val;
				if (((Control)customRadioButton).Text != "")
				{
					LabelStruct item3 = new LabelStruct
					{
						Name = ((Control)customRadioButton).Name,
						Text = customRadioButton.TextString
					};
					LabelStructs.Add(item3);
				}
			}
			else if (val is CustomRecordMacro)
			{
				CustomRecordMacro customRecordMacro = (CustomRecordMacro)(object)val;
				LabelStruct item4 = new LabelStruct
				{
					Name = ((Control)customRecordMacro).Name + "_start",
					Text = customRecordMacro.StartText
				};
				LabelStructs.Add(item4);
				item4 = new LabelStruct
				{
					Name = ((Control)customRecordMacro).Name + "_stop",
					Text = customRecordMacro.StopText
				};
				LabelStructs.Add(item4);
			}
			else
			{
				if (!val.HasChildren)
				{
					continue;
				}
				if (val is CustomComboBox)
				{
					CustomComboBox customComboBox = (CustomComboBox)(object)val;
					ComboBoxStruct item5 = default(ComboBoxStruct);
					item5.Name = ((Control)customComboBox).Name;
					item5.Items = new List<string>();
					item5.Items.Clear();
					for (int i = 0; i < customComboBox.Item.Count; i++)
					{
						item5.Items.Add(customComboBox.Item[i].ToString());
					}
					ComboBoxStructs.Add(item5);
					ComboxIndexList.Add(customComboBox.SelectIndex);
				}
				else
				{
					ForeachGetControl(val);
				}
			}
		}
	}

	public string GetHz(int index)
	{
		string result = "Hz";
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FilePaths[index]);
		XmlNode xmlNode = xmlDocument.GetElementsByTagName("Hz")[0];
		if (xmlNode != null)
		{
			result = xmlNode.InnerText;
		}
		return result;
	}

	public void SetSettingLanguage(int index)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FilePaths[index]);
		XmlNode xmlNode = xmlDocument.GetElementsByTagName("FontSize")[0];
		for (int i = 0; i < 3; i++)
		{
			FromSettingEnum fromSettingEnum = (FromSettingEnum)i;
			xmlNode = xmlDocument.GetElementsByTagName(fromSettingEnum.ToString())[0];
			if (xmlNode != null)
			{
				FromSettingString[i] = xmlNode.InnerText;
			}
		}
		XmlNode xmlNode2 = xmlDocument.GetElementsByTagName(LanguageType[index])[0];
		Dialogs.Clear();
		DialogEnum dialogEnum = DialogEnum.DeviceConnecting;
		for (int j = 0; j < 103; j++)
		{
			dialogEnum = (DialogEnum)j;
			string xpath = "Dialog/" + dialogEnum;
			foreach (XmlNode item in xmlNode2.SelectNodes(xpath))
			{
				Dialogs.Add(item.InnerText);
			}
		}
	}

	public void SetLanguage(int index, Control father)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FilePaths[index]);
		XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("FontSize");
		XmlNode xmlNode = elementsByTagName[0];
		if (xmlNode != null)
		{
			string innerText = xmlNode.InnerText;
			FontSize = float.Parse(innerText, CultureInfo.InvariantCulture);
		}
		LabelStructs.Clear();
		ComboBoxStructs.Clear();
		ComboxIndexList.Clear();
		if (father != null)
		{
			ForeachGetControl(father);
		}
		for (int i = 0; i < ComboBoxStructs.Count; i++)
		{
			elementsByTagName = xmlDocument.GetElementsByTagName("ComboBoxName");
			for (int j = 0; j < elementsByTagName.Count; j++)
			{
				if (!(elementsByTagName[j].InnerText == ComboBoxStructs[i].Name))
				{
					continue;
				}
				xmlNode = elementsByTagName[j];
				if (xmlNode == null)
				{
					continue;
				}
				ComboBoxStruct item = default(ComboBoxStruct);
				item.Name = xmlNode.InnerText;
				item.Items = new List<string>();
				item.Items.Clear();
				item.Values = new List<int>();
				item.Values.Clear();
				int num = 0;
				if (xmlNode.NextSibling.Name == "Item")
				{
					do
					{
						xmlNode = xmlNode.NextSibling;
						item.Items.Add(xmlNode.InnerText);
						int item2 = ValueConvert.StringToInt(xmlNode.Attributes["value"].InnerText);
						item.Values.Add(item2);
						num++;
					}
					while (xmlNode.NextSibling != null && xmlNode.NextSibling.Name == "Item");
				}
				ComboBoxStructs.RemoveAt(i);
				ComboBoxStructs.Insert(i, item);
			}
		}
		for (int k = 0; k < LabelStructs.Count; k++)
		{
			elementsByTagName = xmlDocument.GetElementsByTagName(LabelStructs[k].Name);
			xmlNode = elementsByTagName[0];
			if (xmlNode != null && LabelStructs[k].Name != "" && xmlNode.InnerText != "")
			{
				LabelStruct item3 = new LabelStruct
				{
					Name = LabelStructs[k].Name,
					Text = xmlNode.InnerText
				};
				LabelStructs.RemoveAt(k);
				LabelStructs.Insert(k, item3);
			}
		}
		if (father != null)
		{
			ForeachSetControl(father);
		}
		for (int l = 0; l < 3; l++)
		{
			FromSettingEnum fromSettingEnum = (FromSettingEnum)l;
			elementsByTagName = xmlDocument.GetElementsByTagName(fromSettingEnum.ToString());
			xmlNode = elementsByTagName[0];
			if (xmlNode != null)
			{
				FromSettingString[l] = xmlNode.InnerText;
			}
		}
		_ = xmlDocument.DocumentElement;
		elementsByTagName = xmlDocument.GetElementsByTagName(LanguageType[LanguageIndex]);
		XmlNode xmlNode2 = elementsByTagName[0];
		if (MultiFuncionIndex == 0)
		{
			MultiFuncionIndex = 9;
		}
		ToolStripMenuItemLabelStructs.Clear();
		elementsByTagName = xmlNode2.SelectNodes("ToolStripMenuItem");
		XmlNode xmlNode3 = elementsByTagName[0];
		for (int m = 0; m < xmlNode3.ChildNodes.Count; m++)
		{
			XmlNode xmlNode4 = xmlNode3.ChildNodes[m];
			LabelStruct item4 = new LabelStruct
			{
				Name = xmlNode4.Name,
				Text = xmlNode4.InnerText
			};
			ToolStripMenuItemLabelStructs.Add(item4);
		}
		KeyFunctionStructs.Clear();
		foreach (XmlNode item8 in xmlNode2.SelectNodes("KeyFunction/name"))
		{
			KeyFunctionStruct item5 = new KeyFunctionStruct
			{
				name = item8.InnerText
			};
			string innerText2 = item8.Attributes["value"].InnerText;
			string innerText3 = item8.Attributes["type"].InnerText;
			string innerText4 = item8.Attributes["pull"].InnerText;
			int num2 = int.Parse(innerText2, NumberStyles.HexNumber);
			item5.type = (byte)int.Parse(innerText3, NumberStyles.HexNumber);
			item5.param1 = (byte)num2;
			item5.param2 = (byte)(num2 >> 8);
			item5.RightPull = int.Parse(innerText4, NumberStyles.HexNumber) > 0;
			KeyFunctionMaxType = ((KeyFunctionMaxType > item5.type) ? KeyFunctionMaxType : item5.type);
			KeyFunctionStructs.Add(item5);
		}
		KeyFunctionMaxType++;
		KeyFunctionSubStructs.Clear();
		for (int n = 0; n < KeyFunctionMaxType; n++)
		{
			List<KeyFunctionStruct> list = new List<KeyFunctionStruct>();
			foreach (XmlNode item9 in xmlNode2.SelectNodes("SubKeyFunction/name"))
			{
				KeyFunctionStruct item6 = new KeyFunctionStruct
				{
					name = item9.InnerText
				};
				string innerText5 = item9.Attributes["value"].InnerText;
				string innerText6 = item9.Attributes["type"].InnerText;
				int num3 = int.Parse(innerText5, NumberStyles.HexNumber);
				item6.type = (byte)int.Parse(innerText6, NumberStyles.HexNumber);
				item6.param1 = (byte)num3;
				item6.param2 = (byte)(num3 >> 8);
				item6.RightPull = false;
				if (n == item6.type)
				{
					list.Add(item6);
				}
			}
			KeyFunctionSubStructs.Add(list);
		}
		AllKeyFunctionStructs.Clear();
		for (int num4 = 0; num4 < KeyFunctionStructs.Count; num4++)
		{
			AllKeyFunctionStructs.Add(KeyFunctionStructs[num4]);
		}
		for (int num5 = 0; num5 < KeyFunctionSubStructs.Count; num5++)
		{
			for (int num6 = 0; num6 < KeyFunctionSubStructs[num5].Count; num6++)
			{
				AllKeyFunctionStructs.Add(KeyFunctionSubStructs[num5][num6]);
			}
		}
		DebounceTimeList.Clear();
		foreach (XmlNode item10 in xmlNode2.SelectNodes("DebounceTime/name"))
		{
			KeyFunctionStruct item7 = new KeyFunctionStruct
			{
				name = item10.InnerText
			};
			int num7 = int.Parse(item10.Attributes["value"].InnerText, NumberStyles.HexNumber);
			item7.param1 = (byte)num7;
			item7.param2 = (byte)(num7 >> 8);
			item7.RightPull = false;
			DebounceTimeList.Add(item7);
		}
		Dialogs.Clear();
		DialogEnum dialogEnum = DialogEnum.DeviceConnecting;
		for (int num8 = 0; num8 < 103; num8++)
		{
			dialogEnum = (DialogEnum)num8;
			string xpath = "Dialog/" + dialogEnum;
			foreach (XmlNode item11 in xmlNode2.SelectNodes(xpath))
			{
				Dialogs.Add(item11.InnerText);
			}
		}
	}
}
