using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, IIdexable
{
    public Inventory inventory;
    public Person person = null;
    public bool isCanSetItem = true;
    public float probability = 1;
    public int type_id;
    public int completeWordOrder = 0;

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

    public virtual void CompleteWord(TileWord word)
    {

    }

    public int GetId()
    {
        return type_id;
    }

    public void SetId(int id)
    {
        type_id = id;
    }
}
