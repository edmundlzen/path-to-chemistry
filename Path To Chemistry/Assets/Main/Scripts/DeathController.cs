using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        // TODO: Respawn logic
        Time.timeScale = 1f;
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("gameview");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Lab()
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().goBackToDeath = true;
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("teleport");
    }
}
