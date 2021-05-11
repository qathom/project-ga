using System;

public enum PlayerEra
{
    PAST, PRESENT, FUTURE
}

public class PlayerEra2
{
    public static int PAST = 0;
    public static int PRESENT = 1;
    public static int FUTURE = 2;

    public static int Size = 3;

    public static int[] Values = new int[] { PAST, PRESENT, FUTURE };

    private PlayerEra2() { }

    public static string TagForEra(int era)
    {
        if (era == PAST)
        {
            return "Medieval";
        }
        else if (era == PRESENT)
        {
            return "Present";
        }
        else if (era == FUTURE)
        {
            return "Future";
        }

        return "";
    }
}
