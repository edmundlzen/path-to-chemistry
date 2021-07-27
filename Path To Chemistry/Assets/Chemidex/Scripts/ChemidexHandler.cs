using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Chemidex
{
    public static string chemNum { get; set; }
    public static Dictionary<string, string> chemRecipes { get; set; }
}

public class ChemidexHandler : MonoBehaviour
{
    void Start()
    {
        var playerData = PlayerData.Instance();
        Chemidex.chemNum = "1";
        Chemidex.chemRecipes = new Dictionary<string, string>()
        {
            { "Recipe 1", "K + H2O" },
            { "Recipe 2", "HCl + NH3" },
            { "Recipe 3", "H2O2 + NaI" },
            { "Recipe 4", "C2H3NaO2 + H2O" },
            { "Recipe 5", "KI + H2O2 + C18H35NaO2" }
        };
        GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 5; i++)
        {
            if (playerData.levelAvailable.Contains($"Level {i}"))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = i.ToString();
            }
            else
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = "?";
            }
        }
        chemCheck();
    }
    public void Chem1()
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != "1")
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = "1";
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem2()
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != "2")
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = "2";
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem3()
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != "3")
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = "3";
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem4()
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != "4")
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = "4";
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem5()
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != "5")
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = "5";
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    void chemCheck()
    {
        var playerData = PlayerData.Instance();
        if (playerData.levelAvailable.Contains($"Level {Chemidex.chemNum}"))
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = Chemidex.chemRecipes[$"Recipe {Chemidex.chemNum}"];
        }
        else
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = "?";
        }
    }
}
