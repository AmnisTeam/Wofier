using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDictionary : WordDictionary
{
    public Dictionary<string, bool> dictionary;

    public TempDictionary()
    {
        dictionary.Add("dog", true);
        dictionary.Add("cat", true);
        dictionary.Add("house", true);
        dictionary.Add("car", true);
        dictionary.Add("table", true);
        dictionary.Add("value", true);
        dictionary.Add("phone", true);
        dictionary.Add("sphere", true);
    }

    public bool checkWord(string word)
    {
        return dictionary.ContainsKey(word);
    }
}
