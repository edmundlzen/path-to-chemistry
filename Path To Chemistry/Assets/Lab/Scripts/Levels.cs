using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class levelData
{
    public static int Level { get; set; }
    public static List<string> levelAvailable { get; set; }
}
public class Levels : MonoBehaviour
{
    void Start()
    {
        levelData.Level = 1;
        levelData.levelAvailable = new List<string>();
        levelData.levelAvailable.Add($"Level {levelData.Level}");
    }
    void Update()
    {
        GameObject.Find("Level").GetComponent<Text>().text = levelData.Level.ToString();
        if (levelData.Level == 1)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "K + H2O";
        }
        else if (levelData.Level == 2)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "HCl + NH3";
        }
        else if (levelData.Level == 3)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "H2O2 + NaI";
        }
        else if (levelData.Level == 4)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "C2H3NaO2 + H2O";
        }
        else if (levelData.Level == 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "KI + H2O2 + C18H35NaO2";
        }
        else if (levelData.Level > 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "Under Constaruction!";
        }
    }
}
