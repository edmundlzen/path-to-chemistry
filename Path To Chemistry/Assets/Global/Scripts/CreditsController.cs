using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void BackButtonHandler()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
