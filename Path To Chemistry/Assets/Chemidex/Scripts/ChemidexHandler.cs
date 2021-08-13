using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Chemidex
{
    public static string chemNum { get; set; }
    public static Dictionary<string, string> chemRecipes = new Dictionary<string, string>()
    {
        { "Recipe 1", "K + H2O" },
        { "Recipe 2", "HCl + NH3" },
        { "Recipe 3", "H2O2 + NaI" },
        { "Recipe 4", "C2H3NaO2 + H2O" },
        { "Recipe 5", "KI + H2O2 + C18H35NaO2" }
    };
}

public class ChemidexHandler : MonoBehaviour
{
    public static ElementData elementData; 
    void Start()
    {
        elementData = new ElementData();
        var playerData = PlayerData.Instance();
        Chemidex.chemNum = "1";
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
    public void Recipes()
    {
        SceneManager.LoadScene("Recipes");
    }
    public void periodicTable()
    {
        SceneManager.LoadScene("Periodic Table");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main");
    }
    public void Back()
    {
        SceneManager.LoadScene("Chemidex");
    }
    public void Chem1()
    {
        chemButton("1");
    }
    public void Chem2()
    {
        chemButton("2");
    }
    public void Chem3()
    {
        chemButton("3");
    }
    public void Chem4()
    {
        chemButton("4");
    }
    public void Chem5()
    {
        chemButton("5");
    }
    void chemButton(string Num)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.chemNum != Num)
        {
            GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = Num;
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
