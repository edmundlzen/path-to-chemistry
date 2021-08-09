using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    void Start()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
    }
    void Update()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Level == 1)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "K + H2O";
        }
        else if (playerData.Level == 2)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "HCl + NH3";
        }
        else if (playerData.Level == 3)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "H2O2 + NaI";
        }
        else if (playerData.Level == 4)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "C2H3NaO2 + H2O";
        }
        else if (playerData.Level == 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "KI + H2O2 + C18H35NaO2";
        }
        else if (playerData.Level > 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "Under Constaruction!";
        }
    }
}
