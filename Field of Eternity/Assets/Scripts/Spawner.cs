using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerSpawnPoints;
    [SerializeField]
    private GameObject[] AISpawnPoints;
    [SerializeField]
    private GameObject[] spawnableUnits;

    private int spawnableUnitsIndex = 0;
    private int activePlayerLaneIndex = 0;

    public void SelectLane(int laneIndex)
    {
        activePlayerLaneIndex = laneIndex;
    }

    public void SpawnUnit()
    {
        if(spawnableUnitsIndex == 0)
        {
            GameObject newUnit = Instantiate(spawnableUnits[spawnableUnitsIndex], playerSpawnPoints[activePlayerLaneIndex].transform.position, Quaternion.identity);
            newUnit.GetComponent<UnitAI>().SetTarget(AISpawnPoints[activePlayerLaneIndex].transform);
            newUnit.GetComponent<UnitAI>().SetPlayerOwned(true);
        }
        else
        {
            GameObject spawnGroup = Instantiate(spawnableUnits[spawnableUnitsIndex], playerSpawnPoints[activePlayerLaneIndex].transform.position, Quaternion.identity);
            UnitAI[] groupMembers = spawnGroup.GetComponentsInChildren<UnitAI>();
            spawnGroup.GetComponent<UnitGroupDestroyer>().SetActiveUnitsCount(groupMembers.Length);
            
            for(int i = 0; i < groupMembers.Length; i++)
            {
                groupMembers[i].SetTarget(AISpawnPoints[activePlayerLaneIndex].transform);
                groupMembers[i].SetPlayerOwned(true);
                groupMembers[i].SetGroupMember(true);
            }
        }
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
}
