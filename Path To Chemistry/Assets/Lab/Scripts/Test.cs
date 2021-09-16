using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("Alert").GetComponent<Text>().text = "Test";
            GameObject.Find("Alert").GetComponent<Animator>().SetTrigger("Alert");
            return;
        }
    }
    public void one(GameObject Image)
    {
        Image.SetActive(false);
        Time.timeScale = 1;
    }
    public void two(GameObject Image)
    {
        Image.SetActive(true);
        Time.timeScale = 0;
    }
}
