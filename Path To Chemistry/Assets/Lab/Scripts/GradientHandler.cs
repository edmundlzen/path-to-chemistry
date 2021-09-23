using System.Collections;
using UnityEngine;

public class GradientHandler : MonoBehaviour
{
    float Value = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Value = 1;
        }
        while (Value > 0)
        {
            StartCoroutine(Fade());
        }
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3f);
        Value -= 0.02f;
        GameObject.Find("Image").GetComponent<CanvasGroup>().alpha = Value;
        yield return null;
    }
}