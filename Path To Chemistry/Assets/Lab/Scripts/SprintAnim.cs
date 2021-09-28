using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Transition
{
    public static string ToScene;
}

public class SprintAnim : MonoBehaviour
{
    private void Start()
    {
        // Transition.ToScene = "3D Lab"; // Commented out because it interferes with others setting this value.
        StartCoroutine(Sprint());
    }
    private IEnumerator Sprint()
    {
        GameObject.Find("Eyes").GetComponent<Animator>().SetTrigger("Eyes");
        GameObject.Find("Main Camera").GetComponent<Animator>().SetTrigger("Head");
        yield return new WaitForSeconds(10);
        GameObject.Find("sprintEffect").GetComponent<Animator>().SetTrigger("Sprint");
        yield return new WaitForSeconds(5);
        if (Transition.ToScene != null)
        {
            SceneManager.LoadScene(Transition.ToScene);
        }
        else
        {
            print("Please assign a scene to Transition.ToScene or u will stuck in this transition forever :)");
        }
    }
}
