using UnityEngine;
using UnityEngine.SceneManagement;

public class ChemidexHandler : MonoBehaviour
{
    public void Recipes()
    {
        SceneManager.LoadScene("Recipes");
    }
    public void periodicTable()
    {
        SceneManager.LoadScene("Periodic Table");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
