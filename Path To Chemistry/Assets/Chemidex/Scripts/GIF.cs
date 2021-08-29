using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour
{
    public Sprite[] animatedSprites;
    public bool hasAnimated;
    public GameObject UI;

    private void Update()
    {
        if (!hasAnimated)
        {
            UI.SetActive(false);
            GameObject.Find("Panelkia").GetComponent<Image>().sprite =
                animatedSprites[(int) (Time.time * 10) % animatedSprites.Length];
            if ((int) (Time.time * 10) % animatedSprites.Length == animatedSprites.Length - 1) hasAnimated = true;
        }
        else
        {
            UI.SetActive(true);
        }
    }
}