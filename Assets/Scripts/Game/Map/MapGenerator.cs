using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Transform mapCenter;
    public GameObject tilePrifab;
    public Inventory inventory;
    public PhotonView PV;
    public GamePlayManager gamePlayManager;

    public int mapSizeX;
    public int mapSizeY;
    public float offset = 0.05f;

    public GameObject[][] map;

    public int[] idToRandSpawn;

    public bool saveMap = true;

    string result = "";

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

    public GameObject getRandomTilePrifab()
    {
        RegisterGameObjects registerTiles = GameObject.Find("RegisterTiles").GetComponent<RegisterGameObjects>();
        float[] probabilities = new float[idToRandSpawn.Length];
        for (int x = 0; x < idToRandSpawn.Length; x++)
            probabilities[x] = registerTiles.gameObjects[idToRandSpawn[x]].GetComponent<Tile>().probability;

        return registerTiles.gameObjects[idToRandSpawn[MyMath.SelectRandomElement(probabilities)]];
    }

    public void GenerateMap()
    {
        Vector2 sizeTile = GetSizeTile();
        map = new GameObject[mapSizeX][];
        for (int x = 0; x < mapSizeX; x++)
        {
            map[x] = new GameObject[mapSizeY];
            for (int y = 0; y < mapSizeY; y++)
            {
                GameObject prifab = getRandomTilePrifab();
                if (x == mapSizeX / 2 && y == mapSizeY / 2)
                    prifab = GameObject.Find("RegisterTiles").GetComponent<RegisterGameObjects>().gameObjects[5]; //Центральный тайл

                map[x][y] = Instantiate(prifab, GetLeftTopMap() + new Vector3(x * sizeTile.x, y * sizeTile.y),
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
            {
                letterTile.SetLetter(' ', person);
            }
            else
                letterTile.UnsetLetter();
        }
    }

    [PunRPC]
    public void UpdateWordOnAccept(int[][] coordX, int[][] coordY, int[][] chars, int[][] personID)
    {
        for (int i = 0; i < coordX.Length; i++)
        {
            TileWord tileWord = new TileWord();
            for (int j = 0; j < coordX[i].Length; j++)
            {
                LetterTile letterTile = map[coordX[i][j]][coordY[i][j]].GetComponent<LetterTile>();
                tileWord.tiles.Add(new Vector2Int(coordX[i][j], coordY[i][j]));

                Person person = null;
                for (int z = 0; z < inventory.gamePlayManager.personManager.persons.Count; z++)
                    if (inventory.gamePlayManager.personManager.persons[z].id == personID[i][j])
                        person = inventory.gamePlayManager.personManager.persons[z];

                tileWord.person = person;

                /*Color colorInWord = new UnityEngine.Color(colorInWordArr[0], colorInWordArr[1], colorInWordArr[2]);
                Color letterText = new UnityEngine.Color(letterTextArr[0], letterTextArr[1], letterTextArr[2]);

                letterTile.colorInWord = colorInWord;
                letterTile.letterText.color = letterText;
                letterTile.GetComponent<SpriteRenderer>().color = colorInWord;

                
                letterTile.colorInWord = new UnityEngine.Color(0.1f, 0.1f, 0.9f);
                letterTile.letterText.color = new UnityEngine.Color(0.1f, 0.9f, 0.1f);
                letterTile.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0.9f, 0.1f, 0.1f);*/


                letterTile.SetLetter((char)chars[i][j], person);
                letterTile.inWord = true;


            }

            gamePlayManager.words.Add(tileWord);
        }
    }

    [PunRPC]
    public void UpdateCompletedWord(int[] completeWordTileX, int[] completeWordTileY, 
                                    int[][] findWordX, int[][] findWordY)
    {
        for (int x = 0; x < completeWordTileX.Length; x++)
        {
            TileWord tileWord = new TileWord();
            for (int y = 0; y < findWordX[x].Length; y++)
            {
                tileWord.tiles.Add(new Vector2Int(findWordX[x][y], findWordY[x][y]));
            }
            map[completeWordTileX[x]][completeWordTileY[x]].GetComponent<Tile>().CompleteWord(tileWord);
        }
    }

    public List<Vector2Int> FindPointsInGame(int maxCountPoints)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        for(int x = 0; x < mapSizeX; x++)
            for(int y = 0; y < mapSizeY; y++)
            {
                LetterTile tile = map[x][y].GetComponent<LetterTile>();
                if(tile && tile.isHaveLetter && tile.inWord)
                {
                    points.Add(new Vector2Int(x, y));
                    break;
                }
            }

        int minX = points[0].x;
        int maxX = points[points.Count - 1].x;
        float offset = (float)(maxX - minX) / maxCountPoints;
        List<Vector2Int> offsetedPoints = new List<Vector2Int>();

        float leftPointer = 0;
        float rightPointer = offset;

        for(int x = 0; x < points.Count; x++)
        {
            if(points[x].x >= leftPointer && points[x].x < rightPointer)
            {
                leftPointer += offset;
                rightPointer += offset;
                offsetedPoints.Add(points[x]);
            }
        }

        return offsetedPoints;
    }

    void Awake()
    {
        string MapPath = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            MapPath = Application.streamingAssetsPath + "/default_map.txt";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //MapPath = "jar:file://" + Application.streamingAssetsPath + "/default_map.txt";
            string path = "jar:file://" + Application.dataPath + "!/assets/default_map.txt";
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "default_map.txt");
            File.WriteAllBytes(filepath, wwwfile.bytes);

            MapPath = filepath;
        }
        else
        {
            MapPath = Application.streamingAssetsPath + "/default_map.txt";
        }


        if (saveMap)
        {
            GenerateMap();
            MapLoader.SaveMap(MapPath, ref map);
        }
        else
        {
            MapLoader.LoadMap(MapPath, out map, this);
        }
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
}
