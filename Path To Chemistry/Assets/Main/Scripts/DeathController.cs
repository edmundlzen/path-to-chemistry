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
        transform.parent.Find("Respawn").GetComponent<RespawnController>().returnTo = "death";
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("respawn");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Lab()
    {
        transform.parent.Find("Teleport").GetComponent<TeleportController>().returnTo = "death";
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("teleport");
    }
}
