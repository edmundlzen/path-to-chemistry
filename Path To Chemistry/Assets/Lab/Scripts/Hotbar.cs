using System;
using System.Linq;
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
        hotbar.slotNum = "1";
        GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        var playerData = PlayerData.Instance();
        slotCheck();
        flaskCheck();
        craftingTable();
    }
    public void Button1()
    {
        slotButton("1");
    }
    public void Button2()
    {
        slotButton("2");
    }
    public void Button3()
    {
        slotButton("3");
    }
    public void Button4()
    {
        slotButton("4");
    }
    public void Button5()
    {
        slotButton("5");
    }
    public void Button6()
    {
        slotButton("6");
    }
    public void Button7()
    {
        slotButton("7");
    }
    public void Button8()
    {
        slotButton("8");
    }
    public void Button9()
    {
        slotButton("9");
    }
    public void flaskSlot1()
    {
        flaskButton(1);
    }
    public void flaskSlot2()
    {
        flaskButton(2);
    }
    public void flaskSlot3()
    {
        flaskButton(3);
    }
    public void flaskSlot4()
    {
        flaskButton(4);
    }
    public void flaskSlot5()
    {
        flaskButton(5);
    }
    public void flaskSlot6()
    {
        flaskButton(6);
    }
    public void flaskSlot7()
    {
        flaskButton(7);
    }
    public void flaskSlot8()
    {
        flaskButton(8);
    }
    public void flaskSlot9()
    {
        flaskButton(9);
    }
    public void flaskSlot10()
    {
        flaskButton(10);
    }
    public void craftSlot1()
    {
        craftButton(1);
    }
    public void craftSlot2()
    {
        craftButton(2);
    }
    public void craftSlot3()
    {
        craftButton(3);
    }
    public void craftSlot4()
    {
        craftButton(4);
    }
    public void craftSlot5()
    {
        craftButton(5);
    }
    public void craftSlot6()
    {
        craftButton(6);
    }
    public void craftSlot7()
    {
        craftButton(7);
    }
    public void craftSlot8()
    {
        craftButton(8);
    }
    public void craftSlot9()
    {
        craftButton(9);
    }
    public void craftSlot10()
    {
        craftButton(10);
    }
    void slotButton(string Num)
    {
        if (hotbar.slotNum != Num)
        {
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.grey;
            hotbar.slotNum = Num;
            GameObject.Find("Button" + hotbar.slotNum).GetComponent<Image>().color = Color.cyan;
        }
    }
    void flaskButton(int Index)
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= Index)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.flaskElements.Keys.ElementAt(Index - 1))
                {
                    playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                    break;
                }
                else if ((playerData.slotItem[$"Slot{i}"]["Element"] == null) && (playerData.slotItem[$"Slot{i}"]["Quantity"] == null))
                {
                    playerData.slotItem[$"Slot{i}"]["Element"] = playerData.flaskElements.Keys.ElementAt(Index - 1);
                    playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                    break;
                }
            }
            if (playerData.flaskElements.Values.ElementAt(Index - 1) > 1)
            {
                playerData.flaskElements[playerData.flaskElements.Keys.ElementAt(Index - 1)] = Convert.ToInt32(playerData.flaskElements[playerData.flaskElements.Keys.ElementAt(Index - 1)].ToString()) - 1;
            }
            else
            {
                playerData.flaskElements.Remove(playerData.flaskElements.Keys.ElementAt(Index - 1));
            }
        }
        slotCheck();
        flaskCheck();
    }
    void craftButton(int Index)
    {
        var playerData = PlayerData.Instance();
        if (playerData.Molecule.Count >= Index)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Molecule.Keys.ElementAt(Index - 1))
                {
                    playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                    break;
                }
                else if ((playerData.slotItem[$"Slot{i}"]["Element"] == null) && (playerData.slotItem[$"Slot{i}"]["Quantity"] == null))
                {
                    playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Molecule.Keys.ElementAt(Index - 1);
                    playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                    break;
                }
            }
            if (playerData.Molecule.Values.ElementAt(Index - 1) > 1)
            {
                playerData.Molecule[playerData.Molecule.Keys.ElementAt(Index - 1)] = Convert.ToInt32(playerData.Molecule[playerData.Molecule.Keys.ElementAt(Index - 1)].ToString()) - 1;
            }
            else
            {
                playerData.Molecule.Remove(playerData.Molecule.Keys.ElementAt(Index - 1));
            }
        }
        slotCheck();
        craftingTable();
    }
    void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            if ((playerData.slotItem[$"Slot{i}"]["Element"] != null) && (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
            else if ((playerData.slotItem[$"Slot{i}"]["Element"] != null) && (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if ((playerData.slotItem[$"Slot{i}"]["Element"] == null) && (playerData.slotItem[$"Slot{i}"]["Quantity"] == null))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = "";
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
        }
    }
    void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Item{i}").GetComponent<Text>().text = "";
            GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.flaskElements.Count; i = i + 1)
        {
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text = playerData.flaskElements.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
            }
        }
    }
    void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Element{i}").GetComponent<Text>().text = "";
            GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.Molecule.Count; i = i + 1)
        {
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Element{i}").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = playerData.Molecule.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Element{i}").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = "";
            }
        }
    }
}

