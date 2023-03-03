using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonsManager : MonoBehaviour
{
    public List<Person> persons;

    public void connectPerson(Person person)
    {
        persons.Add(person);
        Debug.Log("Игрок " + person.nickname + " подключился к игре");
    }

    public void disconnectPerson(Person person)
    {
        Debug.Log("Игрок " + person.nickname + " отключился от игры");
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
