using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Transform mapCenter;
    public GameObject tilePrifab;
    public Inventory inventory;
    public PhotonView PV;

    public int mapSizeX;
    public int mapSizeY;
    public float offset = 0.05f;

    public GameObject[][] map;

    public Vector2 GetSizeTile()
    {
        float sizeTileX = tilePrifab.GetComponent<SpriteRenderer>().sprite.rect.width / 100f * tilePrifab.transform.localScale.x + offset;
        float sizeTileY = tilePrifab.GetComponent<SpriteRenderer>().sprite.rect.height / 100f * tilePrifab.transform.localScale.y + offset;
        return new Vector2(sizeTileX, sizeTileY);
    }

    public Vector3 GetLeftTopMap()
    {
        Vector2 sizeTile = GetSizeTile();
        return mapCenter.position + new Vector3(-mapSizeX / 2 * sizeTile.x, -mapSizeY / 2 * sizeTile.y);
    }

    public void GenerateMap()
    {
        Vector2 sizeTile = GetSizeTile();
        map = new GameObject[mapSizeX][];
        for (int x = 0; x < mapSizeX; x++)
        {
            map[x] = new GameObject[mapSizeY];
            for(int y = 0; y < mapSizeY; y++)
            {
                map[x][y] = Instantiate(tilePrifab, GetLeftTopMap() + new Vector3(x * sizeTile.x, y * sizeTile.y), 
                    Quaternion.identity);
                map[x][y].GetComponent<Tile>().ConstructorTile(inventory);
            }
        }
    }

    [PunRPC]
    public void UpdateTileOnEdit(int x, int y, bool isEdit, int personId)
    {
        LetterTile letterTile = map[x][y].GetComponent<LetterTile>();
        if (letterTile != null)
        {
            Person person = null;
            if (personId != -1) 
            { 
                for (int i = 0; i < inventory.gamePlayManager.personManager.persons.Count; i++)
                    if (inventory.gamePlayManager.personManager.persons[i].id == personId)
                        person = inventory.gamePlayManager.personManager.persons[i];
            }
            if (isEdit)
                letterTile.SetLetter(' ', person);
            else
                letterTile.UnsetLetter();
        }
    }

    void Awake()
    {
        GenerateMap();
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        
    }
}
