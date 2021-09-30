using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Task gay;
    public int Target = 30;
    public int Fill = 0;
    private void Start()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = Fill;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gay = new Task(Countdown());
            gay.Start();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            gay.Stop();
        }
    }

    private IEnumerator Countdown()
    {
        Fill = 0;
        for (int i = 1; i <= Target; i++)
        {
            yield return new WaitForSeconds(1);
            Fill += 1;
            GameObject.Find("Text").GetComponent<Text>().text = Fill.ToString();
            GameObject.Find("Slider").GetComponent<Slider>().value = Fill;
        }
        print("Hello");
    }
}
