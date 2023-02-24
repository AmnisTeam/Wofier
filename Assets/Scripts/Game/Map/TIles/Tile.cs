using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public Inventory inventory;
    public Person person = null;
    public bool isCanSetItem = true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ConstructorTile(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public virtual void OnSetItem(Item item, Person person)
    {
        this.person = person;
    }
}
