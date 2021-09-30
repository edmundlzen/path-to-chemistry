using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Chemidex
{
    public static Dictionary<string, string> chemRecipes = new Dictionary<string, string>
    {
        { "Recipe 1", "K + H2O" },
        { "Recipe 2", "HCl + NH3" },
        { "Recipe 3", "NaH + H2O" },
        { "Recipe 4", "HCl + Na2S" },
        { "Recipe 5", "HCl + NaCN" },
        { "Recipe 6", "Na3P + H2O" },
        { "Recipe 7", "NaCl + H2O" },
        { "Recipe 8", "H2O2 + NaI" },
        { "Recipe 9", "HNO3 + N2H4" },
        { "Recipe 10", "AgNO3 + NH3" },
        { "Recipe 11", "HCl + NaClO" },
        { "Recipe 12", "NH3 + NaClO" },
        { "Recipe 13", "IO3 + C3H8O" },
        { "Recipe 14", "HNO3 + C3H8O" },
        { "Recipe 15", "C2H3NaO2 + H2O" },
        { "Recipe 16", "KI + H2O2 + C18H35NaO2" },
    };
    public static Dictionary<string, Dictionary<string, string>> moleculeRecipes = new Dictionary<string, Dictionary<string, string>>()
    {
        {
            "Symbol", new Dictionary<string, string>()
            {
                { "H2O", "H x 2 + O x 1 = H2O" },
                { "NaCl", "Na x 1 + Cl x 1 = NaCl" },
                { "HCl", "H x 1 + Cl x 1 = HCl" },
                { "NH3", "N x 1 + H x 3 = NH3" },
                { "H2O2", "H x 2 + O x 2 = H2O2" },
                { "NaI", "Na x 1 + I x 1 =  NaI" },
                { "Na2S", "Na x 2 + S x 1 = Na2S" },
                { "KI", "K x 1 + I x 1 = KI" },
                { "N2H4", "N x 2 + H x 4 = N2H4" },
                { "AgNO3", "Ag x 1 + N x 1 + O x 3 = AgNO3" },
                { "Na3P", "Na x 3 + P x 1 = Na3P" },
                { "NaH", "Na x 1 + H x 1 = NaH" },
                { "IO3", "I x 1 + O x 3 = IO3" },
                { "C3H8O", "C x 3 + H x 8 + O x 1 = C3H8O" },
                { "HNO3", "H x 1 + N x 1 + O x 3 = HNO3" },
                { "NaClO", "Na x 1 + Cl x 1 + O x 1 = NaClO" },
                { "NaCN", "Na x 1 + C x 1 + N x 1 = NaCN" },
                { "C2H3NaO2", "C x 2 + H x 3 + Na x 1 + O x 2 = C2H3NaO2" },
                { "C18H35NaO2", "C x 18 + H x 35 + Na x 1 + O x 2 = C18H35NaO2" },
            }
        },
        {
            "Name", new Dictionary<string, string>()
            {
                { "Water", "Hydrogen x 2 + Oxygen x 1 = Water" },
                { "Salt", "Sodium x 1 + Chlorine x 1 = Salt" },
                { "Hydrochloric Acid", "Hydrogen x 1 + Chlorine x 1 = Hydrochloric Acid" },
                { "Ammonia", "Nitrogen x 1 + Hydrogen x 3 = Ammonia" },
                { "Hydrogen Peroxide", "Hydrogen x 2 + Oxygen x 2 = Hydrogen Peroxide" },
                { "Sodium Iodide", "Sodium x 1 + Iodine x 1 =  Sodium Iodide" },
                { "Sodium Sulfide", "Sodium x 2 + Sulfur x 1 = Sodium Sulfide" },
                { "Potassium Iodide", "Potassium x 1 + Iodine x 1 = Potassium Iodide" },
                { "Hydrazine", "Nitrogen x 2 + Hydrogen x 4 = Hydrazine" },
                { "Silver Nitrate", "Silver x 1 + Nitrogen x 1 + Oxygen x 3 = Silver Nitrate" },
                { "Sodium Phosphide", "Sodium x 3 + Phosphorus x 1 = Sodium Phosphide" },
                { "Sodium Hydride", "Sodium x 1 + Hydrogen x 1 = Sodium Hydride" },
                { "Iodate", "Iodine x 1 + Oxygen x 3 = Iodate" },
                { "Isopropyl alcohol", "Carbon x 3 + Hydrogen x 8 + Oxygen x 1 = Isopropyl alcohol" },
                { "Nitric Acid", "Hydrogen x 1 + Nitrogen x 1 + Oxygen x 3 = Nitric Acid" },
                { "Sodium Hypochlorite", "Sodium x 1 + Chlorine x 1 + Oxygen x 1 = Sodium Hypochlorite" },
                { "Sodium Cyanide", "Sodium x 1 + Carbon x 1 + Nitrogen x 1 = Sodium Cyanide" },
                { "Sodium Acetate", "Carbon x 2 + Hydrogen x 3 + Sodium x 1 + Oxygen x 2 = Sodium Acetate" },
                { "Soup", "Carbon x 18 + Hydrogen x 35 + Sodium x 1 + Oxygen x 2 = Soup" },
            }
        }
    };
    public static string molMode = "Symbol";
    public static string molSlot = "Molecule (1)";
    public static string chemNum = "1";
}

public class Recipes : MonoBehaviour
{
    private void Start()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find($"Chem ({Chemidex.chemNum})").GetComponent<Image>().color = Color.cyan;
        levellCheck();
        chemCheck();
    }

    public void Back()
    {
        SceneManager.LoadScene(player.startPlace);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Chem()
    {
        var chemName = EventSystem.current.currentSelectedGameObject.name.Replace("Chem (", "").Replace(")", "");
        if (Chemidex.chemNum != chemName)
        {
            GameObject.Find($"Chem ({Chemidex.chemNum})").GetComponent<Image>().color = Color.white;
            Chemidex.chemNum = chemName;
            GameObject.Find($"Chem ({Chemidex.chemNum})").GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }

    private void chemCheck()
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

    private void levellCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 16; i++)
        {
            if (playerData.levelAvailable.Contains($"Level {i}"))
            {
                GameObject.Find($"Chem ({i})/Level").GetComponent<Text>().text = i.ToString();
            }
            else
            {
                GameObject.Find($"Chem ({i})/Level").GetComponent<Text>().text = "?";
            }
        }
    }
}