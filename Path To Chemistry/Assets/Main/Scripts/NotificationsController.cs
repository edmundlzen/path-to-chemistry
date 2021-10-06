using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationsController : MonoBehaviour
{
    public void SendImageNotification(Sprite image, string text, string redOrGreen)
    {
        new Task(AddImageNotification(image, text, redOrGreen));
    }

    private IEnumerator AddImageNotification(Sprite image, string text, string redOrGreen)
    {
        while (true)
        {
            Color greenColor = new Color32(180, 255, 180, 255);
            Color redColor = new Color32(255, 180, 180, 255);
            GameObject newNotification = Instantiate(Resources.Load<GameObject>("Prefabs/ImageNotification"), transform);
            newNotification.transform.Find("Image").GetComponent<Image>().sprite = image;
            newNotification.transform.GetComponent<Image>().color = redOrGreen == "red" ? redColor : greenColor;
            newNotification.transform.Find("Text").GetComponent<Text>().text = text;
            newNotification.transform.SetParent(transform);

            yield return new WaitForSeconds(3f);
            float newAlpha = 1f;
            while (newNotification.GetComponent<CanvasGroup>().alpha > 0)
            {
                newNotification.GetComponent<CanvasGroup>().alpha = newAlpha;
                newAlpha -= 0.01f;
                yield return new WaitForSeconds(.01f);
            }
            Destroy(newNotification);
            yield break;
        }
    }
    
    public void SendTextImageNotification(string textImage, string text, string redOrGreen)
    {
        new Task(AddTextImageNotification(textImage, text, redOrGreen));
    }

    private IEnumerator AddTextImageNotification(string textImage, string text, string redOrGreen)
    {
        while (true)
        {
            Color greenColor = new Color32(180, 255, 180, 255);
            Color redColor = new Color32(255, 180, 180, 255);
            GameObject newNotification = Instantiate(Resources.Load<GameObject>("Prefabs/TextImageNotification"), transform);
            newNotification.transform.Find("TextImage").GetComponent<Text>().text = textImage;
            newNotification.transform.GetComponent<Image>().color = redOrGreen == "red" ? redColor : greenColor;
            newNotification.transform.Find("Text").GetComponent<Text>().text = text;
            newNotification.transform.SetParent(transform);

            yield return new WaitForSeconds(3f);
            float newAlpha = 1f;
            while (newNotification.GetComponent<CanvasGroup>().alpha > 0)
            {
                newNotification.GetComponent<CanvasGroup>().alpha = newAlpha;
                newAlpha -= 0.01f;
                yield return new WaitForSeconds(.01f);
            }
            Destroy(newNotification);
            yield break;
        }
    }
}
