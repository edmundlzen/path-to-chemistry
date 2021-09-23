using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Molecule : MonoBehaviour
{
    private void Start()
    {
        GameObject.Find(Chemidex.molSlot).GetComponent<Image>().color = Color.cyan;
        GameObject.Find("Statistics").GetComponent<Text>().text = Chemidex.moleculeRecipes[Chemidex.molMode].Values.ElementAt(Convert.ToInt32(Chemidex.molSlot.Replace("Molecule (", "").Replace(")", "")) - 1);
        RecipesCheck();
    }

    public void Back()
    {
        SceneManager.LoadScene("Chemidex");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void RecipesCheck()
    {
        for (int i = 1; i <= Chemidex.moleculeRecipes[Chemidex.molMode].Count; i++)
        {
            GameObject.Find($"Molecule ({i})/Text").GetComponent<Text>().text = Chemidex.moleculeRecipes[Chemidex.molMode].Keys.ElementAt(i - 1);
        }
    }
    
    public void Dropdown(int Value)
    {
        if (Value == 0)
        {
            Chemidex.molMode = "Symbol";
        }
        else if (Value == 1)
        {
            Chemidex.molMode = "Name";
        }
        GameObject.Find("Statistics").GetComponent<Text>().text = Chemidex.moleculeRecipes[Chemidex.molMode].Values.ElementAt(Convert.ToInt32(Chemidex.molSlot.Replace("Molecule (", "").Replace(")", "")) - 1);
        RecipesCheck();
    }

    public void MoleculeSlot()
    {
        var ButtonName = EventSystem.current.currentSelectedGameObject.name;
        if (ButtonName != Chemidex.molSlot)
        {
            GameObject.Find(Chemidex.molSlot).GetComponent<Image>().color = Color.white;
            Chemidex.molSlot = ButtonName;
            GameObject.Find(Chemidex.molSlot).GetComponent<Image>().color = Color.cyan;
            GameObject.Find("Statistics").GetComponent<Text>().text = Chemidex.moleculeRecipes[Chemidex.molMode].Values.ElementAt(Convert.ToInt32(Chemidex.molSlot.Replace("Molecule (", "").Replace(")", "")) - 1);
        }
    }
}
