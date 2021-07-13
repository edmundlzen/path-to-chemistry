using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class levelData
{
    public static int Level { get; set; }
}
public class Levels : MonoBehaviour
{
    void Start()
    {
        levelData.Level = 1;
    }
    void Update()
    {
        if (levelData.Level == 1)
        {
            GameObject.Find("Textkia").GetComponent<TextMesh>().text = "K + H2O";
        }
    }
}
