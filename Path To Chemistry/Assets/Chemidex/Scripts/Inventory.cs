using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class InventoryData
{
    public static string Slot { get; set; }
    public static string Input { get; set; }
}

public class Inventory : MonoBehaviour
{
    private void Start()
    {
        Load();
        Load2();
        elementCheck();
        InventoryData.Slot = "Slot (1)";
        GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
        var elementData = ElementData.Instance();
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 118; i++)
            if (InventoryData.Slot == $"Slot ({i})")
            {
                var Name = playerData.Inventory.Keys.ElementAt(i - 1);
                var Quantity = playerData.Inventory.Values.ElementAt(i - 1);
                GameObject.Find("State").GetComponent<Text>().text = $"Name: {Name}\nQuantity: {Quantity}";
            }
        /*
        for (int i = 1; i <= 118; i++)
        {
            playerData.Inventory.Add(elementData.elements.Keys.ElementAt(i - 1), 0);
        }
        Save();
*/
    }

    public void quantityManager(string Input)
    {
        InventoryData.Input = Input;
    }

    public void Done()
    {
        for (var i = 1; i <= 118; i++)
        {
            var playerData = PlayerData.Instance();
            if (Convert.ToInt32(InventoryData.Input) > 100)
                GameObject.Find("Info").GetComponent<Text>().text = "Max";
            else if (InventoryData.Slot == $"Slot ({i})")
                if (Convert.ToInt32(InventoryData.Input) <=
                    playerData.Inventory[playerData.Inventory.Keys.ElementAt(i - 1)])
                    GameObject.Find("Info").GetComponent<Text>().text = "Min";
        }
    }

    public void elementSymbol()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.white;
        InventoryData.Slot = EventSystem.current.currentSelectedGameObject.name;
        GameObject.Find(InventoryData.Slot).GetComponent<Image>().color = Color.cyan;
        for (var i = 1; i <= 118; i++)
            if (EventSystem.current.currentSelectedGameObject.name == $"Slot ({i})")
            {
                var Name = playerData.Inventory.Keys.ElementAt(i - 1);
                var Quantity = playerData.Inventory.Values.ElementAt(i - 1);
                GameObject.Find("State").GetComponent<Text>().text = $"Name: {Name}\nQuantity: {Quantity}";
                break;
            }
    }

    public void elementCheck()
    {
        var elementData = ElementData.Instance();
        for (var i = 1; i <= 118; i++)
            GameObject.Find($"Slot ({i})").transform.GetComponentInChildren<Text>().text =
                elementData.elements.Keys.ElementAt(i - 1);
    }

    private void Save()
    {
        print(Application.persistentDataPath);
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
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
}