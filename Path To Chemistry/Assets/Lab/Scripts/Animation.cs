using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class AnimeData
{
    public static int animeNum;
}

public class Animation : MonoBehaviour
{
    public GameObject Effect1;
    public GameObject Effect2;
    public GameObject Effect3;
    public GameObject Effect4;
    public GameObject Effect5;
    public GameObject roundFlask;
    public Animator animator;

    private void Update()
    {
        if (!player.hasAnimated)
        {
            StartCoroutine(pourExplode());
            player.hasAnimated = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AnimeData.animeNum = 1;
            StartCoroutine(pourExplode());
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            AnimeData.animeNum = 2;
            StartCoroutine(pourExplode());
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            AnimeData.animeNum = 3;
            StartCoroutine(pourExplode());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AnimeData.animeNum = 4;
            StartCoroutine(pourExplode());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AnimeData.animeNum = 5;
            StartCoroutine(pourExplode());
        }
    }
    private IEnumerator pourExplode()
    {
        LabData.isAnimationPlaying = true;
        var Potion = GameObject.Find("Effect").GetComponent<Transform>();
        roundFlask.SetActive(true);
        animator.SetTrigger("Pour");
        GameObject.Find("Experiments").GetComponent<Animator>().SetTrigger("Fill");
        yield return new WaitForSeconds(5);
        if (AnimeData.animeNum <= 5)
        {
            if (AnimeData.animeNum == 1)
            {
                Instantiate(Effect1, Potion.position, Potion.rotation);
            }
            else if (AnimeData.animeNum == 2)
            {
                Instantiate(Effect2, Potion.position, Potion.rotation);
            }
            else if (AnimeData.animeNum == 3)
            {
                Instantiate(Effect3, Potion.position, Potion.rotation);
            }
            else if (AnimeData.animeNum == 4)
            {
                Instantiate(Effect4, Potion.position, Potion.rotation);
            }
            else if (AnimeData.animeNum == 5)
            {
                Instantiate(Effect5, Potion.position, Potion.rotation);
            }
        }
        else
        {
            addAlert("Animation Under Construction!");
        }
        roundFlask.SetActive(false);
        LabData.isAnimationPlaying = false;

    }

    private void addAlert(string Alert)
    {
        int maxQuantity = 50;
        player.History.Add(Alert);
        GameObject.Find("Red Dot").GetComponent<Image>().color = Color.white;
        if (player.History.Count > maxQuantity)
        {
            player.History.RemoveAt(0);
        }
    }
}