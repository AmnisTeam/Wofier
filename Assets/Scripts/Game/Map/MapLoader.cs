using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapLoader
{
    
    public static void LoadMap(string path, out GameObject[][] map, MapGenerator mapGenerator)
    {
        StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
        int sizeX = reader.Read();
        int sizeY = reader.Read();

        RegisterGameObjects registerTiles = GameObject.Find("RegisterTiles").GetComponent<RegisterGameObjects>();

        map = new GameObject[sizeX][];
        for (int x = 0; x < sizeX; x++)
            map[x] = new GameObject[sizeY];

        Vector2 sizeTile = mapGenerator.GetSizeTile();
        for (int y = 0; y < sizeY; y++)
        {
            string str = reader.ReadLine();
            for (int x = 0; x < sizeX; x++)
            {
                int id = str[x] - 48;
                map[x][y] = MapGenerator.Instantiate(registerTiles.gameObjects[id], mapGenerator.GetLeftTopMap() + new Vector3(x * sizeTile.x, y * sizeTile.y),
    Quaternion.identity);
                map[x][y].GetComponent<Tile>().ConstructorTile(mapGenerator.inventory);
            }
        }
    }

    public static void SaveMap(string path, ref GameObject[][] map)
    {
        int sizeX = map.Length;
        int sizeY = map[0].Length;

        StreamWriter writer = new StreamWriter(File.Open(path, FileMode.OpenOrCreate));
        writer.Write(sizeX + " " + sizeY + "\n");

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                writer.Write(map[x][y].GetComponent<IIdexable>().GetId() + 48);
            }
            writer.Write('\n');
        }

        writer.Close();
    }

}
