using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class NetSpellDictionary : WordDictionary
{
    NetSpell.SpellChecker.Spelling oSpell;
    NetSpell.SpellChecker.Dictionary.WordDictionary oDict;

    string result = "";

    public NetSpellDictionary()
    {
        oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
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
            while(!wwwfile.isDone) { }
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "en-US.dic");
            File.WriteAllBytes(filepath, wwwfile.bytes);

            DicPath = filepath;

        }
        else
        {
            DicPath = Application.streamingAssetsPath + "/en-US.dic";
        }

        oDict.DictionaryFile = DicPath;

/*
        for (int i = 0; i < 1000; i++)
            Debug.Log(DicPath);*/

        oDict.Initialize();

        oSpell.Dictionary = oDict;
    }

    public bool checkWord(string word)
    {
        return oSpell.TestWord(word);
    }

}
