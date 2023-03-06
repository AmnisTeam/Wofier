using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetSpellDictionary : WordDictionary
{
    NetSpell.SpellChecker.Spelling oSpell;
    NetSpell.SpellChecker.Dictionary.WordDictionary oDict;

    public NetSpellDictionary()
    {
        oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
        oSpell = new NetSpell.SpellChecker.Spelling();
        /*for (int i = 0; i < 1000; i++)
        {
            Debug.Log(Application.dataPath + "/en-US.dic");
            Console.Write(Application.dataPath + "/en-US.dic" + "\n");
        }*/

        Debug.Log(Application.streamingAssetsPath);
        oDict.DictionaryFile = Application.streamingAssetsPath + "/en-US.dic";
        oDict.Initialize();

        oSpell.Dictionary = oDict;

        //if (oSpell.TestWord("Letters"))
        //    Debug.Log("Yes");
        //else
        //    Debug.Log("No");
    }

    public bool checkWord(string word)
    {
        return oSpell.TestWord(word);
    }
}
