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

        oDict.DictionaryFile = "en-US.dic";
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
