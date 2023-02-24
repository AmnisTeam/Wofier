using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public Person me;
    public PersonsManager personManager;
    public ScoreTableManager scoreTableManager;

    public GameObject findingMenu;
    public TMPro.TMP_Text findingMenuNickname;

    public TMPro.TMP_Text timeNickname;
    public TMPro.TMP_Text time;

    public int idPlayingPerson = -1;

    public float timeToPlayingOnePerson;
    private float timerToPlayerOnePerson = float.NaN;

    public float timeToAppearanceFindingMenu;
    public float findingMenuShowingTime;
    private float findingMenuShowingTimer = float.NaN;

    public void SelectNextPersonToPlay()
    {
        idPlayingPerson = (idPlayingPerson + 1) % personManager.persons.Count;
        timerToPlayerOnePerson = timeToPlayingOnePerson;

        findingMenuNickname.text = personManager.persons[idPlayingPerson].nickname;
        findingMenuNickname.color = personManager.persons[idPlayingPerson].color;
        findingMenu.SetActive(true);
        findingMenu.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceFindingMenu);
        findingMenuShowingTimer = findingMenuShowingTime;

        timeNickname.text = personManager.persons[idPlayingPerson].nickname;
        timeNickname.color = personManager.persons[idPlayingPerson].color;

        scoreTableManager.updateTable();
    }

    public void OnCompleteAnitaion(object gameObject)
    {
        (gameObject as GameObject).SetActive(false);
    }

    void Awake()
    {
        personManager.connectPerson(me);
        personManager.connectPerson(new Person(1, "ThEnd", new Color(1, 0.6f, 0.6f, 1), 1));
        personManager.connectPerson(new Person(2, "DotaKot", new Color(0.6f, 1, 0.6f, 1), 2));
        personManager.connectPerson(new Person(3, "SpectreSpect", new Color(0.6f, 0.6f, 1, 1), 3));
        personManager.connectPerson(new Person(4, "Hexumee", new Color(1, 0.6f, 1, 1), 4));
        personManager.persons[1].score = 200;
    }

    void Start()
    {
        SelectNextPersonToPlay();
    }

    void Update()
    {
        findingMenuShowingTimer -= Time.deltaTime;
        if(findingMenuShowingTimer < 0)
        {
            findingMenu.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceFindingMenu).setOnComplete(OnCompleteAnitaion, findingMenu);
            findingMenuShowingTimer = float.NaN;
        }

        timerToPlayerOnePerson -= Time.deltaTime;
        if(timerToPlayerOnePerson < 0)
        {
            SelectNextPersonToPlay();
        }

        int secundes = (int)(timerToPlayerOnePerson % 60);
        int minutes = (int)(timerToPlayerOnePerson / 60);

        time.text = minutes.ToString("00") + ":" + secundes.ToString("00");
    }
}
