using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class elementConstructor
{
    public static int Protons { get; set; }
    public static int Electrons { get; set; }
    public static int Neutrons { get; set; }
    public static bool hasLoaded = false;
    public static bool hasDone = false;
    public static bool Pause = false;
    public static string SelectedElement = "";
}

public class ElementConstructor : MonoBehaviour
{
    public void Start()
    {
        if (!elementConstructor.hasLoaded)
        {
            Load();
            elementConstructor.hasLoaded = true;
        }
    }
    public void addProton()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Protons + 1 <= 120)
            {
                elementConstructor.Protons += 1;
                GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
    }
    public void addElectron()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Electrons + 1 <= 120)
            {
                elementConstructor.Electrons += 1;
                GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
    }
    public void addNeutron()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Neutrons + 1 <= 180)
            {
                elementConstructor.Neutrons += 1;
                GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
    }
    public void removeProton()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Protons - 1 >= 0)
            {
                elementConstructor.Protons -= 1;
                GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
    }
    public void removeElectron()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Electrons - 1 >= 0)
            {
                elementConstructor.Electrons -= 1;
                GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
    }
    public void removeNeuton()
    {
        if (!elementConstructor.Pause)
        {
            if (elementConstructor.Neutrons - 1 >= 0)
            {
                elementConstructor.Neutrons -= 1;
                GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
                GameObject.Find("Product").GetComponent<Text>().text = "Craft";
            }
        }
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
    public void Craft(GameObject BuyUI)
    {
        var playerData = PlayerData.Instance();
        var elementData = ElementData.Instance();
        if (!elementConstructor.Pause)
        {
            foreach (var Keys in elementData.elements.Keys)
            {
                if (Convert.ToString(elementConstructor.Protons) == elementData.elements[Keys]["protons"] &&
                    Convert.ToString(elementConstructor.Electrons) == elementData.elements[Keys]["electrons"] &&
                    Convert.ToString(elementConstructor.Neutrons) == elementData.elements[Keys]["neutrons"])
                {
                    BuyUI.SetActive(true);
                    player.labPause = true;
                    elementConstructor.Pause = true;
                    elementConstructor.SelectedElement = Keys;
                    GameObject.Find("Product").GetComponent<Text>().text = Keys;
                    GameObject.Find("ElementPanel/ElementName").GetComponent<Text>().text = Keys;
                    break;
                }
                else
                {
                    elementConstructor.SelectedElement = null;
                    GameObject.Find("Product").GetComponent<Text>().text = "Nothing!";
                    return;
                }
            }
            if (elementConstructor.SelectedElement != null)
            {
                for (int i = 1; i <= 94; i++)
                {
                    if (elementData.rarity[i - 1] == elementConstructor.SelectedElement)
                    {
                        GameObject.Find("Slider").GetComponent<Slider>().maxValue = playerData.Energy / 50 * i + 1;
                        break;
                    }
                    else
                    {
                        GameObject.Find("Slider").GetComponent<Slider>().maxValue = playerData.Energy / 50;
                        break;
                    }
                }
                GameObject.Find("Slider").GetComponent<Slider>().value = 0;
            }
        }
    }
    
    public void Buy(GameObject BuyUI)
    {
        var playerData = PlayerData.Instance();
        var elementData = ElementData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("BuyElement/Slider").GetComponent<Slider>().value));
        if (sliderValue > 0)
        {
            for (var i = 1; i <= 9; i = i + 1)
            {
                if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == elementConstructor.SelectedElement)
                {
                    var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                    if (Balance + sliderValue <= 64)
                    {
                        playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + sliderValue;
                        elementConstructor.hasDone = true;
                        break;
                    }
                    else
                    {
                        playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + (64 - Balance);
                        addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                        BuyUI.SetActive(false);
                        slotCheck();
                        return;
                    }
                }
            }
            if (!elementConstructor.hasDone)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                    {
                        if (sliderValue <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = elementConstructor.SelectedElement;
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                            elementConstructor.hasDone = true;
                            break;
                        }
                        else
                        {
                            sliderValue = 64;
                            playerData.slotItem[$"Slot{i}"]["Element"] = elementConstructor.SelectedElement;
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                            addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                            elementConstructor.hasDone = true;
                            break;
                        }
                    }
                }
            }
            if (!elementConstructor.hasDone)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                    {
                        addAlert("Alert: The player's hotbar are full!");
                        BuyUI.SetActive(false);
                        return;
                    }
                }
            }
            for (int i = 1; i <= 94; i++)
            {
                if (elementData.rarity[i - 1] == elementConstructor.SelectedElement)
                {
                    playerData.Energy -= sliderValue * 50 * i + 1;
                    break;
                }
                else
                {
                    playerData.Energy -= sliderValue * 50;
                    break;
                }
            }
        }
        GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        GameObject.Find("Energy").GetComponent<Text>().text = Convert.ToString(playerData.Energy);
        elementConstructor.hasDone = false;
        elementConstructor.Pause = false;
        BuyUI.SetActive(false);
        slotCheck();
    }

    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
        GameObject.Find("ItemName").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{Convert.ToInt32(hotbar.slotNum)}"]["Element"]);
    }
}