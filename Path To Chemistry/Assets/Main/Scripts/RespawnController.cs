using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class RespawnController : MonoBehaviour
{
    public string activeTerrain;
    public string returnTo;

    private GameObject title;
    private GameObject description;
    private GameObject player;
    private Transform teleports;
    private Transform GUIController;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void Start()
    {
        title = transform.Find("Title").gameObject;
        description = transform.Find("Terrain Description").gameObject;
        player = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
        teleports = GameObject.FindGameObjectsWithTag("Teleports")[0].transform;
        GUIController = GameObject.FindGameObjectsWithTag("MainGUIController")[0].transform;
    }

    public void TerrainClickHandler(Transform terrain)
    {
        if (activeTerrain != "")
        {
            print(activeTerrain);
            transform.Find("Terrain Image").Find(activeTerrain).GetComponent<Image>().color = new Color32(255, 0, 0, 0);
        }
        terrain.GetComponent<Image>().color = new Color32(255, 0, 0, 50);
        activeTerrain = terrain.name;

        var terrains = PlayerData.Instance().terrains;
        title.GetComponent<Text>().text = terrain.name;
        description.GetComponent<Text>().text = terrains[terrain.name]["description"].ToString();

        // switch (terrain.name)
        // {
        //     case "Forest":
        //         break;
        //     case "Swamp Forest":
        //         break;
        //     case "Savanna":
        //         break;
        //     case "Beach":
        //         break;
        //     case "Grassland Tundra":
        //         break;
        //     case "Desert 1":
        //         break;
        //     case "Snowy Tundra":
        //         break;
        //     case "Highland Tundra":
        //         break;
        //     case "Desert 2":
        //         break;
        // }
    }

    public void SpawnButtonHandler()
    {
        if (activeTerrain == "") return;
        player.transform.position = teleports.Find(activeTerrain).position;
        GUIController.GetComponent<MainGUIController>().ChangeView("gameview");
    }

    public void CloseButtonHandler()
    {
        GUIController.GetComponent<MainGUIController>().ChangeView(returnTo);
    }
}
