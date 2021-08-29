using System.Collections;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public GameObject normalFlask;
    public GameObject roundFlask;
    public GameObject Effect;
    public bool flaskDissapear = true;
    private bool hasAnimated;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (!hasAnimated)
            {
                StartCoroutine(pourExplode());
                hasAnimated = true;
            }
    }

    private IEnumerator pourExplode()
    {
        var Potion = GameObject.Find("Effect").GetComponent<Transform>();
        roundFlask.SetActive(true);
        animator1.SetTrigger("Pour");
        yield return new WaitForSeconds(1);
        animator2.SetTrigger("Fill");
        yield return new WaitForSeconds(4);
        Instantiate(Effect, Potion.position, Potion.rotation);
        if (flaskDissapear) normalFlask.SetActive(false);
        roundFlask.SetActive(false);
    }
}