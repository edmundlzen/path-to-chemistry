using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterItem : MonoBehaviour, IUsable
{
    public void Use(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("teleport");
        GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<MainGameManager>().useButton = false;
    }
}
