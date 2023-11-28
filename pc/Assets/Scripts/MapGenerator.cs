using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;
    public GameObject barrel;
    public GameObject lava;
    public GameObject mech1;
    public GameObject mech2;
    public float lavaChance;
    public float barrelChance;
    public int obstaclesArea; //Przeszkody będą losowo generowane w kwadracie o zadanym boku. Musi to być liczba parzysta
    public int mapWidth;
    public int mapHeight;
    private float tileSize;
    // Start is called before the first frame update
    void Start()
    {
        tileSize = floor.transform.localScale.x;
        GenerateWalls();
        GenerateFloor();
        GenerateObstacles(barrelChance, lavaChance);
        PlaceMechs();
    }

    void GenerateWalls()
    {
        float y = tileSize * ((mapWidth/2)) + tileSize;
        float x = -tileSize * (mapHeight/2) - tileSize; 
        for (int i = 0; i < mapHeight + 2; i++)
        {
            GameObject wallObject = Instantiate(wall, gameObject.transform);
            wallObject.transform.localPosition = new Vector2(x, y);
            x += tileSize;
        }
        for(int i = 0;i < mapWidth; i++) 
        {
            y -= tileSize;
            GameObject leftWall = Instantiate(wall, gameObject.transform);
            GameObject rightWall = Instantiate(wall, gameObject.transform);
            leftWall.transform.localPosition = new Vector2(-tileSize * (mapHeight / 2) - tileSize, y);
            rightWall.transform.localPosition = new Vector2(tileSize * (mapHeight / 2), y);
        }
        y = -tileSize * (mapWidth / 2);
        x = -tileSize * (mapHeight / 2) - tileSize;
        for (int i = 0; i < mapHeight + 2; i++)
        {
            GameObject wallObject = Instantiate(wall, gameObject.transform);
            wallObject.transform.localPosition = new Vector2(x, y);
            x += tileSize;
        }
    }

    void GenerateFloor()
    {
        float y = tileSize * (mapHeight / 2) + 2 * tileSize;
        for (int i = 0; i < mapWidth; i++)
        {
            float x = -tileSize * (mapWidth / 2) + 2 * tileSize ;
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject floorObject = Instantiate(floor, gameObject.transform);
                floorObject.transform.localPosition = new Vector2(x, y);
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
                    GameObject obstacleObject = Instantiate(barrel, gameObject.transform);
                    obstacleObject.transform.localPosition = new Vector2(x, y);
                }
                else if (randNumber >= lavaChance)
                {
                    GameObject obstacleObject = Instantiate(lava, gameObject.transform);
                    obstacleObject.transform.localPosition = new Vector2(x, y);
                }
                x += tileSize;
            }
            y-= tileSize;
        }
    }
    void PlaceMechs()
    {
        mech1.transform.position = new Vector2(mapWidth /2, mapHeight /2 - tileSize);
        mech2.transform.position = new Vector2(mapWidth /-2, mapHeight /-2 + tileSize);
        Debug.Log("Position 1" + new Vector2(mapWidth / 2, mapHeight / 2 - tileSize));
        Debug.Log("Position 2" + new Vector2(mapWidth / -2, mapHeight / -2 + tileSize));

    }
}
