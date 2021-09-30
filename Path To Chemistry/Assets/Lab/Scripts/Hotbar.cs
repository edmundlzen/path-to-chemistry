using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public static class hotbar
{
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
        slotCheck();
        var playerData = PlayerData.Instance();
        string Scene = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        if (Scene.Split('/')[3].Replace(".unity", "") != "Inventory")
        {
            if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null)
            {
                GameObject.Find("CurrentItem/ItemName").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]);
            }
            else
            {
                GameObject.Find("CurrentItem/ItemName").GetComponent<Text>().text = "Empty";
            }
            GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])))
            {
                GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.red;
            }
            else
            {
                GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
    }

    private IEnumerator autoSave()
    {
        player.isSaving = true;
        yield return new WaitForSeconds(1);
        player.isSaving = false;
    }

    public void hotbarSlot()
    {
        var playerData = PlayerData.Instance();
        string Scene = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        if (!player.Pause && !player.deepPause)
        {
            if ($"HotbarSlot ({hotbar.slotNum})" != EventSystem.current.currentSelectedGameObject.name)
            {
                GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.grey;
                hotbar.slotNum = EventSystem.current.currentSelectedGameObject.name.Replace("HotbarSlot (", "").Replace(")", "");
                if (Scene.Split('/')[3].Replace(".unity", "") != "Inventory")
                {
                    if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null)
                    {
                        GameObject.Find("CurrentItem/ItemName").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]);
                    }
                    else
                    {
                        GameObject.Find("CurrentItem/ItemName").GetComponent<Text>().text = "Empty";
                    }
                    GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
                }
                else
                {
                    if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])))
                    {
                        GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.red;
                    }
                    else
                    {
                        GameObject.Find($"HotbarSlot ({hotbar.slotNum})").GetComponent<Image>().color = Color.cyan;
                    }
                }
                slotCheck();
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
        if (!player.deepPause)
        {
            hotbar.flaskNum = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Replace("Slot (", "").Replace(")", ""));
            if (hotbar.flaskNum <= playerData.flaskElements.Count)
            {
                if (playerData.flaskElements.Values.ElementAt(hotbar.flaskNum - 1) > 1)
                {
                    flaskQuantity.SetActive(true);
                    player.deepPause = true;
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
        player.deepPause = false;
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
                            if (sliderValue <= 64)
                            {
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                hotbar.hasDone = true;
                                break;
                            }
                            else
                            {
                                sliderValue = 64;
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.flaskElements.Keys.ElementAt(hotbar.flaskNum - 1);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                                hotbar.hasDone = true;
                                break;
                            }
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
        if (!player.deepPause)
        {
            hotbar.craftNum = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Replace("Slot (", "").Replace(")", ""));
            if (hotbar.craftNum <= playerData.Molecule.Count)
            {
                if (playerData.Molecule.Values.ElementAt(hotbar.craftNum - 1) > 1)
                {
                    craftQuantity.SetActive(true);
                    player.deepPause = true;
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
        player.deepPause = false;
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
                            if (sliderValue <= 64)
                            {
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                hotbar.hasDone = true;
                                break;
                            }
                            else
                            {
                                sliderValue = 64;
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Molecule.Keys.ElementAt(hotbar.craftNum - 1);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                                hotbar.hasDone = true;
                                break;
                            }
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
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{i}"]["Quantity"]);
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
                GameObject.Find($"HotbarSlot ({i})/Symbol").GetComponent<Text>().text = "";
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
    }

    private void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i++)
        {
            GameObject.Find($"flaskHotbar/Slot ({i})/Symbol").GetComponent<Text>().text = "";
            GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = "";
            if (playerData.flaskElements.Count < i)
            {
                Destroy(GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image"));
            }
        }
        for (var i = 1; i <= playerData.flaskElements.Count; i++)
        {
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                flaskDeepCheck(i);
                GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = Convert.ToString(playerData.flaskElements.Values.ElementAt(i - 1));
            }
            else
            {
                flaskDeepCheck(i);
                GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = "";
            }
        }
    }

    private void flaskDeepCheck(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(playerData.flaskElements.Keys.ElementAt(i - 1)))
        {
            if (GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/F Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"flaskHotbar/Slot ({i})/Item").transform);
            }
            GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.flaskElements.Keys.ElementAt(i - 1)}");
        }
        else
        {
            Destroy(GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image"));
            GameObject.Find($"flaskHotbar/Slot ({i})/Symbol").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
        }
    }

    private void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i++)
        {
            GameObject.Find($"craftTable/Slot ({i})/Element").GetComponent<Text>().text = "";
            GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = "";
            if (playerData.Molecule.Count < i)
            {
                Destroy(GameObject.Find($"craftTable/Slot ({i})/Item/Image"));
            }
        }
        for (var i = 1; i <= playerData.Molecule.Count; i++)
        {
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                deepCraftingTable(i);
                GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = Convert.ToString(playerData.Molecule.Values.ElementAt(i - 1));
            }
            else
            {
                deepCraftingTable(i);
                GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = "";
            }
        }
    }

    private void deepCraftingTable(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(playerData.Molecule.Keys.ElementAt(i - 1)))
        {
            if (GameObject.Find($"craftTable/Slot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/C Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"craftTable/Slot ({i})/Item").transform);
            }
            GameObject.Find($"craftTable/Slot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.Molecule.Keys.ElementAt(i - 1)}");
        }
        else
        {
            Destroy(GameObject.Find($"craftTable/Slot ({i})/Item/Image"));
            GameObject.Find($"craftTable/Slot ({i})/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
        }
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