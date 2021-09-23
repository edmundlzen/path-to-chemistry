using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class InventoryData
{
    public static bool hasDone = false;
    public static bool hasLoaded = false;
    public static string Slot = "Slot (1)";
}
public class Inventory : MonoBehaviour
{
    private void Start()
    {
        if (!InventoryData.hasLoaded)
        {
            Load();
            Load2();
            elementCheck();
            stateCheck();
            Alert();
            var playerData = PlayerData.Instance();
            GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
            GameObject.Find("Energy").GetComponent<Text>().text = $"Energy: {playerData.Energy}";
            InventoryData.hasLoaded = true;
        }
/*
        for (int i = 1; i <= 118; i++)
        {
            playerData.Inventory.Add(elementData.elements.Keys.ElementAt(i - 1), 0);
        }
        Save();
*/
    }
    
    public void Slider(float Value)
    {
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Math.Floor(Value));
    }

    public void maxQuantity()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = GameObject.Find("Slider").GetComponent<Slider>().maxValue;
    }

    public void addQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value -= 1;
        }
    }

    public void getQuantity(GameObject getQuantityUI)
    {
        var playerData = PlayerData.Instance();
        var slotNum = Convert.ToInt32(InventoryData.Slot.Replace("Slot (", "").Replace(")", ""));
        if (!player.Pause)
        {
            if (playerData.Inventory.Values.ElementAt(slotNum - 1) > 1)
            {
                player.Pause = true;
                getQuantityUI.SetActive(true);
                GameObject.Find("getSliderQuantity/Slider").GetComponent<Slider>().maxValue = playerData.Inventory.Values.ElementAt(slotNum - 1);
                GameObject.Find("getSliderQuantity/Slider").GetComponent<Slider>().value = 0;
            }
            else if (playerData.Inventory.Values.ElementAt(slotNum - 1) > 0)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Inventory.Keys.ElementAt(slotNum - 1))
                    {
                        var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                        if (Balance + 1 <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                            InventoryData.hasDone = true;
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (!InventoryData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Inventory.Keys.ElementAt(slotNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                            InventoryData.hasDone = true;
                            break;
                        }
                    }
                }
                if (!InventoryData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            return;
                        }
                    }
                }
                playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= 1;
                InventoryData.hasDone = false;
                stateCheck();
                slotCheck();
                Alert();
            }
        }
    }

    public void takeQuantity(GameObject getQuantityUI)
    {
        var playerData = PlayerData.Instance();
        var slotNum = Convert.ToInt32(InventoryData.Slot.Replace("Slot (", "").Replace(")", ""));
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("getSliderQuantity/Slider").GetComponent<Slider>().value));
        if (sliderValue > 0)
        {
            for (var i = 1; i <= 9; i = i + 1)
            {
                if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Inventory.Keys.ElementAt(slotNum - 1))
                {
                    var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                    if (Balance + sliderValue <= 64)
                    {
                        playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + sliderValue;
                        InventoryData.hasDone = true;
                        break;
                    }
                    else
                    {
                        playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + (64 - Balance);
                        playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= 64 - Balance;
                        getQuantityUI.SetActive(false);
                        chemInventory();
                        return;
                    }
                }
            }
            if (!InventoryData.hasDone)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                    {
                        if (sliderValue <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Inventory.Keys.ElementAt(slotNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                            InventoryData.hasDone = true;
                            break;
                        }
                        else
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Inventory.Keys.ElementAt(slotNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = 64;
                            playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= 64;
                            getQuantityUI.SetActive(false);
                            chemInventory();
                            return;
                        }
                    }
                }
            }
            if (!InventoryData.hasDone)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                    {
                        getQuantityUI.SetActive(false);
                        player.Pause = true;
                        return;
                    }
                }
            }
            playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= sliderValue;
        }
        getQuantityUI.SetActive(false);
        chemInventory();
    }

    private void chemInventory()
    {
        InventoryData.hasDone = false;
        player.Pause = false;
        stateCheck();
        slotCheck();
        Alert();
    }

    public void returnQuantity(GameObject returnQuantityUI)
    {
        var playerData = PlayerData.Instance();
        if (!player.Pause)
        {
            if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
            {
                if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
                {
                    player.Pause = true;
                    returnQuantityUI.SetActive(true);
                    GameObject.Find("returnSliderQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
                    GameObject.Find("returnSliderQuantity/Slider").GetComponent<Slider>().value = 0;
                }
                else if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 0)
                {
                    for (int i = 1; i <= 118; i++)
                    {
                        if (Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]) == playerData.Inventory.Keys.ElementAt(i - 1))
                        {
                            playerData.Inventory[playerData.Inventory.Keys.ElementAt(i - 1)] += 1;
                            GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.white;
                            InventoryData.Slot = $"Slot ({i})";
                            GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
                            break;
                        }
                    }
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                    stateCheck();
                    slotCheck();
                    Alert();
                }
            }
        }
    }

    public void putQuantity(GameObject returnQuantityUI)
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("returnSliderQuantity/Slider").GetComponent<Slider>().value));
        if (sliderValue > 0)
        {
            for (int i = 1; i <= 118; i++)
            {
                if (Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]) == playerData.Inventory.Keys.ElementAt(i - 1))
                {
                    playerData.Inventory[playerData.Inventory.Keys.ElementAt(i - 1)] += sliderValue;
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
                    GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.white;
                    InventoryData.Slot = $"Slot ({i})";
                    GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
                    break;
                }
            }
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
            {
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
            }
        }
        returnQuantityUI.SetActive(false);
        player.Pause = false;
        stateCheck();
        slotCheck();
        Alert();
    }

    public void sellQuantity(GameObject SellUI)
    {
        var playerData = PlayerData.Instance();
        var elementData = ElementData.Instance();
        var slotNum = Convert.ToInt32(InventoryData.Slot.Replace("Slot (", "").Replace(")", ""));
        if (!player.Pause)
        {
            if (playerData.Inventory.Values.ElementAt(slotNum - 1) > 1)
            {
                player.Pause = true;
                SellUI.SetActive(true);
                GameObject.Find("sellSliderQuantity/Slider").GetComponent<Slider>().maxValue = playerData.Inventory.Values.ElementAt(slotNum - 1);
                GameObject.Find("sellSliderQuantity/Slider").GetComponent<Slider>().value = 0;

            }
            else if (playerData.Inventory.Values.ElementAt(slotNum - 1) > 0)
            {
                for (int i = 1; i <= 94; i++)
                {
                    if (elementData.rarity[i - 1] == playerData.Inventory.Keys.ElementAt(slotNum - 1))
                    {
                        playerData.Energy += (i + 1) * 10;
                        break;
                    }
                    else
                    {
                        playerData.Energy += 10;
                        break;
                    }
                }
                if (playerData.Energy > 9999990)
                {
                    playerData.Energy = 9999990;
                }
                playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= 1;
                GameObject.Find("Energy").GetComponent<Text>().text = $"Energy: {playerData.Energy}";
                stateCheck();
                Alert();
            }
        }
    }

    public void sell(GameObject SellUI)
    {
        var playerData = PlayerData.Instance();
        var elementData = ElementData.Instance();
        var slotNum = Convert.ToInt32(InventoryData.Slot.Replace("Slot (", "").Replace(")", ""));
        var sellQuantity = Convert.ToInt32(Math.Floor(GameObject.Find("sellSliderQuantity/Slider").GetComponent<Slider>().value));
        if (sellQuantity > 0)
        {
            for (int i = 1; i <= 94; i++)
            {
                if (elementData.rarity[i - 1] == playerData.Inventory.Keys.ElementAt(slotNum - 1))
                {
                    playerData.Energy += sellQuantity * (i + 1) * 10;
                    break;
                }
                else
                {
                    playerData.Energy += sellQuantity * 10;
                    break;
                }
            }
            if (playerData.Energy > 9999990)
            {
                playerData.Energy = 9999990;
            }
            playerData.Inventory[playerData.Inventory.Keys.ElementAt(slotNum - 1)] -= sellQuantity;
        }
        GameObject.Find("Energy").GetComponent<Text>().text = $"Energy: {playerData.Energy}";
        SellUI.SetActive(false);
        player.Pause = false;
        stateCheck();
        Alert();
    }

    private void stateCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 118; i++)
        {
            if (InventoryData.Slot == $"Slot ({i})")
            {
                var Name = playerData.Inventory.Keys.ElementAt(i - 1);
                var Quantity = playerData.Inventory.Values.ElementAt(i - 1);
                GameObject.Find("State").GetComponent<Text>().text = $"Name: {Name}\nQuantity: {Quantity}";
                break;
            }
        }
    }

    public void elementCheck()
    {
        var elementData = ElementData.Instance();
        for (var i = 1; i <= 118; i++)
        {
            GameObject.Find($"Slot ({i})").transform.GetComponentInChildren<Text>().text = elementData.elements.Keys.ElementAt(i - 1);
        }
    }

    public void elementSymbol()
    {
        if (!player.Pause)
        {
            var playerData = PlayerData.Instance();
            GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.white;
            InventoryData.Slot = EventSystem.current.currentSelectedGameObject.name;
            GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
            stateCheck();
        }
    }

    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null &&
                Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                GameObject.Find($"HotbarSlot ({i})/Item").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                GameObject.Find($"HotbarSlot ({i})/Item").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                GameObject.Find($"HotbarSlot ({i})/Item").GetComponent<Text>().text = "";
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
    }

    private void Alert()
    {
        var playerData = PlayerData.Instance();
        var slotsUnavailable = new List<int>();
        for (var i = 1; i <= 9; i = i + 1)
        {
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
            {
                slotsUnavailable.Add(i);
            }
        }
        if (slotsUnavailable.Count == 9)
        {
            GameObject.Find("Alert").GetComponent<Text>().text = "Alert: Your hotbar slots are full!";
        }
        else
        {
            GameObject.Find("Alert").GetComponent<Text>().text = "";
        }
    }

    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }

    private void Load2()
    {
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }

    private void Save()
    {
        print(Application.persistentDataPath);
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            var Settings = new JsonSerializerSettings();
            Settings.Formatting = Formatting.Indented;
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var Json = JsonConvert.SerializeObject(playerData, Settings);
            var filePath = Path.Combine(directory, "Saves.json");
            File.WriteAllText(filePath, Json);
        }
    }
}