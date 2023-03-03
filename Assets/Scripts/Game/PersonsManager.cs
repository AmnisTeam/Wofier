using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonsManager : MonoBehaviour
{
    public List<Person> persons;

    public void connectPerson(Person person)
    {
        persons.Add(person);
        Debug.Log("����� " + person.nickname + " ����������� � ����");
    }

    public void disconnectPerson(Person person)
    {
        Debug.Log("����� " + person.nickname + " ���������� �� ����");
        persons.Remove(person);

    }

    void Awake()
    {
        persons = new List<Person>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
