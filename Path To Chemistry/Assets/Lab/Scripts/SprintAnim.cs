using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SprintAnim : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    private void Start()
    {
        StartCoroutine(Sprint());
    }
    private IEnumerator Sprint()
    {
        animator1.SetTrigger("Eyes");
        animator2.SetTrigger("Head");
        yield return new WaitForSeconds(10);
        animator3.SetTrigger("Sprint");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("3D Lab");
    }
}
