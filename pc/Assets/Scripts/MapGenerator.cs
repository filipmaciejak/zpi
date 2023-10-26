using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;
    public GameObject barrel;
    public GameObject lava;
    public float lavaChance;
    public float barrelChance;
    public int obstaclesArea; //Przeszkody będą losowo generowane w kwadracie o zadanym boku. Musi to być liczba parzysta
    public int mapSize;
    private float tileSize;
    // Start is called before the first frame update
    void Start()
    {
        tileSize = 1;
        GenerateWalls();
        GenerateFloor();
        GenerateObstacles(barrelChance, lavaChance);
    }

    void GenerateWalls()
    {
        float y = tileSize * ((mapSize/2)) + tileSize;
        float x = -tileSize * (mapSize/2) - tileSize;
        for (int i = 0; i < mapSize + 2; i++)
        {
            Instantiate(wall, new Vector2(x, y), Quaternion.identity);
            x += tileSize;
        }
        for(int i = 0;i < mapSize; i++) 
        {
            y -= tileSize;
            Instantiate(wall, new Vector2(-tileSize * (mapSize / 2) - tileSize, y), Quaternion.identity);
            Instantiate(wall, new Vector2(tileSize * (mapSize / 2), y), Quaternion.identity);
        }
        y = -tileSize * (mapSize / 2);
        x = -tileSize * (mapSize / 2) - tileSize;
        for (int i = 0; i < mapSize + 2; i++)
        {
            Instantiate(wall, new Vector2(x, y), Quaternion.identity);
            x += tileSize;
        }
    }

    void GenerateFloor()
    {
        float y = tileSize * (mapSize / 2);
        for (int i = 0; i < mapSize; i++)
        {
            float x = -tileSize * (mapSize / 2);
            for (int j = 0; j < mapSize; j++)
            {
                Instantiate(floor, new Vector2(x, y), Quaternion.identity);
                x += tileSize;
            }
            y -= tileSize;
        }
    }

    void GenerateObstacles(float barrelChance, float lavaChance)
    {
        lavaChance = 1 - lavaChance;
        float y = (obstaclesArea/ 2) * tileSize;
        for(int i = 0;i < obstaclesArea; i++)
        {
            float x = (obstaclesArea / 2) * tileSize * -1;
            for (int j = 0; j < obstaclesArea; j++)
            {
                float randNumber = Random.Range(0.0f, 1);
                if (randNumber <= barrelChance)
                {
                    Instantiate(barrel, new Vector2(x, y), Quaternion.identity);
                }
                else if (randNumber >= lavaChance)
                {
                    Instantiate(lava, new Vector2(x, y), Quaternion.identity);
                }
                x += tileSize;
            }
            y-= tileSize;
        }
    }
 }
