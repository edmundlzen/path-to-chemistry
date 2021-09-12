using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class AppMode
{
    public static bool hasSwitched = false;
}
public class MainMenu : MonoBehaviour
{
    void Start()
    {
        var togglePos = GameObject.Find("Switcher").GetComponent<Transform>().position;
        if (AppMode.hasSwitched)
        {
            GameObject.Find("Switcher").GetComponent<Transform>().position = new Vector3(togglePos.x + 20, togglePos.y, togglePos.z);
            GameObject.Find("Switcher").GetComponent<Image>().color = Color.green;
        }
        else
        {
            GameObject.Find("Switcher").GetComponent<Image>().color = Color.red;
        }
    }
    public void Play()
    {
        if (!AppMode.hasSwitched)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("Quiz");
        }
    }
    public void Lab()
    {
        if (!AppMode.hasSwitched)
        {
            SceneManager.LoadScene("3D Lab");
        }
        else
        {
            SceneManager.LoadScene("Element Constructor");
        }
    }
    public void Chemidex()
    {
        SceneManager.LoadScene("Chemidex");
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void Switcher()
    {
        var togglePos = GameObject.Find("Switcher").GetComponent<Transform>().position;
        if (AppMode.hasSwitched)
        {
            GameObject.Find("Switcher").GetComponent<Transform>().position = new Vector3(togglePos.x - 100, togglePos.y, togglePos.z);
            GameObject.Find("Switcher").GetComponent<Image>().color = Color.red;
            AppMode.hasSwitched = false;
        }
        else
        {
            GameObject.Find("Switcher").GetComponent<Transform>().position = new Vector3(togglePos.x + 100, togglePos.y, togglePos.z);
            GameObject.Find("Switcher").GetComponent<Image>().color = Color.green;
            AppMode.hasSwitched = true;
        }
    }
}