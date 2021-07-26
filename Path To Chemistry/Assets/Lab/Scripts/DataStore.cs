using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

[DataContract]
public class Data
{
    public Data()
    {
        Counter = 0;
        Array = new List<string>()
        {
            { "Hello" }
        };
        Dict = new Dictionary<string, string>()
        {
            { "Hello", "Hello" }
        };
    }
    [DataMember]
    public int Counter { get; set; }
    [DataMember]
    public string InputText { get; set; }
    [DataMember]
    public List<string> Array { get; set; }
    [DataMember]
    public Dictionary<string, string> Dict { get; set; }
}

public class DataStore : MonoBehaviour
{
    public static Data data;
    void Start()
    {
        data = new Data();
        GameObject.Find("Text1").GetComponent<Text>().text = data.Counter.ToString();
        GameObject.Find("Label1").GetComponent<Text>().text = Application.persistentDataPath;

    }
    public void Update()
    {
        data.InputText = GameObject.Find("Text2").GetComponent<Text>().text;
        GameObject.Find("Label1").GetComponent<Text>().text = $"{Application.persistentDataPath}/Saves.xml";
    }
    public void onButtonPressed()
    {
        data.Counter += 1;
        GameObject.Find("Text1").GetComponent<Text>().text = data.Counter.ToString();
    }
    public void Save()
    {
        using (var StrWriter = new StringWriter())
        {
            using (var XmlWriter = new XmlTextWriter(StrWriter))
            {
                var Serializer = new DataContractSerializer(typeof(Data));
                Serializer.WriteObject(XmlWriter, data);
                XmlWriter.Flush();
                File.WriteAllText($"{Application.persistentDataPath}/Saves.xml", StrWriter.ToString());
            }
        }
    }
    public void Load()
    {
        if (File.Exists($"{Application.persistentDataPath}/Saves.xml"))
        {
            var Serializer = new DataContractSerializer(typeof(Data));
            var Stream = new FileStream($"{Application.persistentDataPath}/Saves.xml", FileMode.Open);
            data = Serializer.ReadObject(Stream) as Data;
            Stream.Close();
            GameObject.Find("Text1").GetComponent<Text>().text = data.Counter.ToString();
            GameObject.Find("Label2").GetComponent<Text>().text = data.InputText.ToString();
        }
    }
}
