using UnityEngine;
using UnityEngine.UI;

public class ChemidexHandler : MonoBehaviour
{
    public static PlayerData playerData;
    void Start()
    {
        playerData = new PlayerData();
        GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
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
        if (playerData.chemNum != "1")
        {
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.white;
            playerData.chemNum = "1";
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem2()
    {
        if (playerData.chemNum != "2")
        {
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.white;
            playerData.chemNum = "2";
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem3()
    {
        if (playerData.chemNum != "3")
        {
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.white;
            playerData.chemNum = "3";
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem4()
    {
        if (playerData.chemNum != "4")
        {
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.white;
            playerData.chemNum = "4";
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    public void Chem5()
    {
        if (playerData.chemNum != "5")
        {
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.white;
            playerData.chemNum = "5";
            GameObject.Find("Chem" + playerData.chemNum).GetComponent<Image>().color = Color.cyan;
            chemCheck();
        }
    }
    void chemCheck()
    {
        if (playerData.levelAvailable.Contains($"Level {playerData.chemNum}"))
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = playerData.chemRecipes[$"Recipe {playerData.chemNum}"];
        }
        else
        {
            GameObject.Find("Statistic").GetComponent<Text>().text = "?";
        }
    }
}
