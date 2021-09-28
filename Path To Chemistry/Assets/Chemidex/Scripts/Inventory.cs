using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
            var playerData = PlayerData.Instance();
            var elementData = ElementData.Instance();   
            elementCheck();
            stateCheck();
            GameObject.Find("Coma").GetComponent<Animator>().SetTrigger("Wake");
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

    public void BackLab()
    {
        hotbar.hasLoaded = false;
        InventoryData.hasLoaded = false;
        StartCoroutine(sleepAnime(player.startPlace));
    }
    private IEnumerator sleepAnime(string Scene)
    {
        GameObject.Find("Sleep").GetComponent<Animator>().SetTrigger("Sleep");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(Scene);
    }

    public void History(GameObject HistoryUI)
    {
        if (!player.Pause)
        {
            player.Pause = true;
            HistoryUI.SetActive(true);
            GameObject.Find("Red Dot").GetComponent<Image>().color = Color.clear;
            if (player.History.Count > 0)
            {
                for (int i = 1; i <= player.History.Count; i++)
                {
                    if (GameObject.Find("Nothing") != null)
                    {
                        Destroy(GameObject.Find("Nothing"));
                    }
                    GameObject newAlert = Instantiate(Resources.Load<GameObject>($"Lab/Alertline"));
                    newAlert.name = $"Alert{i}";
                    newAlert.GetComponent<Text>().text = $"{i}. {player.History[i - 1]}";
                    newAlert.transform.SetParent(GameObject.Find("History").transform);
                }
            }
            else
            {
                GameObject newAlert = Instantiate(Resources.Load<GameObject>($"Lab/Alertline"));
                newAlert.name = $"Nothing";
                newAlert.GetComponent<Text>().text = "Nothing here ¯\\_(ツ)_/¯";
                newAlert.transform.SetParent(GameObject.Find("History").transform);
            }
        }
    }

    public void closeHistory(GameObject HistoryUI)
    {
        if (GameObject.Find("Nothing") != null)
        {
            Destroy(GameObject.Find("Nothing"));
        }
        for (int i = 1; i <= player.History.Count; i++)
        {
            Destroy(GameObject.Find($"Alert{i}"));
        }
        player.Pause = false;
        HistoryUI.SetActive(false);
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
    }

    public void returnQuantity(GameObject returnQuantityUI)
    {
        var playerData = PlayerData.Instance();
        if (!player.Pause)
        {
            if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
            {
                if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])))
                {
                    addAlert("Alert: Compounds are not addable to Inventory!");
                }
                else if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
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
        {
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
                GameObject.Find($"ItemName").GetComponent<Text>().text = "";
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
        }
    }

    private void slotDeepCheck(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"])))
        {
            if (GameObject.Find($"HotbarSlot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/H Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"HotbarSlot ({i})/Item").transform);
            }
            GameObject.Find($"HotbarSlot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.slotItem[$"Slot{i}"]["Element"]}");
        }
        else
        {
            Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
            GameObject.Find($"HotbarSlot ({i})/Symbol").GetComponent<Text>().text = $"{playerData.slotItem[$"Slot{i}"]["Element"]}";
        }
        if (GameObject.Find("ItemName") != null)
        {
            GameObject.Find("ItemName").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{Convert.ToInt32(hotbar.slotNum)}"]["Element"]);
        }
    }
    private void Load()
    {
        var elementData = JsonConvert.DeserializeObject<ElementData>(allElements.Data);
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
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    private void addAlert(string Alert)
    {
        int maxQuantity = 50;
        player.History.Add(Alert);
        GameObject.Find("Red Dot").GetComponent<Image>().color = Color.white;
        if (player.History.Count > maxQuantity)
        {
            player.History.RemoveAt(0);
        }
    }
}