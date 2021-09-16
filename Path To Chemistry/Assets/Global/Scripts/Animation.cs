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

    private void Update()
    {
        if (!player.hasAnimated)
        {
            StartCoroutine(pourExplode());
            player.hasAnimated = true;
        }
    }
    private IEnumerator pourExplode()
    {
        LabData.isAnimationPlaying = true;
        var Potion = GameObject.Find("Effect").GetComponent<Transform>();
        roundFlask.SetActive(true);
        animator1.SetTrigger("Pour");
        //yield return new WaitForSeconds(1);
        animator2.SetTrigger("Fill");
        yield return new WaitForSeconds(5);
        Instantiate(Effect, Potion.position, Potion.rotation);
        if (flaskDissapear) normalFlask.SetActive(false);
        roundFlask.SetActive(false);
        LabData.isAnimationPlaying = false;
    }
}