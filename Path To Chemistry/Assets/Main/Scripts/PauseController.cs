using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Lab()
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("teleport");
    }
    
    public void Respawn()
    {
        // Respawn window
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
}
