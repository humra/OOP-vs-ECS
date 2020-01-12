using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();

        CheckForMissingComponents();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawner.SelectLane(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawner.SelectLane(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawner.SelectLane(2);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            spawner.SpawnUnit(0);
        }
    }

    private void CheckForMissingComponents()
    {
        if(spawner == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: Spawner");
        }
    }
}
