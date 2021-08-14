using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    void Start()
    {
        elementCheck();
    }
    public void elementSymbol()
    {
        print("Hello World");
    }
    public void elementCheck()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        for (int i = 1; i <= 118; i++)
        {
            GameObject.Find($"Slot ({i})").transform.GetComponentInChildren<Text>().text = elementData.elements.Keys.ElementAt(i - 1);
        }
    }
}
