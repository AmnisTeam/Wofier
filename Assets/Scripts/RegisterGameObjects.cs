using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterGameObjects : MonoBehaviour
{
    public GameObject[] gameObjects;

    public void ToIndexGameObjects()
    {
        for(int x = 0; x < gameObjects.Length; x++)
        {
            IIdexable idexable = gameObjects[x].GetComponent<IIdexable>();
            if (idexable == null)
                Debug.Log("Объект в регистре объектов не имеет интерфейс IIdexable.");
            idexable.SetId(x);
        }
    }

    private void Awake()
    {
        ToIndexGameObjects();
    }


}
