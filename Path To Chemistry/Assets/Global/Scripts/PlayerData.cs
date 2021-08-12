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
        Seat = "Main";
        levelAvailable = new List<string>()
        {
            { $"Level {Level}" }
        };
        Molecule = new Dictionary<string, int>();
        flaskElements = new Dictionary<string, int>();
        slotItem = new Dictionary<string, Dictionary<string, object>>()
        {
            { "Slot1", new Dictionary<string, object>()
                {
                    { "Element", "K"},
                    { "Quantity", 1 }
                }
            },
            { "Slot2", new Dictionary<string, object>()
                {
                    { "Element", "H" },
                    { "Quantity", 1 }
                }
            },
            { "Slot3", new Dictionary<string, object>()
                {
                    { "Element", "O" },
                    { "Quantity", 1 }
                }
            },
            { "Slot4", new Dictionary<string, object>()
                {
                    { "Element", "H" },
                    { "Quantity", 1 }
                }
            },
            { "Slot5", new Dictionary<string, object>()
                {
                    { "Element", null },
                    { "Quantity", null }
                }
            },
            { "Slot6", new Dictionary<string, object>()
                {
                    { "Element", null },
                    { "Quantity", null }
                }
            },
            { "Slot7", new Dictionary<string, object>()
                {
                    { "Element", null },
                    { "Quantity", null }
                }
            },
            { "Slot8", new Dictionary<string, object>()
                {
                    { "Element", null },
                    { "Quantity", null }
                }
            },
            { "Slot9", new Dictionary<string, object>()
                {
                    { "Element", null },
                    { "Quantity", null }
                }
            },
        };
    }
    public int Level { get; set; }
    public int Counter { get; set; }
    public string Seat { get; set; }
    public List<string> levelAvailable { get; set; }
    public Dictionary<string, int> Molecule { get; set; }
    public Dictionary<string, int> flaskElements { get; set; }
    public Dictionary<string, Dictionary<string, object>> slotItem { get; set; }
}