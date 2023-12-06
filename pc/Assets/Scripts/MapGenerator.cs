using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum MapElementsEncoding
{
    NONE = 0,
    TRAP = 1,
    DESTRUCTIBLE_WALL = 2,
    INDESTRUCTIBLE_WALL = 3
}
public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int mapWidth;

    [SerializeField]
    private int mapHeight;

    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject mech1;

    [SerializeField]
    private GameObject mech2;

    [SerializeField]
    private int obstaclesSpawnRowRestriction;

    [SerializeField]
    private int obstaclesSpawnColRestriction;

    [SerializeField]
    private int chunkSize;

    [SerializeField]
    private GameObject trapPrefab;

    [SerializeField]
    private GameObject indestructibleWallPrefab;

    [SerializeField]
    private GameObject destructibleWallPrefab;

    private ChunkGenerator chunkGenerator;

    private int[,] mapMatrix;

    private const int STARTING_X = -1;
    private const int STARTING_Y = -1;

    private (int, int) spawnPoint1;
    private (int, int) spawnPoint2;

    void Start()
    {
        mapMatrix = new int[mapWidth, mapHeight];
        spawnPoint1 = (mapWidth - 2, mapHeight - 2);
        spawnPoint2 = (1, 1);
        mapMatrix[spawnPoint1.Item1, spawnPoint1.Item2] = 10;//Set spawnpoints 
        mapMatrix[spawnPoint2.Item1, spawnPoint2.Item2] = 10;
        chunkSize = CalculateChunkSize(mapWidth, mapHeight);
        chunkGenerator = GetComponent<ChunkGenerator>();
        GenerateWalls();
        GenerateFloor();
        PutObstaclesIntoMapMatrix();
        GenerateObstacles();
        MovePlayersToSpawnPoints();
    }

    private void GenerateWalls()
    {
        for (int x = STARTING_X; x < mapWidth + STARTING_X + 2; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, STARTING_Y), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(x, mapHeight + STARTING_Y + 1), Quaternion.identity);
        }

        for (int y = STARTING_Y; y < mapHeight + STARTING_Y + 1; y++)
        {
            Instantiate(wallPrefab, new Vector3(STARTING_X , y), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(mapWidth + STARTING_X + 1, y), Quaternion.identity);

        }
    }

    private void GenerateFloor()
    {
        for(int x = STARTING_X + 1; x < STARTING_X + 1 + mapWidth; x++)
        {
            for(int y = STARTING_Y + 1; y < STARTING_Y + 1 + mapHeight; y++)
            {
                Instantiate(floorPrefab, new Vector3(x, y), Quaternion.identity);
            }
        }
    }


    public int[,] GetMapMatrix()
    {
        return mapMatrix;
    }


    public void MovePlayersToSpawnPoints()
    {
        mech1.transform.position = new Vector3(spawnPoint1.Item1, spawnPoint1.Item2);
        mech2.transform.position = new Vector3(spawnPoint2.Item1, spawnPoint2.Item2);
    }

    private int CalculateChunkSize(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private void PutObstaclesIntoMapMatrix()
    {
        ChunkGenerator trapChunkGenerator = new ChunkGenerator(0.1f, 0.02f, 0.05f);
        ChunkGenerator destructibleWallChunkGenerator = new ChunkGenerator(0.05f, 0.02f, 0.1f);
        ChunkGenerator indestructibleWallGenerator = new ChunkGenerator(0.05f, 0.05f, 0.1f);
        ChunkGenerator[] generators = new ChunkGenerator[] { trapChunkGenerator, destructibleWallChunkGenerator, indestructibleWallGenerator };

        for (int x = 0;  x < mapWidth; x += chunkSize){
            for(int y = 0; y < mapHeight; y += chunkSize)
            {
                ChunkGenerator chunkGenerator = generators[Random.Range(0, 3)];
                for(int x1 = x; x1 < x + chunkSize; x1++)
                {
                    for(int y1 = y; y1 < y + chunkSize; y1++)
                    {
                        mapMatrix[x1, y1] = (int)chunkGenerator.SpawnObstacle();
                        Debug.Log("X: " + x1 + " Y: " + y1 + "Value: " + mapMatrix[x1,y1]);
                    }
                }
            }
        }
    }

    public void GenerateObstacles()
    {
        for (int x = 0; x < mapWidth; x ++)
        {
            for (int y = 0; y < mapHeight; y ++)
            {
                MapElementsEncoding value = (MapElementsEncoding) mapMatrix[x,y];
                if(value == MapElementsEncoding.NONE)
                {
                    continue;
                }
                else if(value == MapElementsEncoding.TRAP)
                {
                    Instantiate(trapPrefab, new Vector3(x, y), Quaternion.identity);//.transform.SetParent(gameObject.transform);
                }
                else if (value == MapElementsEncoding.DESTRUCTIBLE_WALL)
                {
                    Instantiate(destructibleWallPrefab, new Vector3(x, y), Quaternion.identity);//.transform.SetParent(gameObject.transform);
                }
                else if (value == MapElementsEncoding.INDESTRUCTIBLE_WALL)
                {
                    Instantiate(indestructibleWallPrefab, new Vector3(x, y), Quaternion.identity);//.transform.SetParent(gameObject.transform);
                }
            }
        }
    }
}
