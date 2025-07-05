using System;

[Serializable]
public class KillEntry
{
    public string userId;
    public int kills;
}

[Serializable]
public class KillLeaderboard
{
    public KillEntry[] entries;
}