using System.Collections.Generic;
using UnityEngine;
namespace Mao
{
    public class L10N
    {
        static private string _Lang;
        static public string Lang { 
            set {
                _Lang = value;
                PlayerPrefs.SetString("Lang", value);
            }

            get{
                return _Lang;
            }
        }
        static public string GetSystemLang()
        {
            string lang = "en";
            switch (Application.systemLanguage)
            {
                case SystemLanguage.ChineseSimplified:
                    lang = "zh";
                    break;
                case SystemLanguage.ChineseTraditional:
                    lang = "cht";
                    break;
                case SystemLanguage.Korean:
                    lang = "kor";
                    break;
                case SystemLanguage.Japanese:
                    lang = "jp";
                    break;
               // case SystemLanguage.English
            }
            return lang;
        }

        static Dictionary<string, string> _dict = new Dictionary<string, string>();
        static public string L(string key)
        {
            L10nVO vo ;
            if (!VOManager.TryGetVO<L10nVO>(key,out vo))
            {
                return key;
            }

            string ret = vo.en;
            switch (_Lang)
            {
                case "zh":
                    ret = vo.zh;
                    break;
                case "cht":
                    ret = vo.cht;
                    break;
                case "kor":
                    ret = vo.kor;
                    break;
                case "jp":
                    ret = vo.jp;
                    break;
            }

            return ret;
        }

        static public void Init()
        {
            string lang = PlayerPrefs.GetString("Lang", GetSystemLang());
            Lang = lang;
            //SetLang(lang);
        }

        // static private void SetLang(string lang)
        // {
        //     Lang = lang;
        //     _dict.Clear();
        //     var assetInfo = new AssetInfo("L10N");
        //     TextAsset asset = CosmosEntry.ResourceManager.LoadAsset<TextAsset>(assetInfo);
        //     string fileData = asset.text;
        //     var lines = fileData.Split('\n');
        //     string[] fields = lines[0].TrimEnd('\r', '\n').Split(',');
        //     int langIdx = 0;
        //     for (int i = 0; i < fields.Length; i++)
        //     {
        //         if(fields[i].ToLower().Equals(Lang.ToLower())){
        //             langIdx = i;
        //             break;
        //         }
        //     }

        //     for (int i = 1; i < lines.Length; i++)
        //     {
        //         string line = lines[i].TrimEnd('\r', '\n');
        //         if(string.IsNullOrEmpty(line)){
        //             continue;
        //         }
        //         string[] values = line.Split(',');
        //         _dict.Add(values[0],values[langIdx]);
        //     }
        // }
    }


}