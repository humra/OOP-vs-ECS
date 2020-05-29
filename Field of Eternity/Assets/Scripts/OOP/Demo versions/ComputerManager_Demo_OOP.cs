using UnityEngine;

public class ComputerManager_Demo_OOP : MonoBehaviour
{
    [SerializeField]
    private float spawnInterval = 5f;
    [SerializeField]
    private int spawnUpgradeInterval = 3;

    private int currentSpawnIndex = 0;
    private int spawnCounter = 0;

    private IComputerManagerDemo computerManager;

    private void Start()
    {
        InvokeRepeating("SpawnUnits", 1f, spawnInterval);
    }

    private void SpawnUnits()
    {
        computerManager.SpawnUnitsAllLanes(currentSpawnIndex);
        spawnCounter++;

        if(spawnCounter == spawnUpgradeInterval && currentSpawnIndex < computerManager.GetSpawnableUnitsLength() - 1)
        {
            currentSpawnIndex++;
            spawnCounter = 0;
        }

        if(spawnCounter == spawnUpgradeInterval && currentSpawnIndex == computerManager.GetSpawnableUnitsLength() - 1)
        {
            CancelInvoke("SpawnUnits");
        }
    }

    public void SetComputerManagerInterface(IComputerManagerDemo computerManager)
    {
        this.computerManager = computerManager;
    }
}
