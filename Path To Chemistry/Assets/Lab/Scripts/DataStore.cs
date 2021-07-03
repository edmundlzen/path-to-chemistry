using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public static class Store
{
    public static Dictionary<string, string> Data { get; set; }
}

public class DataStore : MonoBehaviour
{
    void Start()
    {
        Store.Data = new Dictionary<string, string>()
        {
            { "Slot1", "MG" },
            { "Slot2", "" },
            { "Slot3", "He" },
            { "Slot4", "K" },
            { "Slot5", "H" },
            { "Slot6", "" },
            { "Slot7", "Fe" },
            { "Slot8", "" }
        };
        GameObject.Find("Label").GetComponent<Text>().text = $"{ Application.persistentDataPath }/Save.txt";
    }

    
    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = $"{ Application.persistentDataPath }/Save.txt";
        FileStream stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, Store.Data);
        stream.Close();
    }
    public void Load()
    {
        string filePath = $"{ Application.persistentDataPath }/Save.txt";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            GameObject.Find("Label").GetComponent<Text>().text = formatter.Deserialize(stream).ToString();
            stream.Close();
        }
        else
        {
            Debug.LogError("File Not Found!");
        }
    }
}
