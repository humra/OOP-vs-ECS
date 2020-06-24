using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner_Demo_OOP : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerSpawnPoints;
    [SerializeField]
    private GameObject[] computerSpawnPoints;
    [SerializeField]
    private GameObject[] spawnablePlayerUnits;
    [SerializeField]
    private GameObject[] spawnableComputerUnits;

    private ISpawnManager spawnManager;

    public void SpawnUnitPlayer(int laneIndex, int spawnableIndex, bool multiple)
    {
        if (!multiple)
        { 
            GameObject newUnit = Instantiate(spawnablePlayerUnits[spawnableIndex], playerSpawnPoints[laneIndex].transform.position, spawnablePlayerUnits[spawnableIndex].transform.rotation);
            newUnit.GetComponent<UnitAI>().SetTarget(computerSpawnPoints[laneIndex].transform);
            newUnit.GetComponent<UnitAI>().SetPlayerOwned(true);
            newUnit.GetComponent<UnitAI>().unitManager = FindObjectOfType<GameManager_Demo_OOP>();
            spawnManager.AddPlayerUnit(newUnit.GetComponent<UnitAI>());
        }
        else
        {
            GameObject spawnGroup = Instantiate(spawnablePlayerUnits[spawnableIndex], playerSpawnPoints[laneIndex].transform.position, spawnablePlayerUnits[spawnableIndex].transform.rotation);
            UnitAI[] groupMembers = spawnGroup.GetComponentsInChildren<UnitAI>();
            spawnGroup.GetComponent<UnitGroupDestroyer>().SetActiveUnitsCount(groupMembers.Length);
            spawnManager.AddPlayerUnits(groupMembers);

            for (int i = 0; i < groupMembers.Length; i++)
            {
                groupMembers[i].SetTarget(computerSpawnPoints[laneIndex].transform);
                groupMembers[i].SetPlayerOwned(true);
                groupMembers[i].SetGroupMember(true);
                groupMembers[i].unitManager = FindObjectOfType<GameManager_Demo_OOP>();
            }
        }
    }

    public void SpawnUnitComputer(int laneIndex, int spawnableIndex, bool multiple)
    {
        if (!multiple)
        {
            GameObject newUnit = Instantiate(spawnableComputerUnits[spawnableIndex], computerSpawnPoints[laneIndex].transform.position, Quaternion.identity);
            newUnit.GetComponent<UnitAI>().SetTarget(playerSpawnPoints[laneIndex].transform);
            newUnit.GetComponent<UnitAI>().SetPlayerOwned(false);
            newUnit.GetComponent<UnitAI>().unitManager = FindObjectOfType<GameManager_Demo_OOP>();
            spawnManager.AddComputerUnit(newUnit.GetComponent<UnitAI>());
        }
        else
        {
            GameObject spawnGroup = Instantiate(spawnableComputerUnits[spawnableIndex], computerSpawnPoints[laneIndex].transform.position, Quaternion.identity);
            UnitAI[] groupMemebers = spawnGroup.GetComponentsInChildren<UnitAI>();
            spawnGroup.GetComponent<UnitGroupDestroyer>().SetActiveUnitsCount(groupMemebers.Length);
            spawnManager.AddComputerUnits(groupMemebers);

            for (int i = 0; i < groupMemebers.Length; i++)
            {
                groupMemebers[i].SetTarget(playerSpawnPoints[laneIndex].transform);
                groupMemebers[i].SetPlayerOwned(false);
                groupMemebers[i].SetGroupMember(true);
                groupMemebers[i].unitManager = FindObjectOfType<GameManager_Demo_OOP>();
            }
        }
    }

    public int GetComputerSpawnableUnitsLength()
    {
        return spawnableComputerUnits.Length;
    }

    public void SetSpawnerManagerInterface(ISpawnManager spawnManager)
    {
        this.spawnManager = spawnManager;
    }
}
