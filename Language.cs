using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Language
{
    public string name;
    public string pathFile;
    public Texture2D icon;

    public bool anyError;
    public List<string> errList = new List<string>();


    public Dictionary<string,Language.LocalizedText> Localization = new Dictionary<string, LocalizedText>();
    public Dictionary<string, HashSet<string>> AlreadyLoadedFiles = new Dictionary<string, HashSet<string>>();

    [System.Serializable]
    public class LocalizedText
    {
        private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);
        public string value{get;private set;}
        public string key;
        public LocalizedText(string key,string value)
        {
            this.key = key;
            this.value = value;
        }
        public string Format(object arg0)
        {
          return string.Format(this.value, arg0);
        }

        public string Format(object arg0, object arg1)
        {
          return string.Format(this.value, arg0, arg1);
        }

        public string Format(object arg0, object arg1, object arg2)
        {
          return string.Format(this.value, arg0, arg1, arg2);
        }

        public string Format(params object[] args)
        {
          return string.Format(this.value, args);
        }

        public override string ToString()
        {
          return this.value;
        }
        public static explicit operator string(LocalizedText text)
        {
            return text.value;
        }

        public string fileSource;
		public int fileSourceLine;
		public string fileSourceFullPath;
		public bool isPlaceholder;
    }
}
