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
        
        mainGUIController.UpdateGameviewHotbar();
    }
}
