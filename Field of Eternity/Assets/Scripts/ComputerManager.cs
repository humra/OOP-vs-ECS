using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    private ComputerSpawn[] computerSpawns = new ComputerSpawn[100];
    private int currentSpawnIndex = 0;
    private int laneLength;
    private int spawnableUnitsLength;
    private IComputerManager computerManager;

    private void Start()
    {
        InvokeRepeating("SpawnUnits", 5f, 3f);
    }

    private void GenerateComputerSpawns()
    {
        for(int i = 0; i < 100; i++)
        {
            computerSpawns[i] = new ComputerSpawn(laneLength, spawnableUnitsLength);
        }

        currentSpawnIndex = 0;
    }

    private void SpawnUnits()
    {
        if(currentSpawnIndex < 100)
        {
            SendToSpawner();
        }
        else
        {
            GenerateComputerSpawns();
            SendToSpawner();
        }
    }

    private void SendToSpawner()
    {
        if (computerManager.SpawnComputerUnit(computerSpawns[currentSpawnIndex].laneIndex, computerSpawns[currentSpawnIndex].spawnableUnitsIndex))
        {
            currentSpawnIndex++;
        }
    }

    public void SetLaneSpawnableUnitsLength(int laneLength, int spawnableUnitsLength)
    {
        this.laneLength = laneLength;
        this.spawnableUnitsLength = spawnableUnitsLength;
    }

    public void SetComputerManagerInterface(IComputerManager computerManager)
    {
        this.computerManager = computerManager;
    }

    public void GenerateSpawns()
    {
        GenerateComputerSpawns();
    }
}
