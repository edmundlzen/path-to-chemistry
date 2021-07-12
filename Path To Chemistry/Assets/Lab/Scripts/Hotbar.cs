using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class hotbarData
{
    public static string slotNum { get; set; }
    public static Dictionary<string, string> slotItem { get; set; }
}

public class Hotbar : MonoBehaviour
{
    void Start()
    {
        hotbarData.slotNum = "1";
        hotbarData.slotItem = new Dictionary<string, string>()
        {
            { "Slot1", "MG" },
            { "Slot2", "H" },
            { "Slot3", "H" },
            { "Slot4", "Fe" },
            { "Slot5", "He" },
            { "Slot6", "O" },
            { "Slot7", "K" },
            { "Slot8", "C" },
            { "Slot9", "Be" }
        };
        GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = hotbarData.slotItem[$"Slot{i}"];
        }
    }
    public void Button1()
    {
        if (hotbarData.slotNum != "1")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "1";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button2()
    {
        if (hotbarData.slotNum != "2")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "2";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button3()
    {
        if (hotbarData.slotNum != "3")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "3";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button4()
    {
        if (hotbarData.slotNum != "4")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "4";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button5()
    {
        if (hotbarData.slotNum != "5")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "5";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button6()
    {
        if (hotbarData.slotNum != "6")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "6";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button7()
    {
        if (hotbarData.slotNum != "7")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "7";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button8()
    {
        if (hotbarData.slotNum != "8")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "8";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button9()
    {
        if (hotbarData.slotNum != "9")
        {
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.grey;
            hotbarData.slotNum = "9";
            GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
}
