using UnityEngine;
using UnityEngine.SceneManagement;

public class LabHandler : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("pauseMenu").SetActive(false);
    }
    public void Pause(GameObject pauseMenu)
    {
        pauseMenu.SetActive(true);
    }
    public void Resume(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
