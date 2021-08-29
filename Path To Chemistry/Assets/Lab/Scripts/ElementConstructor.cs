using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public static class elementConstructor
{
    public static int Protons { get; set; }
    public static int Electrons { get; set; }
    public static int Neutrons { get; set; }
}

public class ElementConstructor : MonoBehaviour
{
    public void Start()
    {
        Load();
        elementConstructor.Protons = 0;
        elementConstructor.Electrons = 0;
        elementConstructor.Neutrons = 0;
        GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
        GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
        GameObject.Find("NeutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
    }

    public void addProton()
    {
        if (elementConstructor.Protons + 1 <= 120)
        {
            elementConstructor.Protons += 1;
            GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void addElectron()
    {
        if (elementConstructor.Electrons + 1 <= 120)
        {
            elementConstructor.Electrons += 1;
            GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void addNeutron()
    {
        if (elementConstructor.Neutrons + 1 <= 180)
        {
            elementConstructor.Neutrons += 1;
            GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void removeProton()
    {
        if (elementConstructor.Protons - 1 >= 0)
        {
            elementConstructor.Protons -= 1;
            GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void removeElectron()
    {
        if (elementConstructor.Electrons - 1 >= 0)
        {
            elementConstructor.Electrons -= 1;
            GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void removeNeuton()
    {
        if (elementConstructor.Neutrons - 1 >= 0)
        {
            elementConstructor.Neutrons -= 1;
            GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void Craft()
    {
        var elementData = ElementData.Instance();
        foreach (var Keys in elementData.elements.Keys)
            if (Convert.ToString(elementConstructor.Protons) == elementData.elements[Keys]["protons"] &&
                Convert.ToString(elementConstructor.Electrons) == elementData.elements[Keys]["electrons"] &&
                Convert.ToString(elementConstructor.Neutrons) == elementData.elements[Keys]["neutrons"])
            {
                GameObject.Find("Product").GetComponent<Text>().text = Keys;
                break;
            }
            else
            {
                GameObject.Find("Product").GetComponent<Text>().text = "Nothing!";
            }
    }

    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
}