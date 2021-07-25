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
        chemNum = "1";
        raycastObject = "";
        craftedMolecule = "";
        Molecule = new List<string>();
        flaskElements = new List<string>();
        levelAvailable = new List<string>()
        {
            { $"Level {Level}" }
        };
        moleculeCount = new Dictionary<string, int>();
        slotItem = new Dictionary<string, string>()
        {
            { "Slot1", "Hydrochloric Acid" },
            { "Slot2", "Ammonia" },
            { "Slot3", "Hydrogen Peroxide" },
            { "Slot4", "Sodium Iodide" },
            { "Slot5", "Water" },
            { "Slot6", "Water" },
            { "Slot7", "K" },
            { "Slot8", "" },
            { "Slot9", "Sodium Acetate" }
        };
        chemRecipes = new Dictionary<string, string>()
        {
            { "Recipe 1", "K + H2O" },
            { "Recipe 2", "HCl + NH3" },
            { "Recipe 3", "H2O2 + NaI" },
            { "Recipe 4", "C2H3NaO2 + H2O" },
            { "Recipe 5", "KI + H2O2 + C18H35NaO2" }
        };
    }
    [DataMember]
    public int Level { get; set; }
    [DataMember]
    public string Seat { get; set; }
    [DataMember]
    public string chemNum { get; set; }
    [DataMember]
    public string raycastObject { get; set; }
    [DataMember]
    public string craftedMolecule { get; set; }
    [DataMember]
    public List<string> Molecule { get; set; }
    [DataMember]
    public List<string> flaskElements { get; set; }
    [DataMember]
    public List<string> levelAvailable { get; set; }
    [DataMember]
    public Dictionary<string, int> moleculeCount { get; set; }
    [DataMember]
    public Dictionary<string, string> slotItem { get; set; }
    [DataMember]
    public Dictionary<string, string> chemRecipes { get; set; }
}
