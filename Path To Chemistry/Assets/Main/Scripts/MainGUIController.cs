using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class MainGUIController : MonoBehaviour
{
    private GameObject gameviewButtons;
    private GameObject gameviewHotbar;
    private GameObject inventory;
    private GameObject crafting;
    private GameObject smelting;
    private GameObject playerCrafting;
    private GameObject materialReducer;
    private GameObject teleport;
    private GameObject death;
    private GameObject pause;
    
    private string activeView;

    private string activeItem;
    
    public GameObject hand;
    public FirstPersonController player;
    public bool goBackToDeath;

    void Awake()
    {
        gameviewButtons = transform.Find("Gameview Buttons").gameObject;
        gameviewHotbar = gameviewButtons.transform.Find("Gameview Hotbar").Find("Gameview Hotbar Items").gameObject;
        inventory = transform.Find("Inventory").gameObject;
        crafting = transform.Find("Crafting").gameObject;
        smelting = transform.Find("Smelting").gameObject;
        playerCrafting = transform.Find("Player Crafting").gameObject;
        materialReducer = transform.Find("Material Reducer").gameObject;
        teleport = transform.Find("Teleport").gameObject;
        death = transform.Find("Death").gameObject;
        pause = transform.Find("Pause").gameObject;
    }
    
    void Start()
    {
        // Load();
        goBackToDeath = false;
        ChangeView("gameview");
    }

    // void Update()
    // {
    //     string keyPressed = Input.inputString;
    //
    //     switch (keyPressed)
    //     {
    //         case "z":
    //             ChangeView("gameview");
    //             break;
    //         case "x":
    //             ChangeView("inventory");
    //             break;
    //         case "c":
    //             ChangeView("crafting");
    //             break;
    //         case "v":
    //             ChangeView("smelting");
    //             break;
    //     }
    // }
    
    // private void Load()
    // {
    //     var directory = $"{Application.persistentDataPath}/Data";
    //     var filePath = Path.Combine(directory, "Saves.json");
    //     var fileContent = File.ReadAllText(filePath);
    //     var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
    //     PlayerData.Instance().UpdatePlayerData(playerData);
    // }
    //
    // private void Save()
    // {
    //     var playerData = PlayerData.Instance();
    //     var directory = $"{Application.persistentDataPath}/Data";
    //     if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
    //
    //     var Settings = new JsonSerializerSettings();
    //     Settings.Formatting = Formatting.Indented;
    //     Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //     var Json = JsonConvert.SerializeObject(playerData, Settings);
    //     var filePath = Path.Combine(directory, "Saves.json");
    //     File.WriteAllText(filePath, Json);
    // }

    public void ChangeView(string view)
    {
        player.freeze = true;
        activeView = view;
        
        // Save();
        
        gameviewButtons.SetActive(false);
        inventory.SetActive(false);
        crafting.SetActive(false);
        smelting.SetActive(false);
        playerCrafting.SetActive(false);
        materialReducer.SetActive(false);
        teleport.SetActive(false);
        death.SetActive(false);
        pause.SetActive(false);

        switch (view)
        {
            case "gameview":
                if (goBackToDeath)
                {
                    goBackToDeath = false;
                    ChangeView("death");
                    return;
                }
                player.freeze = false;
                // Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;
                gameviewButtons.SetActive(true);
                UpdateGameviewHotbar();
                break;
            case "inventory":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inventory.SetActive(true);
                break;
            case "crafting":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crafting.SetActive(true);
                break;
            case "smelting":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                smelting.SetActive(true);
                break;
            case "playerCrafting":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerCrafting.SetActive(true);
                break;
            case "materialReducer":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                materialReducer.SetActive(true);
                break;
            case "teleport":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                teleport.SetActive(true);
                break;
            case "death":
                goBackToDeath = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                death.SetActive(true);
                break;
            case "pause":
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pause.SetActive(true);
                break;
            default:
                Debug.LogError("Argument for changeView is invalid. It must be one of either: 'inventory', 'crafting', 'smelting'");
                break;
        }
    }

    public void ChangeActiveItem(int hotbarSlot)
    {
        var playerData = PlayerData.Instance();
        var survivalHotbar = playerData.survivalHotbar;
        var hotbarItem = survivalHotbar[hotbarSlot];
        switch (hotbarItem)
        {
            case null:
                break;
            default:
                activeItem = hotbarItem;
                SetHandItem(Resources.Load<GameObject>("Prefabs/" + hotbarItem));
                break;
        }
    }

    public void SetHandItem(GameObject item)
    {
        foreach (Transform existingItem in hand.transform)
        {
            Destroy(existingItem.gameObject);
        }

        var newHandItem = Instantiate(item, hand.transform, false);
        newHandItem.name = item.name;
        newHandItem.transform.rotation = hand.transform.rotation;
    }

    public string GetActiveItem()
    {
        return activeItem;
    }

    public void UpdateGameviewHotbar()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalHotbar = playerData.survivalHotbar;

        foreach (Transform slot in gameviewHotbar.transform) slot.Find("Slot Components").gameObject.SetActive(false);
        
        foreach (var reference in survivalHotbar)
        {
            if (reference == null) continue;
            var item = survivalInventory[reference];
            if (int.Parse(item["quantity"].ToString()) <= 0)
            {
                if (item["name"].ToString() == activeItem)
                {
                    Destroy(hand.transform.GetChild(0).gameObject);
                }

                continue;
            }
            foreach (Transform slot in gameviewHotbar.transform)
            {
                var slotGraphics = slot.Find("Slot Components");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite =
                        Resources.Load<Sprite>("Sprites/" + item["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = item["quantity"].ToString();
                    slot.transform.name = item["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void GameviewHotbarClickHandler(GameObject slot)
    {
        ChangeActiveItem(slot.transform.GetSiblingIndex());
    }
}
