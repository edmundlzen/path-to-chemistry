using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private GameObject hotbar;
    private GameObject inventory;
    private GameObject materialsSlots;

    private void Awake()
    {
        inventory = GameObject.Find("Inventory Items");
        materialsSlots = GameObject.Find("Materials Display").transform.Find("Scroll View").GetChild(0).GetChild(0).gameObject;
        hotbar = GameObject.Find("Hotbar").transform.Find("Hotbar Items").gameObject;
    }

    private void OnEnable()
    {
        UpdateInventoryView();
        UpdateHotbarView();
        UpdateMaterialsView();
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
        }
        else if (!survivalHotbar.Contains(item["name"].ToString()) && survivalHotbar.Count < 9)
        {
            playerData.survivalHotbar.Insert(0, item["name"].ToString());
            UpdateHotbarView();
        }
    }

    public void HotbarClickHandler(GameObject slot)
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var item = survivalInventory[slot.name];
    }

    private void UpdateInventoryView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;

        foreach (Transform slot in inventory.transform)
        {
            var slotGraphics = slot.Find("Slot Components");
            slotGraphics.gameObject.SetActive(false);
        }
        
        foreach (var item in survivalInventory)
        {
            if (int.Parse(item.Value["quantity"].ToString()) <= 0) continue;
            foreach (Transform slot in inventory.transform)
            {
                var slotGraphics = slot.Find("Slot Components");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Sprites/" + item.Value["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = item.Value["quantity"].ToString();
                    slot.transform.name = item.Value["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void UpdateHotbarView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalHotbar = playerData.survivalHotbar;

        foreach (Transform slot in hotbar.transform) slot.Find("Slot Components").gameObject.SetActive(false);

        foreach (var reference in survivalHotbar)
        {
            if (reference == null) continue;
            var item = survivalInventory[reference];
            if (int.Parse(item["quantity"].ToString()) <= 0) continue;
            foreach (Transform slot in hotbar.transform)
            {
                var slotGraphics = slot.Find("Slot Components");
                if (!slotGraphics.gameObject.activeSelf)
                {
                    slotGraphics.Find("Image").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Sprites/" + item["image"].ToString());
                    slotGraphics.Find("Text").GetComponent<Text>().text = item["quantity"].ToString();
                    slot.transform.name = item["name"].ToString();
                    slotGraphics.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void UpdateMaterialsView()
    {
        var firstMaterialSlot = materialsSlots.transform.GetChild(0);
        firstMaterialSlot.gameObject.SetActive(false);
        foreach (Transform materialSlot in materialsSlots.transform)
        {
            if (materialSlot.GetSiblingIndex() != 0)
            {
                Destroy(materialSlot.gameObject);
            }
        }
        
        var playerData = PlayerData.Instance();
        var survivalMaterials = playerData.survivalMaterials;

        foreach (var material in survivalMaterials)
        {
            if (firstMaterialSlot.gameObject.activeSelf)
            {
                firstMaterialSlot.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + material.Value["image"].ToString());
                firstMaterialSlot.Find("Text").GetComponent<Text>().text = material.Value["quantity"].ToString();

                firstMaterialSlot.name = material.Key;
                firstMaterialSlot.gameObject.SetActive(true);
                continue;
            }

            Transform newMaterialSlot = Instantiate(firstMaterialSlot);
            newMaterialSlot.SetParent(materialsSlots.transform, false);
            
            newMaterialSlot.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + material.Value["image"].ToString());
            newMaterialSlot.Find("Text").GetComponent<Text>().text = "x " + material.Value["quantity"];
            
            newMaterialSlot.name = material.Key;
            newMaterialSlot.gameObject.SetActive(true);
        }
    }
}