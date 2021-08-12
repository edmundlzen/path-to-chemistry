using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;

public class ElementData
{
    public ElementData()
    {
        elementDat = new Dictionary<string, string>();
    }
    public Dictionary<string, string> elementDat { get; set; }
}

public class Elements : MonoBehaviour
{
    public ElementData elementData;
    void Start()
    {
        elementData = new ElementData();
    }
    public void Load()
    {
        string directory = $"{Application.dataPath}";
        var filePath = Path.Combine(directory, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        elementData = JsonConvert.DeserializeObject<ElementData>(fileContent.ToString());
        GameObject.Find("Text").GetComponent<Text>().text = elementData.ToString();
    }
}
