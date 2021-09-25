using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, IUsable
{
    public void Use(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("teleport");
        print("SHIT");
    }
}
