using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Variable
{
    public static string Hotbar { get; set; }
}

public class ButtonHandler : MonoBehaviour
{
    public void Start()
    {
        Variable.Hotbar = "1";
        GameObject.Find("Button" + Variable.Hotbar).GetComponent<Image>().color = Color.cyan;
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
}
