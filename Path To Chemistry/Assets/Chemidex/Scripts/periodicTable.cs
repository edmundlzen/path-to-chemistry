using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class PeriodicTable
{
    public static string periodicNum { get; set; }
}

public class periodicTable : MonoBehaviour
{
    void Start()
    {
        Load();
        var elementData = ElementData.Instance();
        elementCheck();
        PeriodicTable.periodicNum = "Element (1)";
        GameObject.Find(PeriodicTable.periodicNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 118; i++)
        {
            if (PeriodicTable.periodicNum == $"Element ({i})")
            {
                var elementState = elementData.elements[elementData.elements.Keys.ElementAt(i - 1)];
                GameObject.Find("State").GetComponent<Text>().text = $"Symbol: {elementState["symbol"]}\nName: {elementState["name"]}\nGroup: {elementState["group"]}\nProtons: {elementState["protons"]}\nElectrons: {elementState["electrons"]}\nNeutrons: {elementState["neutrons"]}\nWeight: {elementState["weight"]}";
                break;
            }
        }
    }
    public void Back()
    {
        SceneManager.LoadScene("Chemidex");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    void elementCheck()
    {
        var elementData = ElementData.Instance();
        for (int i = 1; i <= 118; i++)
        {
            GameObject.Find($"Element ({i})/Symbol").GetComponent<Text>().text = elementData.elements.Keys.ElementAt(i - 1);
        }
    }
    void addElement()
    {
        var elementData = ElementData.Instance();
        if (PeriodicTable.periodicNum != EventSystem.current.currentSelectedGameObject.name)
        {
            GameObject.Find(PeriodicTable.periodicNum).GetComponent<Image>().color = Color.white;
            PeriodicTable.periodicNum = EventSystem.current.currentSelectedGameObject.name;
            GameObject.Find(PeriodicTable.periodicNum).GetComponent<Image>().color = Color.cyan;
            for (int i = 1; i <= 118; i++)
            {
                if (EventSystem.current.currentSelectedGameObject.name == $"Element ({i})")
                {
                    var elementState = elementData.elements[elementData.elements.Keys.ElementAt(i - 1)];
                    GameObject.Find("State").GetComponent<Text>().text = $"Symbol: {elementState["symbol"]}\nName: {elementState["name"]}\nGroup: {elementState["group"]}\nProtons: {elementState["protons"]}\nElectrons: {elementState["electrons"]}\nNeutrons: {elementState["neutrons"]}\nWeight: {elementState["weight"]}";
                    break;
                }
            }
        }
    }
    void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
}
