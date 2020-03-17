public static class ResourceTracker
{
    public static int PlayerHealthMax = 1000;
    public static int ComputerHealthMax = 1000;
    public static int PlayerHealthCurrent;
    public static int ComputerHealthCurrent;
    public static int PlayerSupplyMax = 1000;
    public static int ComputerSupplyMax = 1000;
    public static int PlayerSupplyCurrent;
    public static int ComputerSupplyCurrent;
    public static int PlayerSupplyStart = 100;
    public static int ComputerSupplyStart = 100;
    public static int[] UnitSupplyCost = new int[] { 5, 15, 30, 50 };

    public static void ResetValues()
    {
        PlayerHealthCurrent = PlayerHealthMax;
        ComputerHealthCurrent = ComputerHealthMax;
        PlayerSupplyCurrent = PlayerSupplyStart;
        ComputerSupplyCurrent = ComputerSupplyStart;
    }
}
