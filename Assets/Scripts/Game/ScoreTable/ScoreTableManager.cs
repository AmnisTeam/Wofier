using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTableManager : MonoBehaviour
{
    public GameObject scoreTable;
    public GameObject personRowPrifab;
    public PersonsManager personManager;
    public float offset = 15;
    public int countShow = 3;
    public bool isHidden = true;

    public GameObject showScoreTableButton;
    public GameObject hiddenScoreTableButton;

    public List<PersonRow> personRows;
    private bool blockUpdate = false;

    public float timeHidden = 0.2f;

    public void EnableGameObject(object gameObject)
    {
        (gameObject as GameObject).SetActive(true);
    }

    public void DisableGameObject(object gameObject)
    {
        (gameObject as GameObject).SetActive(false);
    }

    public void UnBlockUpdateAndUpdate()
    {
        blockUpdate = false;
        updateTable();
    }

    public void ShowAllPlayers()
    {
        blockUpdate = true;
        isHidden = false;
        showScoreTableButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeHidden).setOnComplete(DisableGameObject, showScoreTableButton);
        for(int x = countShow; x < personRows.Count; x++)
        {
            personRows[x].gameObject.SetActive(true);
            personRows[x].GetComponent<CanvasGroup>().LeanAlpha(1, timeHidden);
        }
        hiddenScoreTableButton.SetActive(true);
        hiddenScoreTableButton.GetComponent<CanvasGroup>().LeanAlpha(1, timeHidden).setOnComplete(UnBlockUpdateAndUpdate);

        hiddenScoreTableButton.transform.localPosition = new Vector3(
    personRowPrifab.GetComponent<RectTransform>().rect.width / 2 - hiddenScoreTableButton.GetComponent<RectTransform>().rect.width / 2,
    -(personRowPrifab.GetComponent<RectTransform>().rect.height + offset) * personRows.Count);
    }

    public void HiddenAllPlayers()
    {
        blockUpdate = true;
        isHidden = true;
        hiddenScoreTableButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeHidden).setOnComplete(DisableGameObject, hiddenScoreTableButton);
        for (int x = countShow; x < personRows.Count; x++)
            personRows[x].GetComponent<CanvasGroup>().LeanAlpha(0, timeHidden).setOnComplete(DisableGameObject, personRows[x].gameObject);

        showScoreTableButton.SetActive(true);
        showScoreTableButton.GetComponent<CanvasGroup>().LeanAlpha(1, timeHidden).setOnComplete(UnBlockUpdateAndUpdate);

        showScoreTableButton.transform.localPosition = new Vector3(
personRowPrifab.GetComponent<RectTransform>().rect.width / 2 - showScoreTableButton.GetComponent<RectTransform>().rect.width / 2,
-(personRowPrifab.GetComponent<RectTransform>().rect.height + offset) * Mathf.Min(countShow, personRows.Count));
    }

    public void updateTable()
    {
        if (!blockUpdate)
        {
            List<Person> sortedPersons = new List<Person>();
            for (int x = 0; x < personManager.persons.Count; x++)
                sortedPersons.Add(personManager.persons[x]);

            sortedPersons.Sort((Person x, Person y) =>
            {
                if (x.score > y.score) return -1;
                else if (x.score < y.score) return 1;
                else return 0;
            });

            for (int x = 0; x < personRows.Count; x++)
                Destroy(personRows[x].gameObject);
            personRows.Clear();

            for (int x = 0; x < personManager.persons.Count; x++)
            {
                PersonRow row = Instantiate(personRowPrifab, scoreTable.transform).GetComponent<PersonRow>();
                row.nickname.text = sortedPersons[x].nickname;
                row.nickname.color = sortedPersons[x].color;
                row.score.text = sortedPersons[x].score.ToString();
                row.transform.localPosition = new Vector3(0, -(row.GetComponent<RectTransform>().rect.height + offset) * x);
                if (isHidden)
                {
                    if (x >= countShow)
                    {
                        row.GetComponent<CanvasGroup>().alpha = 0;
                        row.gameObject.SetActive(false);
                    }
                }
                personRows.Add(row);
            }

            showScoreTableButton.transform.localPosition = new Vector3(
                personRowPrifab.GetComponent<RectTransform>().rect.width / 2 - showScoreTableButton.GetComponent<RectTransform>().rect.width / 2,
                -(personRowPrifab.GetComponent<RectTransform>().rect.height + offset) * Mathf.Min(countShow, personRows.Count));

            hiddenScoreTableButton.transform.localPosition = new Vector3(
                personRowPrifab.GetComponent<RectTransform>().rect.width / 2 - hiddenScoreTableButton.GetComponent<RectTransform>().rect.width / 2,
                -(personRowPrifab.GetComponent<RectTransform>().rect.height + offset) * personRows.Count);

            if (isHidden)
            {
                showScoreTableButton.SetActive(true);
                showScoreTableButton.GetComponent<CanvasGroup>().alpha = 1;

                hiddenScoreTableButton.SetActive(false);
                hiddenScoreTableButton.GetComponent<CanvasGroup>().alpha = 0;
            }
            else
            {
                showScoreTableButton.SetActive(false);
                showScoreTableButton.GetComponent<CanvasGroup>().alpha = 0;

                hiddenScoreTableButton.SetActive(true);
                hiddenScoreTableButton.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
