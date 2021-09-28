using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private GameObject GUIController;

    void Start()
    {
        GUIController = GameObject.FindGameObjectsWithTag("MainGUIController")[0].gameObject;
    }
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        // TODO: SAVEEEEEE
        SceneManager.LoadScene("Main Menu");
    }
    
    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }
    
    public void Respawn()
    {
        transform.parent.Find("Respawn").GetComponent<RespawnController>().returnTo = "pause";
        GUIController.GetComponent<MainGUIController>().ChangeView("respawn");
    }
}
