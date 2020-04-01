using UnityEngine;

public class ComputerSpawn
{
    public int laneIndex;
    public int spawnableUnitsIndex;

    public ComputerSpawn(int laneLength, int spawnableUnitsLength)
    {
        laneIndex = Random.Range(0, laneLength - 1);
        spawnableUnitsIndex = Random.Range(0, spawnableUnitsLength - 1);
    }
}
