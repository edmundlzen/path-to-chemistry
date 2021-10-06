using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackItem : MonoBehaviour, IUsable
{
    public void Use(Transform playerCamera)
    {
        PlayerData.Instance().survivalHealth += 10;
        var survivalInventory = PlayerData.Instance().survivalInventory;
        survivalInventory["Nasi Lemak"]["quantity"] =
            int.Parse(survivalInventory["Nasi Lemak"]["quantity"].ToString()) - 1;    
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().UpdateGameviewHotbar();
    }
}
