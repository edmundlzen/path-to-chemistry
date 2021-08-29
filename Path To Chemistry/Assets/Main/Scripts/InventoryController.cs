using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private GameObject inventory;
    private GameObject materials;
    private GameObject hotbar;
    
    void Awake()
    {
        inventory = GameObject.Find("Inventory Items");
        materials = GameObject.Find("Materials Display").transform.Find("Materials").gameObject;
        hotbar = GameObject.Find("Hotbar").transform.Find("Hotbar Items").gameObject;
    }
    void OnEnable()
    {
        // Load();
        
        UpdateInventoryView();
        UpdateHotbarView();
        UpdateMaterialsView();
    }
    
    void Load()
    {
        string directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }

    void Save()
    {
        var playerData = PlayerData.Instance();
        string directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    
    public void InventoryClickHandler(GameObject slot)
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalHotbar = playerData.survivalHotbar;
        var item = survivalInventory[slot.name];

        if (!survivalHotbar.Contains(item["name"].ToString()) && survivalHotbar.Count >= 9)
        {
            playerData.survivalHotbar.RemoveAt(8);
            playerData.survivalHotbar.Insert(0, item["name"].ToString());
            UpdateHotbarView();
            Save();
        } else if (!survivalHotbar.Contains(item["name"].ToString()) && survivalHotbar.Count < 9)
        {
            playerData.survivalHotbar.Insert(0, item["name"].ToString());
            UpdateHotbarView();
            Save();
        }
    }
    
    public void HotbarClickHandler(GameObject slot)
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var item = survivalInventory[slot.name];
        print(slot.name);
    }
    
    void UpdateInventoryView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;

        foreach (var item in survivalInventory)
        {
            if (Int32.Parse(item.Value["quantity"].ToString()) <= 0)
            {
                continue;
            }
            foreach (Transform slot in inventory.transform)
            {
                Transform slotGraphics = slot.Find("Slot Components");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(item.Value["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = item.Value["quantity"].ToString();
                    slot.transform.name = item.Value["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void UpdateHotbarView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalHotbar = playerData.survivalHotbar;

        foreach (Transform slot in hotbar.transform)
        {
            slot.Find("Slot Components").gameObject.SetActive(false);
        }

        foreach (var reference in survivalHotbar)
        {
            var item = survivalInventory[reference];
            if (Int32.Parse(item["quantity"].ToString()) <= 0)
            {
                continue;
            }
            foreach (Transform slot in hotbar.transform)
            {
                Transform slotGraphics = slot.Find("Slot Components");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(item["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = item["quantity"].ToString();
                    slot.transform.name = item["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void UpdateMaterialsView()
    {
        var playerData = PlayerData.Instance();
        var survivalMaterials = playerData.survivalMaterials;

        foreach (var material in survivalMaterials)
        {
            foreach (Transform slot in materials.transform)
            {
                Transform slotGraphics = slot.Find("Material Graphics");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>(material.Value["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = "x " + material.Value["quantity"].ToString();
                    slot.transform.name = material.Value["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
