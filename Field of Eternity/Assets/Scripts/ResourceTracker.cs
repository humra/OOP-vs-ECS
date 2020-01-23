public static class ResourceTracker
{
    public static int PlayerHealthMax = 1000;
    public static int AIHealthMax = 1000;
    public static int PlayerHealthCurrent;
    public static int AIHealthCurrent;

    public static void ResetValues()
    {
        PlayerHealthCurrent = PlayerHealthMax;
        AIHealthCurrent = AIHealthMax;
    }
}
