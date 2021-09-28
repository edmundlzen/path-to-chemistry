using UnityEngine;
using UnityEngine.SceneManagement;

public class ChemidexHandler : MonoBehaviour
{
    public void experimentsRecipes()
    {
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Recipes");
    }

    public void periodicTable()
    {
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Periodic Table");
    }

    public void compoundRecipes()
    {
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Molecule Recipes");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}