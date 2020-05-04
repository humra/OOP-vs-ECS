using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IUnitManager, ISupplyChange, IComputerManager, ISpawnManager, IPauseMenuManager
{ 
    private Spawner spawner;
    private UIManager uiManager;
    private SupplyManager supplyManager;
    private ComputerManager computerManager;
    private List<UnitAI> playerUnits;
    private List<UnitAI> computerUnits;
    private List<UnitAI> markedForRemoval;

    private void Start()
    {
        Time.timeScale = 1f;

        spawner = GetComponent<Spawner>();
        uiManager = GetComponent<UIManager>();
        supplyManager = GetComponent<SupplyManager>();
        computerManager = GetComponent<ComputerManager>();

        playerUnits = new List<UnitAI>();
        computerUnits = new List<UnitAI>();
        markedForRemoval = new List<UnitAI>();

        CheckForMissingComponents();

        ResourceTracker.ResetValues();
        supplyManager.StartResupplyForBoth();
        supplyManager.SetSupplyChangeInterface(this);
        uiManager.LoadElements();
        uiManager.UpdateUI();
        uiManager.SetPauseMenuManager(this);
        computerManager.SetComputerManagerInterface(this);
        computerManager.SetLaneSpawnableUnitsLength(spawner.GetLaneLength(), spawner.GetComputerSpawnableUnitsLength());
        computerManager.GenerateSpawns();
        spawner.SetSpawnerManagerInterface(this);

        InvokeRepeating("CheckForCombatEngagement", 3f, 1f);
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.TogglePauseMenu();
        }
    }

    private void AIWin()
    {
        uiManager.GameOver(false);
    }

    private void PlayerWin()
    {
        uiManager.GameOver(true);
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

    public void StopTrackingUnit(UnitAI unit)
    {
        playerUnits.Remove(unit);
    }

    #region UnitListManagement

    public void AddPlayerUnit(UnitAI unit)
    {
        playerUnits.Add(unit);
    }

    public void AddPlayerUnits(UnitAI[] units)
    {
        for(int i = 0; i < units.Length; i++)
        {
            playerUnits.Add(units[i]);
        }
    }

    public void AddComputerUnit(UnitAI unit)
    {
        computerUnits.Add(unit);
    }

    public void AddComputerUnits(UnitAI[] units)
    {
        for(int i = 0; i < units.Length; i++)
        {
            computerUnits.Add(units[i]);
        }
    }

    public void StopTrackingUnits(UnitAI unit)
    {
        markedForRemoval.Add(unit);
    }

    #endregion

    #region CombatEngagement

    private void CheckForCombatEngagement()
    {
        if(playerUnits.Count > 0)
        {
            EngagePlayerUnits();
        }

        if(computerUnits.Count > 0)
        {
            EngageComputerUnits();
        }

        if(markedForRemoval.Count > 0)
        {
            CleanMarkedForRemoval();
        }
    }

    private void EngagePlayerUnits()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i].inCombat || playerUnits[i] == null || !playerUnits[i].readyForCombat)
            {
                continue;
            }

            Vector3 unitPosition = playerUnits[i].transform.position;
            float engageRange = playerUnits[i].GetEngageRange();

            for (int j = 0; j < computerUnits.Count; j++)
            {
                if (computerUnits[j] == null)
                {
                    continue;
                }

                if ((Vector3.Distance(unitPosition, computerUnits[j].transform.position) <= engageRange))
                {
                    playerUnits[i].EngageTarget(computerUnits[j]);
                }
            }
        }
    }

    private void EngageComputerUnits()
    {
        for (int i = 0; i < computerUnits.Count; i++)
        {
            if (computerUnits[i].inCombat || computerUnits[i] == null || !computerUnits[i].readyForCombat)
            {
                continue;
            }

            Vector3 unitPosition = computerUnits[i].transform.position;
            float engageRange = computerUnits[i].GetEngageRange();

            for (int j = 0; j < playerUnits.Count; j++)
            {
                if (playerUnits[j] == null)
                {
                    continue;
                }

                if ((Vector3.Distance(unitPosition, playerUnits[j].transform.position) <= engageRange))
                {
                    computerUnits[i].EngageTarget(playerUnits[j]);
                }
            }
        }
    }

    private void CleanMarkedForRemoval()
    {
        for (int i = 0; i < markedForRemoval.Count; i++)
        {
            if (markedForRemoval[i].GetPlayerOwned())
            {
                playerUnits.Remove(markedForRemoval[i]);
            }
            else
            {
                computerUnits.Remove(markedForRemoval[i]);
            }
        }

        markedForRemoval.Clear();
    }

    #endregion

    #region PauseMenu

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}

#region Interfaces

public interface IUnitManager
{
    void UnitReachedBase();
    void StopTrackingUnit(UnitAI unit);
}

public interface ISupplyChange
{
    void ResupplyCompleted();
}

public interface IComputerManager
{
    bool SpawnComputerUnit(int laneIndex, int spawnableUnitIndex);
}

public interface ISpawnManager
{
    void AddPlayerUnit(UnitAI unit);
    void AddPlayerUnits(UnitAI[] units);
    void AddComputerUnit(UnitAI unit);
    void AddComputerUnits(UnitAI[] units);
}

public interface IPauseMenuManager
{
    void RestartGame();
    void QuitGame();
    void MainMenu();
}

#endregion