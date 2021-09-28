using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private int gay = 0;
    private string all;
    private void Start()
    {
        Load();
        var elementData = ElementData.Instance();
        foreach (var i in elementData.elements.Keys)
        {
            gay += 1;
            all += $"{gay}. {i}\n";
        }
        GameObject.Find("Text").GetComponent<Text>().text = all;
    }
    private void Load()
    {
        var elementData = JsonConvert.DeserializeObject<ElementData>(allElements.Data);
        ElementData.Instance().UpdateElementData(elementData);
    }
}
