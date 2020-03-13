public static class ResourceTracker
{
    public static int PlayerHealthMax = 1000;
    public static int ComputerHealthMax = 1000;
    public static int PlayerHealthCurrent;
    public static int ComputerHealthCurrent;

    public static void ResetValues()
    {
        PlayerHealthCurrent = PlayerHealthMax;
        ComputerHealthCurrent = ComputerHealthMax;
    }
}
