using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class NetSpellDictionary : WordDictionary
{
    //Dictionary<string, bool> stroka;
    HashSet<string> stroka;

    string result = "";

    public NetSpellDictionary()
    {
        /*oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
        oSpell = new NetSpell.SpellChecker.Spelling();


        string DicPath = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            DicPath = Application.streamingAssetsPath + "/en-US.dic";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //DicPath = "jar:file://" + Application.dataPath + "!/assets/en-US.dic";
            string path = "jar:file://" + Application.dataPath + "!/assets/en-US.dic";
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "en-US.dic");
            File.WriteAllBytes(filepath, wwwfile.bytes);

            DicPath = filepath;
        }
        else
        {
            DicPath = Application.streamingAssetsPath + "/en-US.dic";
        }

        oDict.DictionaryFile = DicPath;

        oDict.Initialize();

        oSpell.Dictionary = oDict;*/
        string DicPath = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            DicPath = Application.streamingAssetsPath + "/en-US.txt";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //DicPath = "jar:file://" + Application.dataPath + "!/assets/en-US.dic";
            string path = "jar:file://" + Application.dataPath + "!/assets/en-US.txt";
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "en-US.txt");
            File.WriteAllBytes(filepath, wwwfile.bytes);

            DicPath = filepath;
        }
        else
        {
            DicPath = Application.streamingAssetsPath + "/en-US.txt";
        }

        stroka = new HashSet<string>();

        var array = File.ReadAllLines(DicPath);
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                stroka.Add(array[i]);
            }
        }
    }

    public bool checkWord(string word)
    {
        return stroka.Contains(word.ToLower());
    }

    /*public bool checkWord2(string word)
    {
        return oSpell.TestWord(word);
    }*/



}
