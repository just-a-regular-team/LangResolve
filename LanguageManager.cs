using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LanguageManager
{
    LanguageInfor languageInfor;

    public LanguageManager()
    {
        instance = this;
        languageInfor = new LanguageInfor();
    }


    public void LoadLang()
    {
        if(isLoadLang)
        {
            return;
        }else
        {
            isLoadLang = true;
            try
            {
                if(languageInfor.activeLang == null)
                {
                    languageInfor.activeLang = LanguageInfor.defaultLanguage;
                }
                languageInfor.activeLang.AlreadyLoadedFiles.Clear();
                string[] directory = System.IO.Directory.GetDirectories(languageInfor.activeLang.pathFile);
                GenFilePath.FindAllAssetInPath(languageInfor.activeLang.pathFile,(string dirPath) =>
                {
                    if (this.TryRegisterFileIfNew(languageInfor.activeLang.pathFile, dirPath))
					{
						this.LoadFromFile_Keyed(dirPath);
					}
                });
                 
            }
            catch (System.Exception ex)
            {
                OnLog.LogError("LanguageLoad","Exception loading language data. Rethrowing. Exception: " + ex);
                throw;
            }
            finally
            {
                isLoadLang = false;
                WhenFinishLangTrans();
            }
        }

    }

    void WhenFinishLangTrans()
    {
       
        while(languageInfor.activeLang.errList.Count > 0)
        {
            string s = languageInfor.activeLang.errList[0];
            languageInfor.activeLang.errList.RemoveAt(0);
            OnLog.LogError("TRANSLATE",s);
        }
    }

    public bool TryRegisterFileIfNew(string dir, string filePath)
		{
			if (!filePath.StartsWith(dir))
			{
				OnLog.LogError("LanguageLoad","Failed to get a relative path for a file: " + filePath + ", located in " + dir);
				return false;
			}
			string item = filePath.Substring(dir.Length);
			if (!this.languageInfor.activeLang.AlreadyLoadedFiles.ContainsKey(dir))
			{
				this.languageInfor.activeLang.AlreadyLoadedFiles[dir] = new HashSet<string>();
			}
			else if (this.languageInfor.activeLang.AlreadyLoadedFiles[dir].Contains(item))
			{
				return false;
			}
			this.languageInfor.activeLang.AlreadyLoadedFiles[dir].Add(item);
			return true;
		}
        
        private void LoadFromFile_Keyed(string file)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(file))
				{
					if (dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.languageInfor.activeLang.errList.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.languageInfor.activeLang.name +'\n'
                        +$"old:{dictionary[xmlKeyValuePair.key]}->new:{xmlKeyValuePair.value}");
                        dictionary[xmlKeyValuePair.key] = xmlKeyValuePair.value;
                        
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				this.languageInfor.activeLang.errList.Add(string.Concat(new object[]
				{
					"Exception loading from translation file ",
					file,
					": ",
					ex
				}));
				dictionary.Clear();
				dictionary2.Clear();
				 
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				Language.LocalizedText keyedReplacement = new Language.LocalizedText(keyValuePair.Key,text);
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.fileSource = file.Split("/").Last();
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file;
				this.languageInfor.activeLang.Localization.SetOrAdd(keyValuePair.Key, keyedReplacement);
			}
		}

    public static string Translate(string textKey)
    {
        string allText = "";
        string[] get = textKey.Split(" ");
        Language.LocalizedText text;
        for(int i = 0;i < get.Length;i++)
        {
            text = null;
            if(!Instance.languageInfor.activeLang.Localization.TryGetValue(get[i],out text))
            {
                return textKey;
            }
             
            if(i == get.Length)
            {
                allText += text.value;
            }else
            {
                allText += text.value + " ";
            }
        }
        
        return allText;

         
        if(!Instance.languageInfor.activeLang.Localization.TryGetValue(textKey,out text))
        {
            return textKey;
        }
        return text.value;
    }
    public static LanguageManager Instance
    {
        get
        {
            if(instance == null)
            {
                new LanguageManager();
            }
            return instance;
        }
    }
    public static LanguageManager instance;

    bool isLoadLang;
}

public static class Translated
{
    public static string Translate(this string textKey)
    {
        textKey = LanguageManager.Translate(textKey); 
        return textKey;
    }
}
