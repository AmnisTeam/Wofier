using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerPerson
{
    public Person person;
    public TilesWordInfo word;
}

public class EndMenuManager : MonoBehaviour
{
    public EndGamePlayersTable playersTable;
    public EndGameResultPanel resultPanel;

    public void SetEndMenuData(List<Person> persons, WinnerPerson topWordWinner, WinnerPerson longestWordWinner)
    {
        persons.Sort((Person a, Person b) =>
        {
            if (a.score > b.score) return -1;
            else if (a.score < b.score) return 1;
            else return 0;
        });

        playersTable.SetTable(persons);

        //Победитель
        resultPanel.winner.SetWinner(persons[0]);

        //Самое дорогое слово
        resultPanel.topWordWinner.prizeWinner.SetWinner(topWordWinner.person);
        resultPanel.topWordWinner.word.SetWord(topWordWinner.word);

        //Самое длинное слово
        resultPanel.longestWordWinner.prizeWinner.SetWinner(longestWordWinner.person);
        resultPanel.longestWordWinner.word.SetWord(longestWordWinner.word);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
