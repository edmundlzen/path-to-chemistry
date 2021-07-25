using UnityEngine;
using UnityEngine.UI;


public static class hotbarData
{
    public static string slotNum { get; set; }
}

public class Hotbar : MonoBehaviour
{
    public static PlayerData playerData;
    void Start()
    {
        playerData = new PlayerData();
        hotbarData.slotNum = "1";
        GameObject.Find("Button" + hotbarData.slotNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
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
