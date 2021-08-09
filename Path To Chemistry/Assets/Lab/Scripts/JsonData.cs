using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveObject
{
    private static readonly object threadlock = new object();
    private static SaveObject instance = null;
    public static SaveObject Instance()
    {
        lock (threadlock)
        {
            if (instance == null)
            {
                instance = new SaveObject();
            }
            return instance;
        }
    }
    public void UpdateSaveObject(SaveObject saveObject)
    {
        instance = saveObject;
    }
    private SaveObject()
    {
        Counter = 0;
        InputText = "";
        Array = new List<string>()
    {
        { "Hello1" },
        { "Hello2" },
        { "Hello3" },
        { "Hello4" },
        { "Hello5" }
    };
        Dict = new Dictionary<string, string>()
    {
        { "Hello1", "Hello1" },
        { "Hello2", "Hello2" },
        { "Hello3", "Hello3" },
        { "Hello4", "Hello4" },
        { "Hello5", "Hello5" }
        };
    }
    public int Counter { get; set; }
    public string InputText { get; set; }
    public List<string> Array { get; set; }
    public Dictionary<string, string> Dict { get; set; }
}

public class JsonData : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Label1").GetComponent<Text>().text = $"{Application.persistentDataPath}/Save Data/Saves.txt";
    }
    void Update()
    {
        var saveObject = SaveObject.Instance();
        GameObject.Find("Text1").GetComponent<Text>().text = saveObject.Counter.ToString();
        GameObject.Find("Text2").GetComponent<Text>().text = saveObject.InputText;
    }
    public void onButtonPressed()
    {
        var saveObject = SaveObject.Instance();
        saveObject.Counter += 1;
        GameObject.Find("Text1").GetComponent<Text>().text = saveObject.Counter.ToString();
    }
    public void Save()
    {
        var saveObject = SaveObject.Instance();
        saveObject.InputText = GameObject.Find("Text2").GetComponent<Text>().text;
        string directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(saveObject, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    public void Load()
    {
        string directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var saveObject = JsonConvert.DeserializeObject<SaveObject>(fileContent);
        SaveObject.Instance().UpdateSaveObject(saveObject);
    }
}