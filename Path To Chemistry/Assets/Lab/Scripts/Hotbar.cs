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
    public void flaskSlot1()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 1)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[0];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(0);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot2()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 2)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[1];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(1);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot3()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 3)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[2];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(2);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot4()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 4)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[3];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(3);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot5()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 5)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[4];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(4);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot6()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 6)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[5];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(5);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot7()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 7)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[6];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(6);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot8()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 8)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[7];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(7);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot9()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 9)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[8];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(8);
        }
        slotCheck();
        flaskCheck();
    }
    public void flaskSlot10()
    {
        var playerData = PlayerData.Instance();
        if (playerData.flaskElements.Count >= 10)
        {
            for (int i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"] == "")
                {
                    playerData.slotItem[$"Slot{i}"] = playerData.flaskElements[9];
                    break;
                }
            }
            playerData.flaskElements.RemoveAt(9);
        }
        slotCheck();
        flaskCheck();
    }
    public void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
        }
    }
    public void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Item{i}").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.flaskElements.Count; i = i + 1)
        {
            GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements[i - 1];
        }
    }
}

