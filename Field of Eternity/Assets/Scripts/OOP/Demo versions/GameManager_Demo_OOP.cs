using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Demo_OOP : MonoBehaviour, ISpawnManager, IPauseMenuManager, IComputerManagerDemo, IUnitManager
{
    private Spawner_Demo_OOP spawner;
    private UIManager_Demo_OOP uiManager;
    private ComputerManager_Demo_OOP computerManager;
    private SupplyManager supplyManager;
    private List<UnitAI> playerUnits;
    private List<UnitAI> computerUnits;
    private List<UnitAI> markedForRemoval;
    private Vector3 unitPosition;
    private float engageRange;

    private void Start()
    {
        Time.timeScale = 1f;

        spawner = GetComponent<Spawner_Demo_OOP>();
        uiManager = GetComponent<UIManager_Demo_OOP>();
        supplyManager = GetComponent<SupplyManager>();
        computerManager = GetComponent<ComputerManager_Demo_OOP>();

        playerUnits = new List<UnitAI>();
        computerUnits = new List<UnitAI>();
        markedForRemoval = new List<UnitAI>();

        CheckForMissingComponents();

        ResourceTracker.ResetValues();
        supplyManager.StartResupplyForBoth();
        uiManager.LoadElements();
        uiManager.SetPauseMenuManager(this);
        spawner.SetSpawnerManagerInterface(this);
        computerManager.SetComputerManagerInterface(this);

        InvokeRepeating("CheckForCombatEngagement", 3f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.TogglePauseMenu();
        }
    }

    private void CheckForMissingComponents()
    {
        if (spawner == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: Spawner");
        }
        if (uiManager == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: UIManager");
        }
        if (supplyManager == null)
        {
            Debug.LogError("GameManager.cs - Missing Component: SupplyManager");
        }
    }

    public void SpawnComputerUnit(int laneIndex, int spawnableUnitIndex)
    {
        spawner.SpawnUnitComputer(laneIndex, spawnableUnitIndex);
    }

    public void StopTrackingUnit(UnitAI unit)
    {
        playerUnits.Remove(unit);
    }

    public void SpawnUnitsAllLanes(int spawnUnitIndex)
    {
        for(int i = 0; i < 3; i++)
        {
            spawner.SpawnUnitPlayer(i, spawnUnitIndex);
            spawner.SpawnUnitComputer(i, spawnUnitIndex);
        }
    }

    public int GetSpawnableUnitsLength()
    {
        return spawner.GetComputerSpawnableUnitsLength();
    }

    public void UnitReachedBase() { }

    #region UnitListManagement

    public void AddPlayerUnit(UnitAI unit)
    {
        playerUnits.Add(unit);
    }

    public void AddPlayerUnits(UnitAI[] units)
    {
        for (int i = 0; i < units.Length; i++)
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
        for (int i = 0; i < units.Length; i++)
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
        if (playerUnits.Count > 0)
        {
            EngagePlayerUnits();
        }

        if (computerUnits.Count > 0)
        {
            EngageComputerUnits();
        }

        if (markedForRemoval.Count > 0)
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

            unitPosition = playerUnits[i].transform.position;
            engageRange = playerUnits[i].GetEngageRange();

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

            unitPosition = computerUnits[i].transform.position;
            engageRange = computerUnits[i].GetEngageRange();

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

public interface IComputerManagerDemo
{
    void SpawnUnitsAllLanes(int spawnUnitIndex);
    int GetSpawnableUnitsLength();
}