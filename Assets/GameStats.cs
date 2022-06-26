public class GameStats
{
    public bool gameWon;
    public string winPlayedRatio;

    public int unitsBuilt;
    public int unitsLost;
    public int unitsKilled;

    public int buildingsConstructed;
    public int buildingsLost;
    public int buildingsDestroyed;

    public float resourcesMined;
    public float resourcesSpent;

    public float timePlayed;
    public string timePlayedFormat;

    public GameStats()
    {
        gameWon = false;
        winPlayedRatio = "";
        unitsBuilt = 0;
        unitsLost = 0;
        unitsKilled = 0;
        buildingsConstructed = 0;
        buildingsLost = 0;
        buildingsDestroyed = 0;
        resourcesMined = 0;
        resourcesSpent = 0;
        timePlayed = 0;
        timePlayedFormat = "";
    }
}
