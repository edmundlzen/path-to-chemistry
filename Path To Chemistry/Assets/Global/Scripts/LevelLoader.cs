using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator Transition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main Menu");
    }
}