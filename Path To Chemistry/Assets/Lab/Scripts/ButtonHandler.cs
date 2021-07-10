using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Variable
{
    public static string Hotbar { get; set; }
    public static Dictionary<string, string> hotbarData { get; set; }
}

public class ButtonHandler : MonoBehaviour
{
    public void Start()
    {
        Variable.hotbarData = new Dictionary<string, string>()
        {
            { "Slot1", "" },
            { "Slot2", "NaCl" },
            { "Slot3", "H2O" },
            { "Slot4", "Fe" },
            { "Slot5", "He" },
            { "Slot6", "Li" },
            { "Slot7", "K" },
            { "Slot8", "C" },
            { "Slot9", "Be" }
        };
        Variable.hotbarData["Slot1"] = "MG";
        Variable.Hotbar = "1";
        GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        Check();
    }
    public void Button1()
    {
        if (Variable.Hotbar != "1")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "1";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button2()
    {
        if (Variable.Hotbar != "2")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "2";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button3()
    {
        if (Variable.Hotbar != "3")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "3";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button4()
    {
        if (Variable.Hotbar != "4")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "4";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button5()
    {
        if (Variable.Hotbar != "5")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "5";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button6()
    {
        if (Variable.Hotbar != "6")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "6";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button7()
    {
        if (Variable.Hotbar != "7")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "7";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button8()
    {
        if (Variable.Hotbar != "8")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "8";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button9()
    {
        if (Variable.Hotbar != "9")
        {
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.grey;
            Variable.Hotbar = "9";
            GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
        }
    }
    void Check()
    {
        if (Variable.hotbarData["Slot1"] != "")
        {
            GameObject.Find("Text1").GetComponent<Text>().text = Variable.hotbarData["Slot1"];
        }
        if (Variable.hotbarData["Slot2"] != "")
        {
            GameObject.Find("Text2").GetComponent<Text>().text = Variable.hotbarData["Slot2"];
        }
        if (Variable.hotbarData["Slot3"] != "")
        {
            GameObject.Find("Text3").GetComponent<Text>().text = Variable.hotbarData["Slot3"];
        }
        if (Variable.hotbarData["Slot4"] != "")
        {
            GameObject.Find("Text4").GetComponent<Text>().text = Variable.hotbarData["Slot4"];
        }
        if (Variable.hotbarData["Slot5"] != "")
        {
            GameObject.Find("Text5").GetComponent<Text>().text = Variable.hotbarData["Slot5"];
        }
        if (Variable.hotbarData["Slot6"] != "")
        {
            GameObject.Find("Text6").GetComponent<Text>().text = Variable.hotbarData["Slot6"];
        }
        if (Variable.hotbarData["Slot7"] != "")
        {
            GameObject.Find("Text7").GetComponent<Text>().text = Variable.hotbarData["Slot7"];
        }
        if (Variable.hotbarData["Slot8"] != "")
        {
            GameObject.Find("Text8").GetComponent<Text>().text = Variable.hotbarData["Slot8"];
        }
        if (Variable.hotbarData["Slot9"] != "")
        {
            GameObject.Find("Text9").GetComponent<Text>().text = Variable.hotbarData["Slot9"];
        }
    }
}
