using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();

        CheckForMissingComponents();

        ResourceTracker.ResetValues();
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
            spawner.SpawnUnit();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            spawner.IncreaseSpawnIndex();
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            spawner.DecreaseSpawnIndex();
        }

        if(ResourceTracker.PlayerHealthCurrent <= 0)
        {
            AIWin();
        }
        else if(ResourceTracker.AIHealthCurrent <= 0)
        {
            PlayerWin();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Player health: " + ResourceTracker.PlayerHealthCurrent + " / " + ResourceTracker.PlayerHealthMax);
            Debug.Log("AI health: " + ResourceTracker.AIHealthCurrent + " / " + ResourceTracker.AIHealthMax);
        }
    }

    private void AIWin()
    {
        //TO-DO
        Debug.Log("AI HAS WON!");
    }

    private void PlayerWin()
    {
        //TO-DO
        Debug.Log("PLAYER HAS WON!");
    }

    private void CheckForMissingComponents()
    {
        if(spawner == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: Spawner");
        }
    }
}
