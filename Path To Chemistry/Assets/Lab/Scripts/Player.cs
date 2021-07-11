using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class playerData
{
    public static string raycastObject { get; set; }
    public static List<string> flaskElements { get; set; }
}
public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject explosionEffect;
    float xRotation = 0f;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        playerData.raycastObject = "";
        playerData.flaskElements = new List<string>();
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
            playerData.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = playerData.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Add"))
            {
                if (hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] != "")
                {
                    playerData.flaskElements.Add(hotbarData.slotItem[$"Slot{hotbarData.slotNum}"]);
                    hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] = "";
                    for (int i = 1; i <= 9; i = i + 1)
                    {
                        GameObject.Find($"Text{i}").GetComponent<Text>().text = hotbarData.slotItem[$"Slot{i}"];
                    }
                }
            }
            if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "React"))
            {
                if ((playerData.flaskElements.Contains("K") && (playerData.flaskElements.Contains("H2O")) && (playerData.flaskElements.Count == 2)))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                    Instantiate(explosionEffect, potion.position, potion.rotation);
                    Destroy(GameObject.Find("Flask"));
                    Destroy(GameObject.Find("Add"));
                    Destroy(GameObject.Find("React"));
                }
                else
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Nothing Happened!";
                }
            }
        }
    }
}
