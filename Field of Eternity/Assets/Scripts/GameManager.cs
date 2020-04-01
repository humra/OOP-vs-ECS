using UnityEngine;

public class GameManager : MonoBehaviour, IUnitDestroy, ISupplyChange, IComputerManager
{ 
    private Spawner spawner;
    private UIManager uiManager;
    private SupplyManager supplyManager;
    private ComputerManager computerManager;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        uiManager = GetComponent<UIManager>();
        supplyManager = GetComponent<SupplyManager>();
        computerManager = GetComponent<ComputerManager>();

        CheckForMissingComponents();

        ResourceTracker.ResetValues();
        supplyManager.StartResupplyForBoth();
        supplyManager.SetSupplyChangeInterface(this);
        uiManager.LoadElements();
        uiManager.UpdateUI();
        computerManager.SetComputerManagerInterface(this);
        computerManager.SetLaneSpawnableUnitsLength(spawner.GetLaneLength(), spawner.GetSpawnableUnitsLength());
        computerManager.GenerateSpawns();
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
            spawner.SpawnUnitPlayer();
            uiManager.UpdateUI();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            spawner.IncreaseSpawnIndex();
            uiManager.increaseSelectedIndex();
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            spawner.DecreaseSpawnIndex();
            uiManager.decreaseSelectedIndex();
        }

        if(ResourceTracker.PlayerHealthCurrent <= 0)
        {
            AIWin();
        }
        else if(ResourceTracker.ComputerHealthCurrent <= 0)
        {
            PlayerWin();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Player health: " + ResourceTracker.PlayerHealthCurrent + " / " + ResourceTracker.PlayerHealthMax);
            Debug.Log("AI health: " + ResourceTracker.ComputerHealthCurrent + " / " + ResourceTracker.ComputerHealthMax);
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
        if(uiManager == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: UIManager");
        }
        if(supplyManager == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: SupplyManager");
        }
        if(computerManager == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: ComputerManager");
        }
    }

    public void UnitReachedBase()
    {
        uiManager.UpdateUI();
    }

    public void ResupplyCompleted()
    {
        uiManager.UpdateUI();
    }

    public bool SpawnComputerUnit(int laneIndex, int spawnableUnitIndex)
    {
        return spawner.SpawnUnitComputer(laneIndex, spawnableUnitIndex);
    }
}

public interface IUnitDestroy
{
    void UnitReachedBase();
}

public interface ISupplyChange
{
    void ResupplyCompleted();
}

public interface IComputerManager
{
    bool SpawnComputerUnit(int laneIndex, int spawnableUnitIndex);
}