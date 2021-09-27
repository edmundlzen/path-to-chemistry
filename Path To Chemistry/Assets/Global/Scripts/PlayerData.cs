using System.Collections.Generic;

public class PlayerData
{
    private static readonly object threadlock = new object();
    private static PlayerData instance;

    private PlayerData()
    {
        Level = 1;
        Energy = 100000;
        Experience = 69;
        Nickname = "You Are Gay";
        ID = null;
        Seat = "Main";
        levelAvailable = new List<string>
        {
            $"Level {Level}"
        };
        Molecule = new Dictionary<string, int>()
        {
            { "Og", 5 },
            { "C", 5 },
            { "N", 5 },
            { "F", 5 },
            { "Ne", 5 },
            { "Ar", 5 },
            { "Kr", 5 },
            { "Xe", 5 },
            { "Si", 5 },
        };
        Inventory = new Dictionary<string, int>();
        flaskElements = new Dictionary<string, int>();
        nonResellable = new Dictionary<string, bool>()
        {
            { "Classic Lab", true },
            { "Industry Lab", true },
            { "Home Lab", false },
            { "AR Lab", false }
        };
        Shop = new Dictionary<string, Dictionary<string, int>>()
        {
            {
                "Items", new Dictionary<string, int>()
                {
                    { "Health Potion", 1000 }
                }
            },
            {
                "Materials 1", new Dictionary<string, int>()
                {
                    {"Gravel", 1000},
                    {"Obsidian", 1000},
                    {"Sand", 1000},
                    {"Sandstone", 1000},
                    {"Stone", 1000},
                    {"Quartz", 1000}
                }
            },
            {
                "Materials 2", new Dictionary<string, int>()
                {
                    {"Grass", 1000},
                    {"Dirt", 1000},
                    {"Log", 1000},
                    {"Lava", 1000},
                    {"Cobblestone", 1000},
                    {"Mossy Cobblestone", 1000}
                }
            },
            {
                "Materials 3", new Dictionary<string, int>()
                {
                    {"Water", 1000},
                    {"Ice", 1000},
                    {"Packed Ice", 1000},
                    {"Snow", 1000},
                    {"Iron Ore", 1000},
                    {"Gold Ore", 1000},
                }
            },
            {
                "Materials 4", new Dictionary<string, int>()
                {
                    {"Diamond Ore", 1000},
                    {"Lapis Lazuli Ore", 1000},
                    {"Coal Ore", 1000},
                    {"Emerald Ore", 1000},
                    {"Clay", 1000},
                    {"Terracotta", 1000},
                }
            },
            {
                "Materials 5", new Dictionary<string, int>()
                {
                    {"Sugar", 1000},
                    {"Charcoal", 1000},
                    {"Ink Sac", 1000},
                }
            },
            {
                "Lab Maps", new Dictionary<string, int>()
                {
                    {"Industry Lab", 10000},
                    {"Home Lab", 10000},
                    {"AR Lab", 10000}
                }
            }
        };
        // TODO: Replace these with real values.
        survivalInventory = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Pistol", new Dictionary<string, object>
                {
                    {"image", "pistol"},
                    {"name", "Pistol"},
                    {"quantity", 1}
                }
            },
            {
                "Laser Collector", new Dictionary<string, object>
                {
                    {"image", "lasergun"},
                    {"name", "Laser Collector"},
                    {"quantity", 1}
                }
            },
            {
                "Flamethrower", new Dictionary<string, object>
                {
                    {"image", "flamethrower"},
                    {"name", "Flamethrower"},
                    {"quantity", 1}
                }
            },
            {
                "Teleporter", new Dictionary<string, object>
                {
                    {"image", "teleporter"},
                    {"name", "Teleporter"},
                    {"quantity", 13}
                }
            },
            {
                "Item 6", new Dictionary<string, object> // Crafting table
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 6"},
                    {"quantity", 45}
                }
            },
            {
                "Item 7", new Dictionary<string, object> // Furnace
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 7"},
                    {"quantity", 82}
                }
            },
        };
        survivalHotbar = new List<string>()
        {
        };
        survivalMaterials = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Cane", new Dictionary<string, object>
                {
                    {"image", "cane"},
                    {"name", "Cane"},
                    {"quantity", 10}
                }
            },
            {
                "Clay", new Dictionary<string, object>
                {
                    {"image", "clay"},
                    {"name", "Clay"},
                    {"quantity", 987}
                }
            },
            {
                "Coal", new Dictionary<string, object>
                {
                    {"image", "coal"},
                    {"name", "Coal"},
                    {"quantity", 234}
                }
            },
            {
                "Crystal", new Dictionary<string, object>
                {
                    {"image", "crystal"},
                    {"name", "Crystal"},
                    {"quantity", 463}
                }
            },
            {
                "Flint", new Dictionary<string, object>
                {
                    {"image", "flint"},
                    {"name", "Flint"},
                    {"quantity", 256}
                }
            },
            {
                "Gold", new Dictionary<string, object>
                {
                    {"image", "gold"},
                    {"name", "Gold"},
                    {"quantity", 256}
                }
            },
            {
                "Metal", new Dictionary<string, object>
                {
                    {"image", "metal"},
                    {"name", "Metal"},
                    {"quantity", 234}
                }
            },
            {
                "Sand", new Dictionary<string, object>
                {
                    {"image", "sand"},
                    {"name", "Sand"},
                    {"quantity", 983}
                }
            },
            {
                "Sand Rock", new Dictionary<string, object>
                {
                    {"image", "sandrock"},
                    {"name", "Sand Rock"},
                    {"quantity", 746}
                }
            },
            {
                "Stone", new Dictionary<string, object>
                {
                    {"image", "stone"},
                    {"name", "Stone"},
                    {"quantity", 237}
                }
            },
            {
                "Wood", new Dictionary<string, object>
                {
                    {"image", "wood"},
                    {"name", "Wood"},
                    {"quantity", 735}
                }
            },
            {
                "Coal Ore", new Dictionary<string, object>
                {
                    {"image", "coalore"},
                    {"name", "Coal Ore"},
                    {"quantity", 735}
                }
            },
            {
                "Diamond Ore", new Dictionary<string, object>
                {
                    {"image", "diamondore"},
                    {"name", "Diamond Ore"},
                    {"quantity", 500}
                }
            },
            {
                "Emerald Ore", new Dictionary<string, object>
                {
                    {"image", "emeraldore"},
                    {"name", "Emerald Ore"},
                    {"quantity", 735}
                }
            },
            {
                "Gold Ore", new Dictionary<string, object>
                {
                    {"image", "goldore"},
                    {"name", "Gold Ore"},
                    {"quantity", 735}
                }
            },
            {
                "Iron Ore", new Dictionary<string, object>
                {
                    {"image", "ironore"},
                    {"name", "Iron Ore"},
                    {"quantity", 735}
                }
            },
            {
                "Lapis Lazuli Ore", new Dictionary<string, object>
                {
                    {"image", "lapislazuliore"},
                    {"name", "Lapis Lazuli Ore"},
                    {"quantity", 735}
                }
            },
        };
        survivalRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 1"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 7},
                            {"Sand", 28} 
                        }
                    }
                }
            },
            {
                "Water Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 2"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 927},
                            {"Cane", 26},
                            {"Stone", 76}
                        }
                    }
                }
            }
        };
        survivalPlayerRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 1"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 7},
                            {"Sand", 28} 
                        }
                    }
                }
            },
            {
                "Water Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 2"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 927},
                            {"Cane", 26},
                            {"Stone", 76}
                        }
                    }
                }
            }
        };
        survivalSmeltingRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 1"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 7},
                            {"Sand", 28} 
                        }
                    }
                }
            },
            {
                "Water Bucket", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 2"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Metal", 927},
                            {"Cane", 26},
                            {"Stone", 76}
                        }
                    }
                }
            }
        };
        materialReducerRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Coal", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Coal"},
                    {
                        "elements", new Dictionary<string, int>
                        {
                            {"He", 927},
                            {"Cl", 26},
                            {"Li", 76}
                        }
                    }
                }
                },
            {
                "Crystal", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Crystal"},
                    {
                        "elements", new Dictionary<string, int>
                        {
                            {"H", 7},
                            {"O", 28} 
                        }
                    }
                }
            },
            {
                "Cane", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Cane"},
                    {
                        "elements", new Dictionary<string, int>
                        {
                            {"P", 927},
                            {"Ti", 26},
                            {"Al", 76}
                        }
                    }
                }
            }
        };
        teleports = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Classic Lab", new Dictionary<string, object>
                {
                    {"description", "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maxime mollitia,\nmolestiae quas vel sint commodi repudiandae consequuntur voluptatum laborum\nnumquam blanditiis harum quisquam eius sed odit fugiat iusto fuga praesentium\noptio, eaque rerum! Provident similique accusantium nemo autem. Veritatis\nobcaecati tenetur iure eiu"},
                    {"image", "Classic Lab"},
                    {"scene", "Classic Lab"}
                }
            },
            {
                "Industry Lab", new Dictionary<string, object>
                {
                    {"description", "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maxime mollitia,\nmolestiae quas vel sint commodi repudiandae consequuntur voluptatum laborum\nnumquam blanditiis harum quisquam eius sed odit fugiat iusto fuga praesentium\noptio, eaque rerum! Provident similique accusantium nemo autem. Veritatis\nobcaecati tenetur iure eiu"},
                    {"image", "Industry Lab"},
                    {"scene", "Industry Lab"}
                }
            },
            {
                "Home Lab", new Dictionary<string, object>
                {
                    {"description", "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maxime mollitia,\nmolestiae quas vel sint commodi repudiandae consequuntur voluptatum laborum\nnumquam blanditiis harum quisquam eius sed odit fugiat iusto fuga praesentium\noptio, eaque rerum! Provident similique accusantium nemo autem. Veritatis\nobcaecati tenetur iure eiu"},
                    {"image", "Home Lab"},
                    {"scene", "3D Home"}
                }
            },
            {
                "AR Lab", new Dictionary<string, object>
                {
                    {"description", "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maxime mollitia,\nmolestiae quas vel sint commodi repudiandae consequuntur voluptatum laborum\nnumquam blanditiis harum quisquam eius sed odit fugiat iusto fuga praesentium\noptio, eaque rerum! Provident similique accusantium nemo autem. Veritatis\nobcaecati tenetur iure eiu"},
                    {"image", "Placeholder"},
                    {"scene", "AR Lab"}
                }
            }
        };
        survivalHealth = 100; // Normal full value -> 100
        slotItem = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Slot1", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot2", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot3", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot4", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot5", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot6", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot7", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot8", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            },
            {
                "Slot9", new Dictionary<string, object>
                {
                    {"Element", "H2O"},
                    {"Quantity", 5}
                }
            }
        };
    }

    public int Level { get; set; }
    public int Counter { get; set; }
    public int Energy { get; set; }
    public int Experience { get; set; }
    public string Nickname { get; set; }
    public string ID { get; set; }
    public string Seat { get; set; }
    public List<string> levelAvailable { get; set; }
    public Dictionary<string, int> Molecule { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public Dictionary<string, int> flaskElements { get; set; }
    public Dictionary<string, bool> nonResellable { get; set; }
    public Dictionary<string, Dictionary<string, int>> Shop { get; set; }
    public Dictionary<string, Dictionary<string, object>> slotItem { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalInventory { get; set; }
    public List<string> survivalHotbar { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalMaterials { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalRecipes { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalPlayerRecipes { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalSmeltingRecipes { get; set; }
    public Dictionary<string, Dictionary<string, object>> materialReducerRecipes { get; set; }
    public Dictionary<string, Dictionary<string, object>> teleports { get; set; }
    public int survivalHealth { get; set; }

    public static PlayerData Instance()
    {
        lock (threadlock)
        {
            if (instance == null) instance = new PlayerData();
            return instance;
        }
    }

    public void UpdatePlayerData(PlayerData playerData)
    {
        instance = playerData;
    }
}

public class ElementData
{
    private static readonly object threadlock = new object();
    private static ElementData instance;

    public ElementData()
    {
        rarity = new List<string>();
        elements = new Dictionary<string, Dictionary<string, string>>();
    }

    public List<string> rarity { get; set; }
    public Dictionary<string, Dictionary<string, string>> elements { get; set; }

    public static ElementData Instance()
    {
        lock (threadlock)
        {
            if (instance == null) instance = new ElementData();
            return instance;
        }
    }

    public void UpdateElementData(ElementData elementData)
    {
        instance = elementData;
    }
}