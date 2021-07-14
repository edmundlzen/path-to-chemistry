using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Chemidex
{
    public static string chemNum { get; set; }
    public static Dictionary<string, string> chemState { get; set; }
    public static Dictionary<string, string> chemRecipes { get; set; }
}
public class ChemidexHandler : MonoBehaviour
{
    void Start()
    {
        Chemidex.chemNum = "1";
        Chemidex.chemState = new Dictionary<string, string>();
        levelData.levelAvailable = new List<string>();
        levelData.levelAvailable.Add("Level 1");
        levelData.levelAvailable.Add("Level 2");
        levelData.levelAvailable.Add("Level 3");
        Chemidex.chemRecipes = new Dictionary<string, string>();
        Chemidex.chemRecipes.Add("Recipe 1", "K + H2O");
        Chemidex.chemRecipes.Add("Recipe 2", "HCl + NH3");
        Chemidex.chemRecipes.Add("Recipe 3", "H2O2 + NaI");
        Chemidex.chemRecipes.Add("Recipe 4", "C2H3NaO2 + H2O");
        Chemidex.chemRecipes.Add("Recipe 5", "KI + H2O2 + C18H35NaO2");
        GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
        for (int i = 1; i <= 5; i++)
        {
            if (levelData.levelAvailable.Contains($"Level {i}"))
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
        if (levelData.levelAvailable.Contains($"Level {Chemidex.chemNum}"))
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = Chemidex.chemRecipes[$"Recipe {Chemidex.chemNum}"];
        }
        else
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = "?";
        }
    }
}
