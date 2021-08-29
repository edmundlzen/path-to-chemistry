using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Chemidex
{
    public static Dictionary<string, string> chemRecipes = new Dictionary<string, string>
    {
        {"Recipe 1", "K + H2O"},
        {"Recipe 2", "HCl + NH3"},
        {"Recipe 3", "H2O2 + NaI"},
        {"Recipe 4", "C2H3NaO2 + H2O"},
        {"Recipe 5", "KI + H2O2 + C18H35NaO2"}
    };

    public static string chemNum { get; set; }
}

public class Recipes : MonoBehaviour
{
    private void Start()
    {
        var playerData = PlayerData.Instance();
        Chemidex.chemNum = "1";
        GameObject.Find("Chem" + Chemidex.chemNum).GetComponent<Image>().color = Color.cyan;
        levellCheck();
        chemCheck();
    }

    public void Back()
    {
        SceneManager.LoadScene("Chemidex");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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

    private void chemButton(string Num)
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

    private void chemCheck()
    {
        var playerData = PlayerData.Instance();
        if (playerData.levelAvailable.Contains($"Level {Chemidex.chemNum}"))
            GameObject.Find("Statistic").GetComponent<Text>().text = Chemidex.chemRecipes[$"Recipe {Chemidex.chemNum}"];
        else
            GameObject.Find("Statistic").GetComponent<Text>().text = "?";
    }

    private void levellCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 5; i++)
            if (playerData.levelAvailable.Contains($"Level {i}"))
                GameObject.Find($"Text{i}").GetComponent<Text>().text = i.ToString();
            else
                GameObject.Find($"Text{i}").GetComponent<Text>().text = "?";
    }
}