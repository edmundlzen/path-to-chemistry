using System.Collections.Generic;

public class PlayerData
{
    private static readonly object threadlock = new object();
    private static PlayerData instance;

    private PlayerData()
    {
        Level = 1;
        Seat = "Main";
        levelAvailable = new List<string>
        {
            $"Level {Level}"
        };
        Molecule = new Dictionary<string, int>();
        Inventory = new Dictionary<string, int>();
        flaskElements = new Dictionary<string, int>();
        // TODO: Replace these with real values.
        survivalInventory = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Item 1", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 1"},
                    {"quantity", 7}
                }
            },
            {
                "Item 2", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 2"},
                    {"quantity", 8}
                }
            },
            {
                "Item 3", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 3"},
                    {"quantity", 4}
                }
            },
            {
                "Item 4", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 4"},
                    {"quantity", 5}
                }
            },
            {
                "Item 5", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 5"},
                    {"quantity", 13}
                }
            },
            {
                "Item 6", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 6"},
                    {"quantity", 45}
                }
            },
            {
                "Item 7", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 7"},
                    {"quantity", 82}
                }
            },
            {
                "Item 8", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 8"},
                    {"quantity", 77}
                }
            },
            {
                "Item 9", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 9"},
                    {"quantity", 25}
                }
            },
            {
                "Item 10", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 10"},
                    {"quantity", 98}
                }
            },
            {
                "Item 11", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 11"},
                    {"quantity", 86}
                }
            },
            {
                "Item 12", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Item 12"},
                    {"quantity", 54}
                }
            }
        };
        // TODO: Initialize this with 9 nulls to represent an empty hotbar.
        survivalHotbar = new List<string>();
        survivalMaterials = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Material 1", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Material 1"},
                    {"quantity", 54}
                }
            },
            {
                "Material 2", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Material 2"},
                    {"quantity", 987}
                }
            },
            {
                "Material 3", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Material 3"},
                    {"quantity", 234}
                }
            },
            {
                "Material 4", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Material 4"},
                    {"quantity", 463}
                }
            },
            {
                "Material 5", new Dictionary<string, object>
                {
                    {"image", "placeholder_item"},
                    {"name", "Material 5"},
                    {"quantity", 256}
                }
            }
        };
        survivalRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Item 1", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 1"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28} 
                        }
                    }
                }
            },
            {
                "Item 2", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 2"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    }
                }
            },
            {
                "Item 3", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 3"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    }
                }
            },
            {
                "Item 4", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 4"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    }
                }
            },
            {
                "Item 5", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 5"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    }
                }
            },
            {
                "Item 6", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 6"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    }
                }
            },
            {
                "Item 7", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 7"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    }
                }
            },
            {
                "Item 8", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 8"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    }
                }
            },
            {
                "Item 9", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 9"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    }
                }
            },
            {
                "Item 10", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 10"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    }
                }
            },
            {
                "Item 11", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 11"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    }
                }
            },
            {
                "Item 12", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 12"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    }
                }
            }
        };
        survivalSmeltingRecipes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Item 1", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 1"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28} 
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 2", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 2"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    },
                    {"time", 5}
                }
            },
            {
                "Item 3", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 3"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    },
                    {"time", 2}
                }
            },
            {
                "Item 4", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 4"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    },
                    {"time", 6}
                }
            },
            {
                "Item 5", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 5"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 6", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 6"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 7", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 7"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 8", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 8"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 9", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 9"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 10", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 10"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 7},
                            {"Material 3", 28}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 11", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 11"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 3", 927},
                            {"Material 4", 26},
                            {"Material 5", 76}
                        }
                    },
                    {"time", 3}
                }
            },
            {
                "Item 12", new Dictionary<string, object>
                {
                    {"enabled", true},
                    {"name", "Item 12"},
                    {
                        "materials", new Dictionary<string, int>
                        {
                            {"Material 1", 76},
                            {"Material 2", 23},
                            {"Material 5", 98}
                        }
                    },
                    {"time", 3}
                }
            }
        };
            slotItem = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Slot1", new Dictionary<string, object>
                {
                    {"Element", "K"},
                    {"Quantity", 7}
                }
            },
            {
                "Slot2", new Dictionary<string, object>
                {
                    {"Element", "H"},
                    {"Quantity", 1}
                }
            },
            {
                "Slot3", new Dictionary<string, object>
                {
                    {"Element", "O"},
                    {"Quantity", 1}
                }
            },
            {
                "Slot4", new Dictionary<string, object>
                {
                    {"Element", "H"},
                    {"Quantity", 1}
                }
            },
            {
                "Slot5", new Dictionary<string, object>
                {
                    {"Element", null},
                    {"Quantity", null}
                }
            },
            {
                "Slot6", new Dictionary<string, object>
                {
                    {"Element", null},
                    {"Quantity", null}
                }
            },
            {
                "Slot7", new Dictionary<string, object>
                {
                    {"Element", null},
                    {"Quantity", null}
                }
            },
            {
                "Slot8", new Dictionary<string, object>
                {
                    {"Element", null},
                    {"Quantity", null}
                }
            },
            {
                "Slot9", new Dictionary<string, object>
                {
                    {"Element", null},
                    {"Quantity", null}
                }
            }
        };
    }

    public int Level { get; set; }
    public int Counter { get; set; }
    public string Seat { get; set; }
    public List<string> levelAvailable { get; set; }
    public Dictionary<string, int> Molecule { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public Dictionary<string, int> flaskElements { get; set; }
    public Dictionary<string, Dictionary<string, object>> slotItem { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalInventory { get; set; }
    public List<string> survivalHotbar { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalMaterials { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalRecipes { get; set; }
    public Dictionary<string, Dictionary<string, object>> survivalSmeltingRecipes { get; set; }

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
        elements = new Dictionary<string, Dictionary<string, string>>();
    }

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