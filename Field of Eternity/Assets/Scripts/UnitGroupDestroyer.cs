using UnityEngine;

public class UnitGroupDestroyer : MonoBehaviour
{
    private int activeUnits = -1;

    public void SetActiveUnitsCount(int units)
    {
        activeUnits = units;

        if(activeUnits <= 0)
        {
            Debug.LogError("UnitGroupDestroyer - The number of units is 0 or less on " + gameObject.name);
        }
    }

    public void UnitDestroyed()
    {
        activeUnits--;

        if(activeUnits == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
