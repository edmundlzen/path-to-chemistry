using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
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
    private PlayerData()
    {
        Level = 1;
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
    [DataMember]
    public int Level { get; set; }
    [DataMember]
    public string Seat { get; set; }
    [DataMember]
    public List<string> Molecule { get; set; }
    [DataMember]
    public List<string> flaskElements { get; set; }
    [DataMember]
    public List<string> levelAvailable { get; set; }
    [DataMember]
    public Dictionary<string, string> slotItem { get; set; }
}
