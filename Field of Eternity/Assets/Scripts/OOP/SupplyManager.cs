using UnityEngine;
using UnityEngine.SceneManagement;

public class SupplyManager : MonoBehaviour
{
    [SerializeField]
    private int supplyAmount = 5;
    [SerializeField]
    private float supplyRate = 1f;

    private ISupplyChange supplyChange;

    public void StartResupplyForBoth()
    {
        InvokeRepeating("Resupply", 0f, 1f / supplyRate);
    }

    private void Resupply()
    {
        if(ResourceTracker.PlayerSupplyCurrent < ResourceTracker.PlayerSupplyMax)
        {
            ResourceTracker.PlayerSupplyCurrent += supplyAmount;
            ResourceTracker.PlayerSupplyCurrent = Mathf.Clamp(ResourceTracker.PlayerSupplyCurrent, 0, ResourceTracker.PlayerSupplyMax);
        }
        if(ResourceTracker.ComputerSupplyCurrent < ResourceTracker.ComputerSupplyMax)
        {
            ResourceTracker.ComputerSupplyCurrent += supplyAmount;
            ResourceTracker.ComputerSupplyCurrent = Mathf.Clamp(ResourceTracker.ComputerSupplyCurrent, 0, ResourceTracker.ComputerSupplyMax);
        }

        if(SceneManager.GetActiveScene().name.Equals("Playable"))
        {
            supplyChange.ResupplyCompleted();
        }
    }

    public void SetSupplyChangeInterface(ISupplyChange supplyChange)
    {
        this.supplyChange = supplyChange;
    }
}
