using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public static class ShopData
{
    public static int slotNum;
    public static int sectionNum = 1;
    public static List<string> Section = new List<string>();
}

public class ShopHander : MonoBehaviour
{
    public GameObject Unaffordable;
    private void Start()
    {
        var playerData = PlayerData.Instance();
        Default();
        ProductName();
        GameObject.Find("Section").GetComponent<Text>().text = ShopData.Section[0];
        GameObject.Find("Energy").GetComponent<Text>().text = $"${playerData.Energy}";
    }
    public void DropDown(int Value)
    {
        if (!player.Pause)
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
                for (int i = 1; i <= 6; i++)
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
    }
    public void Default()
    {
        ShopData.Section.Add("Items");
        for (int i = 1; i <= 6; i++)
        {
            ShopData.Section.Add($"Materials {i}");
        }
        ShopData.Section.Add("Lab Maps");
    }

    public void Back()
    {
        SceneManager.LoadScene("Terrain");
    }

    public void Next()
    {
        if (!player.Pause)
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
    }

    public void Previous()
    {
        if (!player.Pause)
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
    }

    private void ProductName()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 6; i++)
        {
            GameObject.Find($"Image ({i})").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Shop/Empty");
            GameObject.Find($"Image ({i})/Image/Product").GetComponent<Text>().text = "";
            GameObject.Find($"Image ({i})/Buy{i}/Price").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Count; i++)
        {
            var Item = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Keys.ElementAt(i - 1);
            var Price = $"${playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Values.ElementAt(i - 1)}";
            GameObject.Find($"Image ({i})").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Shop/{Item}");
            GameObject.Find($"Image ({i})/Image/Product").GetComponent<Text>().text = Item;
            if (playerData.nonResellable.ContainsKey(Item))
            {
                if (playerData.nonResellable[Item])
                {
                    GameObject.Find($"Image ({i})/Buy{i}/Price").GetComponent<Text>().text = "Sold";
                }
                else
                {
                    GameObject.Find($"Image ({i})/Buy{i}/Price").GetComponent<Text>().text = Price;
                }
            }
            else
            {
                GameObject.Find($"Image ({i})/Buy{i}/Price").GetComponent<Text>().text = Price;
            }
        }
    }

    public void Slider(float Quantity)
    {
        var playerData = PlayerData.Instance();
        var PricePerQuantity = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Values.ElementAt(ShopData.slotNum - 1);
        GameObject.Find("sliderValue").GetComponent<Text>().text = $"Quantity: {Math.Floor(Quantity)}";
        GameObject.Find("Cost").GetComponent<Text>().text = $"Cost: ${Math.Floor(Quantity) * PricePerQuantity}";
    }

    public void maxQuantity()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = GameObject.Find("Slider").GetComponent<Slider>().maxValue;
    }

    public void addQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value -= 1;
        }
    }

    public void Buy(GameObject IdentidyQuantity)
    {
        var playerData = PlayerData.Instance();
        if (!player.Pause)
        {
            ShopData.slotNum = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Replace("Buy", ""));
            var Item = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Keys.ElementAt(ShopData.slotNum - 1);
            var Cost = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Values.ElementAt(ShopData.slotNum - 1);
            if (playerData.nonResellable.ContainsKey(Item))
            {
                if (!playerData.nonResellable[Item])
                {
                    if (playerData.Energy >= Cost)
                    {
                        int maxQuantity = 1;
                        player.Pause = true;
                        GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().interactable = false;
                        IdentidyQuantity.SetActive(true);
                        GameObject.Find("Slider").GetComponent<Slider>().maxValue = maxQuantity;
                        GameObject.Find("Slider").GetComponent<Slider>().value = 0;
                    }
                    else if (playerData.Energy < Cost)
                    {
                        Unaffordable.SetActive(true);
                        GameObject.Find("Info/Text").GetComponent<Text>().text = $"You have no enough energy to buy {Item}";
                    }
                }
                else if (playerData.Energy < Cost)
                {
                    Unaffordable.SetActive(true);
                    GameObject.Find("Info/Text").GetComponent<Text>().text = $"You have no enough energy to buy {Item}";
                }
                else if (playerData.nonResellable[Item])
                {
                    Unaffordable.SetActive(true);
                    GameObject.Find("Info/Text").GetComponent<Text>().text = $"You have bought {Item}";
                }
            }
            else
            {
                if (playerData.Energy >= Cost)
                {
                    int maxQuantity = playerData.Energy / Cost;
                    player.Pause = true;
                    GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().interactable = false;
                    IdentidyQuantity.SetActive(true);
                    GameObject.Find("Slider").GetComponent<Slider>().maxValue = maxQuantity;
                    GameObject.Find("Slider").GetComponent<Slider>().value = 0;
                }
                else if (playerData.Energy < Cost)
                {
                    Unaffordable.SetActive(true);
                    GameObject.Find("Info/Text").GetComponent<Text>().text = $"You have no enough energy to buy {Item}";
                }
            }
        }
    }

    public void Deal(GameObject IdentidyQuantity)
    {
        var playerData = PlayerData.Instance();
        var Quantity = Convert.ToInt32(Math.Floor(GameObject.Find("Slider").GetComponent<Slider>().value));
        var PricePerQuantity = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Values.ElementAt(ShopData.slotNum - 1);
        var Item = playerData.Shop[ShopData.Section[ShopData.sectionNum - 1]].Keys.ElementAt(ShopData.slotNum - 1);
        playerData.Energy -= Quantity * PricePerQuantity;
        if (playerData.nonResellable.ContainsKey(Item))
        {
            playerData.nonResellable[Item] = true;
        }
        GameObject.Find("Energy").GetComponent<Text>().text = $"${playerData.Energy}";
        IdentidyQuantity.SetActive(false);
        player.Pause = false;
        GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().interactable = true;
        ProductName();
    }

    public void Ok()
    {
        Unaffordable.SetActive(false);
    }
}
