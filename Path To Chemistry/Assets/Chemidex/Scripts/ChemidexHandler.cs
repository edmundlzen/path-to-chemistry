using UnityEngine;
using UnityEngine.UI;

public class ChemidexHandler : MonoBehaviour
{
    void Start()
    {
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
        var playerData = PlayerData.Instance();
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
