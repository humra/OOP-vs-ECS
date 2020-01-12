using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerSpawnPoints;
    [SerializeField]
    private GameObject[] AISpawnPoints;
    [SerializeField]
    private GameObject[] spawnableUnits;

    private int activePlayerLaneIndex = 0;

    public void SelectLane(int laneIndex)
    {
        activePlayerLaneIndex = laneIndex;
    }

    public void SpawnUnit(int unitIndex)
    {
        Instantiate(spawnableUnits[unitIndex], playerSpawnPoints[activePlayerLaneIndex].transform.position, Quaternion.identity);
    }
}
