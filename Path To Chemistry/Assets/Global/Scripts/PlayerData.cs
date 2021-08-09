using System.Collections.Generic;

public class PlayerData
{
    private static readonly object threadlock = new object();
    private static PlayerData instance = null;
    public static PlayerData Instance()
    {
        lock (threadlock)
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }
    public void UpdatePlayerData(PlayerData playerData)
    {
        instance = playerData;
    }
    private PlayerData()
    {
        Level = 1;
        Counter = 0;
        Seat = "Main";
        Molecule = new List<string>();
        flaskElements = new List<string>();
        levelAvailable = new List<string>()
        {
            { $"Level {Level}" }
        };
        slotItem = new Dictionary<string, string>()
        {
            { "Slot1", "K" },
            { "Slot2", "H" },
            { "Slot3", "O" },
            { "Slot4", "H" },
            { "Slot5", "" },
            { "Slot6", "" },
            { "Slot7", "" },
            { "Slot8", "" },
            { "Slot9", "" }
        };
    }
    public int Level { get; set; }
    public int Counter { get; set; }
    public string Seat { get; set; }
    public List<string> Molecule { get; set; }
    public List<string> flaskElements { get; set; }
    public List<string> levelAvailable { get; set; }
    public Dictionary<string, string> slotItem { get; set; }
}