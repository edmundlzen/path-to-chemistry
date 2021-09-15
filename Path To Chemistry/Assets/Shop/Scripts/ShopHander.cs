using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class ShopData
{
    public static List<string> Section = new List<string>();
    public static int sectionNum = 1;
}

public class ShopHander : MonoBehaviour
{
    private void Start()
    {
        Default();
        ProductName();
        GameObject.Find("Section").GetComponent<Text>().text = ShopData.Section[0];
    }
    public void DropDown(int Value)
    {
        ShopData.Section.Clear();
        ShopData.sectionNum = 1;
        if (Value == 0)
        {
            Default();
        }
        else if (Value == 1)
        {
            ShopData.Section.Add("Items");
        }
        else if (Value == 2)
        {
            for (int i = 1; i <= 5; i++)
            {
                ShopData.Section.Add($"Materials {i}");
            }
        }
        else if (Value == 3)
        {
            ShopData.Section.Add("Lab Maps");
        }
        GameObject.Find("Section").GetComponent<Text>().text = ShopData.Section[0];
        ProductName();
    }
    public void Default()
    {
        ShopData.Section.Add("Items");
        for (int i = 1; i <= 5; i++)
        {
            ShopData.Section.Add($"Materials {i}");
        }
        ShopData.Section.Add("Lab Maps");
    }

    public void Next()
    {
        if (ShopData.sectionNum + 1 <= ShopData.Section.Count)
        {
            ShopData.sectionNum += 1;
        }
        else
        {
            ShopData.sectionNum = 1;
        }
        GameObject.Find("Section").GetComponent<Text>().text = ShopData.Section[ShopData.sectionNum - 1];
        ProductName();
    }

    public void Previous()
    {
        if (ShopData.sectionNum - 1 > 0)
        {
            ShopData.sectionNum -= 1;     
        }
        else
        {
            ShopData.sectionNum = ShopData.Section.Count;
        }
        GameObject.Find("Section").GetComponent<Text>().text = ShopData.Section[ShopData.sectionNum - 1];
        ProductName();
    }

    private void ProductName()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 6; i++)
        {
            GameObject.Find($"Image ({i})").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Empty");
            GameObject.Find($"Image ({i})/Product").GetComponent<Text>().text = "";
            GameObject.Find($"Image ({i})/Price").GetComponent<Text>().text = "";
        }
            for (int i = 1; i <= playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Count; i++)
        {
            GameObject.Find($"Image ({i})").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Keys.ElementAt(i - 1)}");
            GameObject.Find($"Image ({i})/Product").GetComponent<Text>().text = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Keys.ElementAt(i - 1);
            GameObject.Find($"Image ({i})/Price").GetComponent<Text>().text = Convert.ToString(playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Values.ElementAt(i - 1));
        }
    }
}
