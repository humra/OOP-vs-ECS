using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerSpawnPoints;
    [SerializeField]
    private GameObject[] computerSpawnPoints;
    [SerializeField]
    private GameObject[] spawnableUnits;

    private int spawnableUnitsIndex = 0;
    private int activePlayerLaneIndex = 0;

    private ISpawnManager spawnManager;

    public void SelectLane(int laneIndex)
    {
        activePlayerLaneIndex = laneIndex;
    }

    public void SpawnUnitPlayer()
    {
        if(ResourceTracker.PlayerSupplyCurrent < ResourceTracker.UnitSupplyCost[spawnableUnitsIndex])
        {
            return;
        }

        if(spawnableUnitsIndex == 0)
        {
            GameObject newUnit = Instantiate(spawnableUnits[spawnableUnitsIndex], playerSpawnPoints[activePlayerLaneIndex].transform.position, Quaternion.identity);
            newUnit.GetComponent<UnitAI>().SetTarget(computerSpawnPoints[activePlayerLaneIndex].transform);
            newUnit.GetComponent<UnitAI>().SetPlayerOwned(true);
            newUnit.GetComponent<UnitAI>().unitManager = FindObjectOfType<GameManager>();
            spawnManager.AddPlayerUnit(newUnit.GetComponent<UnitAI>());
        }
        else
        {
            GameObject spawnGroup = Instantiate(spawnableUnits[spawnableUnitsIndex], playerSpawnPoints[activePlayerLaneIndex].transform.position, Quaternion.identity);
            UnitAI[] groupMembers = spawnGroup.GetComponentsInChildren<UnitAI>();
            spawnGroup.GetComponent<UnitGroupDestroyer>().SetActiveUnitsCount(groupMembers.Length);
            spawnManager.AddPlayerUnits(groupMembers);

            for (int i = 0; i < groupMembers.Length; i++)
            {
                groupMembers[i].SetTarget(computerSpawnPoints[activePlayerLaneIndex].transform);
                groupMembers[i].SetPlayerOwned(true);
                groupMembers[i].SetGroupMember(true);
                groupMembers[i].unitManager = FindObjectOfType<GameManager>();
            }
        }

        ResourceTracker.PlayerSupplyCurrent -= ResourceTracker.UnitSupplyCost[spawnableUnitsIndex];
        ResourceTracker.PlayerSupplyCurrent = Mathf.Clamp(ResourceTracker.PlayerSupplyCurrent, 0, ResourceTracker.PlayerSupplyMax);
    }

    public bool SpawnUnitComputer(int laneIndex, int spawnableIndex)
    {
        if(ResourceTracker.ComputerSupplyCurrent < ResourceTracker.UnitSupplyCost[spawnableIndex])
        {
            Debug.Log("Not enough resources to spawn computer units!");
            return false;
        }

        if(spawnableIndex == 0)
        {
            GameObject newUnit = Instantiate(spawnableUnits[spawnableIndex], computerSpawnPoints[laneIndex].transform.position, Quaternion.identity);
            newUnit.GetComponent<UnitAI>().SetTarget(playerSpawnPoints[laneIndex].transform);
            newUnit.GetComponent<UnitAI>().SetPlayerOwned(false);
            newUnit.GetComponent<UnitAI>().unitManager = FindObjectOfType<GameManager>();
            spawnManager.AddComputerUnit(newUnit.GetComponent<UnitAI>());
        }
        else
        {
            GameObject spawnGroup = Instantiate(spawnableUnits[spawnableIndex], computerSpawnPoints[laneIndex].transform.position, Quaternion.identity);
            UnitAI[] groupMemebers = spawnGroup.GetComponentsInChildren<UnitAI>();
            spawnGroup.GetComponent<UnitGroupDestroyer>().SetActiveUnitsCount(groupMemebers.Length);
            spawnManager.AddComputerUnits(groupMemebers);

            for(int i = 0; i < groupMemebers.Length; i++)
            {
                groupMemebers[i].SetTarget(playerSpawnPoints[laneIndex].transform);
                groupMemebers[i].SetPlayerOwned(false);
                groupMemebers[i].SetGroupMember(true);
                groupMemebers[i].unitManager = FindObjectOfType<GameManager>();
            }
        }

        ResourceTracker.ComputerSupplyCurrent -= ResourceTracker.UnitSupplyCost[spawnableIndex];
        ResourceTracker.ComputerSupplyCurrent = Mathf.Clamp(ResourceTracker.ComputerSupplyCurrent, 0, ResourceTracker.ComputerSupplyMax);
        return true;
    }

    public void IncreaseSpawnIndex()
    {
        if(spawnableUnitsIndex + 1 < spawnableUnits.Length)
        {
            spawnableUnitsIndex++;
        }
    }

    public void DecreaseSpawnIndex()
    {
        if(spawnableUnitsIndex != 0)
        {
            spawnableUnitsIndex--;
        }
    }

    public int GetLaneLength()
    {
        return computerSpawnPoints.Length;
    }

    public int GetSpawnableUnitsLength()
    {
        return spawnableUnits.Length;
    }

    public void SetSpawnerManagerInterface(ISpawnManager spawnManager)
    {
        this.spawnManager = spawnManager;
    }
}