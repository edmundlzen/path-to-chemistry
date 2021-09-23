using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class hotbar
{
    public static bool hasLoaded = false;
    public static bool hasDone = false;
    public static string slotNum = "1";
    public static int flaskNum;
    public static int craftNum;
}

public class Hotbar : MonoBehaviour
{
    public GameObject flaskQuantity;
    public GameObject craftQuantity;
    private void Start()
    {
        if (!hotbar.hasLoaded)
        {
            Load();
            slotCheck();
            GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
            hotbar.hasLoaded = true;
        } 
    }

    public void hotbarSlot()
    {
        if (!player.Pause)
        {
            if ($"HotbarSlot ({hotbar.slotNum})" != EventSystem.current.currentSelectedGameObject.name)
            {
                GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.grey;
                hotbar.slotNum = EventSystem.current.currentSelectedGameObject.name.Replace("HotbarSlot (", "").Replace(")", "");
                GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
    }

    public void Slider(float Value)
    {
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Mathf.Floor(Value));
    }
    public void flaskButton()
    {
        var playerData = PlayerData.Instance();
        hotbar.flaskNum = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Replace("Slot", ""));
        if (hotbar.flaskNum <= playerData.flaskElements.Count)
        {
            if (playerData.flaskElements.Values.ElementAt(hotbar.flaskNum - 1) > 1)
            {
                flaskQuantity.SetActive(true);
                GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().maxValue = playerData.flaskElements.Values.ElementAt(hotbar.flaskNum - 1);
                GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value = 0;
            }
            else
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1))
                    {
                        if (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1 <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                            hotbar.hasDone = true;
                            break;
                        }
                        else
                        {
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            return;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                            hotbar.hasDone = true;
                            break;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            return;
                        }
                    }
                }
                hotbar.hasDone = false;
                playerData.flaskElements.Remove(playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1));
                slotCheck();
                flaskCheck();
            }
        }
    }
    public void maxFlask()
    {
        GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value = GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().maxValue;
    }
    public void addFlaskQuantity()
    {
        if (GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeFlaskQuantity()
    {
        if (GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value -= 1;
        }
    }
    public void getFlaskQuantity()
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("getFlaskQuantity/Slider").GetComponent<Slider>().value));
        flaskQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.flaskElements.Count >= hotbar.flaskNum)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1))
                    {
                        var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                        if (Balance + sliderValue <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + sliderValue;
                            hotbar.hasDone = true;
                            break;
                        }
                        else
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + (64 - Balance);
                            playerData.flaskElements[playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1)] -= 64 - Balance;
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            Flask();
                            return;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                            hotbar.hasDone = true;
                            break;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            flaskQuantity.SetActive(false);
                            return;
                        }
                    }
                }
                hotbar.hasDone = false;
                playerData.flaskElements[playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1)] -= sliderValue;
                Flask();
            }
        }
    }

    public void Flask()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Values.ElementAt(hotbar.flaskNum - 1) < 1)
        {
            playerData.flaskElements.Remove(playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1));
        }
        slotCheck();
        flaskCheck();
    }

    public void craftButton()
    {
        var playerData = PlayerData.Instance();
        hotbar.craftNum = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Replace("Slot", ""));
        if (hotbar.craftNum <= playerData.Molecule.Count)
        {
            if (playerData.Molecule.Values.ElementAt(hotbar.craftNum - 1) > 1)
            {
                craftQuantity.SetActive(true);
                GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().maxValue = playerData.Molecule.Values.ElementAt(hotbar.craftNum - 1);
                GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value = 0;
            }
            else
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1))
                    {
                        if (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1 <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                            hotbar.hasDone = true;
                            break;
                        }
                        else
                        {
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            return;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                            hotbar.hasDone = true;
                            break;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            return;
                        }
                    }
                }
                hotbar.hasDone = false;
                playerData.Molecule.Remove(playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1));
                slotCheck();
                craftingTable();
            }
        }
    }
    public void maxCraft()
    {
        GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value = GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().maxValue;
    }
    public void addCraftQuantity()
    {
        if (GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeCraftQuantity()
    {
        if (GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value -= 1;
        }
    }
    public void getCraftQuantity()
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("getCraftQuantity/Slider").GetComponent<Slider>().value));
        craftQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.Molecule.Count >= hotbar.craftNum)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1))
                    {
                        var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                        if (Balance + sliderValue <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + sliderValue;
                            hotbar.hasDone = true;
                            break;
                        }
                        else
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + (64 - Balance);
                            playerData.Molecule[playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1)] -= 64 - Balance;
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            Workbench();
                            return;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                            hotbar.hasDone = true;
                            break;
                        }
                    }
                }
                if (!hotbar.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            craftQuantity.SetActive(false);
                            return;
                        }
                    }
                }
                hotbar.hasDone = false;
                playerData.Molecule[playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1)] -= sliderValue;
                Workbench();
            }
        }
    }

    private void Workbench()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Molecule.Values.ElementAt(hotbar.craftNum - 1) < 1)
        {
            playerData.Molecule.Remove(playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1));
        }
        slotCheck();
        craftingTable();
    }
    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
        {
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
    }

    private void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = "";
            GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text = "";
        }
        for (var i = 1; i <= playerData.flaskElements.Count; i = i + 1)
        {
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text =
                    playerData.flaskElements.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text = "";
            }
        }
    }

    private void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = "";
            GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text = "";
        }

        for (var i = 1; i <= playerData.Molecule.Count; i = i + 1)
        {
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text =
                    playerData.Molecule.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text = "";
            }
        }
    }
    private void Load()
    {
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }

    private void addAlert(string Alert)
    {
        int maxQuantity = 50;
        player.History.Add(Alert);
        if (player.History.Count > maxQuantity)
        {
            player.History.RemoveAt(0);
        }
    }
}