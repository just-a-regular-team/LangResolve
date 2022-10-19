using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public static class DirectXmlLoaderSimple
{
    public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(string file)
		{
			XDocument xdocument;
            using (Stream stream = new FileStream(file,FileMode.Open,FileAccess.Read))
			{
				xdocument = XDocument.Load(XmlReader.Create(stream), LoadOptions.SetLineInfo);
                foreach (XElement xelement in xdocument.Root.Elements())
			    {
				string key = xelement.Name.ToString();
				string text = xelement.Value;
				text = text.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = text,
					lineNumber = ((IXmlLineInfo)xelement).LineNumber
				};
			    }
			}
			 
			 
			yield break;
			 
		}

		 
		public struct XmlKeyValuePair
		{
			public string key;
			public string value;
			public int lineNumber;
		}
}
