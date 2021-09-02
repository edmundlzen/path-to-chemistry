using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    private MainGUIController mainGUIController;

    void Awake()
    {
        mainGUIController = GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>();
    }
    
    private void Load()
    {
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }

    private void Save()
    {
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    
    public void Use(Transform playerCamera)
    {
        var playerData = PlayerData.Instance();

        playerData.survivalInventory[transform.name]["quantity"] =
            Int32.Parse(playerData.survivalInventory[transform.name]["quantity"].ToString()) - 1;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/P " + transform.name), hit.point, Quaternion.identity);
        }
        // Save()
        mainGUIController.UpdateGameviewHotbar();
    }
}
