using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialReducerItem : MonoBehaviour, IUsable
{
    public void Use(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("materialReducer");
        GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<MainGameManager>().useButton = false;
    }
}
