using System;
using UnityEngine;

public class CraftingTable : MonoBehaviour, IUsable
{
    private bool changeView = false;
    public void Use(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("inventory");
    }
}