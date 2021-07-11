using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Data
{
    public static string raycastObject { get; set; }
    public static List<string> Elements { get; set; }
}

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject explosionEffect;
    float xRotation = 0f;
    void Start()
    {
        Data.Elements = new List<string>();
        //Cursor.lockState = CursorLockMode.Locked;
        Data.raycastObject = "";
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            var potion = GameObject.Find("Flask").GetComponent<Transform>();
            Data.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = Data.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (Data.raycastObject == "Placement"))
            {
                if (Variable.hotbarData[$"Slot{Variable.Hotbar}"] != "")
                {
                    Data.Elements.Add(Variable.hotbarData[$"Slot{Variable.Hotbar}"]);
                    Variable.hotbarData[$"Slot{Variable.Hotbar}"] = "";
                    for (int i = 1; i <= 9; i = i + 1)
                    {
                        GameObject.Find($"Text{i}").GetComponent<Text>().text = Variable.hotbarData[$"Slot{i}"];
                    }
                }
            }
            if ((Input.GetMouseButtonDown(0)) && (Data.raycastObject == "Reaction"))
            {
                if ((Data.Elements.Contains("K") && (Data.Elements.Contains("H2O"))))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                    Instantiate(explosionEffect, potion.position, potion.rotation);
                    Destroy(GameObject.Find("Flask"));
                    Destroy(GameObject.Find("Reaction"));
                    Destroy(GameObject.Find("Placement"));
                }
                else
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Nothing Happened!";
                }
            }
        }
    }
}
