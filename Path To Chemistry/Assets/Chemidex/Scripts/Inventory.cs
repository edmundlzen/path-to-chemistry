using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    void Start()
    {
        Load();
        elementCheck();
        var elementData = ElementData.Instance();
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 118; i++)
        {
            playerData.Inventory.Add(elementData.elements.Keys.ElementAt(i - 1), 0);
        }
        Save();
    }
    public void elementSymbol()
    {
        print("Hello World");
    }
    public void elementCheck()
    {
        var elementData = ElementData.Instance();
        for (int i = 1; i <= 118; i++)
        {
            GameObject.Find($"Slot ({i})").transform.GetComponentInChildren<Text>().text = elementData.elements.Keys.ElementAt(i - 1);
        }
    }
    void Save()
    {
        print(Application.persistentDataPath);
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
    void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
}
