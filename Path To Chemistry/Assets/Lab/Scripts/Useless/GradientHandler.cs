using System.Collections;
using UnityEngine;

public class GradientHandler : MonoBehaviour
{
    float Value = 1;
    Task fade;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            fade = new Task(Fade());
            fade.Stop();
            fade.Start();
        }
    }
    IEnumerator Fade()
    {
        Value = 1;
        yield return new WaitForSeconds(5f);
        while (Value > 0)
        {
            Value -= 0.02f;
            GameObject.Find("Image").GetComponent<CanvasGroup>().alpha = Value;
            yield return null;
        }
    }
}