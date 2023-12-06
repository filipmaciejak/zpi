using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator: MonoBehaviour
{

    
    private float trapSpawnChance;
    private float indestructibleWallSpawnChance;
    private float destructibleWallSpawnChance;

    public ChunkGenerator(float trapSpawnChance, float indestructibleWallSpawnChance, float destructibleWallSpawnChance)
    {
        this.trapSpawnChance = trapSpawnChance;
        this.indestructibleWallSpawnChance = indestructibleWallSpawnChance;
        this.destructibleWallSpawnChance = destructibleWallSpawnChance;

    }

    public MapElementsEncoding SpawnObstacle()
    {
        float spawnChance = Random.Range(0f, 1f);
        if (spawnChance < trapSpawnChance)
        {
            return MapElementsEncoding.TRAP;
        }
        else if (spawnChance < (destructibleWallSpawnChance + trapSpawnChance))
        {
            return MapElementsEncoding.DESTRUCTIBLE_WALL;
        }

        else if (spawnChance < (destructibleWallSpawnChance + indestructibleWallSpawnChance + trapSpawnChance))
        {
            return MapElementsEncoding.INDESTRUCTIBLE_WALL;
        }

        return MapElementsEncoding.NONE;
    }
}
