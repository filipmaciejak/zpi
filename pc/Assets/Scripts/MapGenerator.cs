using UnityEngine;

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
    private GameObject destructibleWallPrefab;

    [SerializeField]
    private float destructibleWallSpawnChance;

    [SerializeField]
    private GameObject trapTilePrefab;

    [SerializeField]
    private float trapTileSpawnChance;

    [SerializeField]
    private GameObject impassableObstaclePrefab;

    [SerializeField]
    private float impassableObstacleSpawnChance;

    [SerializeField]
    private GameObject mech1;

    [SerializeField]
    private GameObject mech2;

    [SerializeField]
    private int obstaclesSpawnRowRestriction;

    [SerializeField]
    private int obstaclesSpawnColRestriction;

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
        GenerateWalls();
        GenerateFloor();
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

    private void GenerateObstacles()
    {
        for (int row = obstaclesSpawnRowRestriction; row < mapWidth - obstaclesSpawnRowRestriction; row++)
        {
            for (int col = obstaclesSpawnColRestriction; col < mapHeight - obstaclesSpawnColRestriction; col++)
            {
                if (mapMatrix[row, col] == 0)
                {
                    TryToGenerateObstacle(row, col);
                }
            }
        }
    }

    public void MovePlayersToSpawnPoints()
    {
        mech1.transform.position = new Vector3(spawnPoint1.Item1, spawnPoint1.Item2);
        mech2.transform.position = new Vector3(spawnPoint2.Item1, spawnPoint2.Item2);
    }

    private void TryToGenerateObstacle(int x, int y)
    {
        float spawnChance = Random.Range(0f, 1f);
        GameObject obstacleToSpawn = null;
        if(spawnChance < trapTileSpawnChance) 
        {
            obstacleToSpawn = trapTilePrefab;
        }
        else if(spawnChance < (destructibleWallSpawnChance + trapTileSpawnChance))
        {
            obstacleToSpawn = destructibleWallPrefab;
        }
        
        else if (spawnChance < (destructibleWallSpawnChance + impassableObstacleSpawnChance + trapTileSpawnChance))
        {
            obstacleToSpawn = impassableObstaclePrefab;
        }

        if(obstacleToSpawn != null)
        {
            Instantiate(obstacleToSpawn, new Vector3(x, y), Quaternion.identity);
        }
    }
}
