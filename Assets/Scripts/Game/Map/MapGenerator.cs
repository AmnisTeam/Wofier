using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Transform mapCenter;
    public GameObject tilePrifab;

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
            }
        }
    }

    void Awake()
    {
        GenerateMap();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}