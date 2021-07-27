using UnityEngine;
using UnityEngine.UI;

public static class hotbar
{
    public static string slotNum { get; set; }
}

public class Hotbar : MonoBehaviour
{
    void Start()
    {
        var playerData = PlayerData.Instance();
        hotbar.slotNum = "1";
        GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
        }
    }
    public void Button1()
    {
        if (hotbar.slotNum != "1")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "1";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button2()
    {
        if (hotbar.slotNum != "2")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "2";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button3()
    {
        if (hotbar.slotNum != "3")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "3";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button4()
    {
        if (hotbar.slotNum != "4")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "4";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button5()
    {
        if (hotbar.slotNum != "5")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "5";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button6()
    {
        if (hotbar.slotNum != "6")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "6";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button7()
    {
        if (hotbar.slotNum != "7")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "7";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button8()
    {
        if (hotbar.slotNum != "8")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "8";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    public void Button9()
    {
        if (hotbar.slotNum != "9")
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = "9";
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
}
