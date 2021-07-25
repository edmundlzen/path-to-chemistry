using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public static PlayerData playerData;
    void Start()
    {
        playerData = new PlayerData();
    }
    void Update()
    {
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        if (playerData.Level == 1)
        {
            print("In1");
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "K + H2O";
        }
        else if (playerData.Level == 2)
        {
            print("In2");
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "HCl + NH3";
        }
        else if (playerData.Level == 3)
        {
            print("In3");
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
