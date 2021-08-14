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
        Time.timeScale = 0;
    }
    public void Resume(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
