using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void Lab()
    {
        SceneManager.LoadScene("3D Lab");
    }
    public void Chemidex()
    {
        SceneManager.LoadScene("Chemidex");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
