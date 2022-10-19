using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageInfor
{

    public Language activeLang = null;
    private static List<Language> listLng = new List<Language>();
    public readonly static Language defaultLanguage = new Language()
    {
        name = "English",
        pathFile = Application.dataPath+"/TheEndOfWorld/Verse/LangResolve/Lang/English"
    };

    private static readonly List<string> SupportedAutoSelectLanguages = new List<string>
	{
		"Arabic",
		"ChineseSimplified",
		"Czech",
		"Danish",
		"Dutch",
		"English",
		"Estonian",
		"Finnish",
		"French",
		"German",
		"Hungarian",
		"Italian",
		"Japanese",
		"Korean",
		"Norwegian",
		"Polish",
		"Portuguese",
		"PortugueseBrazilian",
		"Romanian",
		"Russian",
		"Slovak",
		"Spanish",
		"SpanishLatin",
		"Swedish",
		"Turkish",
		"Ukrainian"
		};
}
