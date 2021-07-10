using UnityEngine;
using UnityEngine.UI;

public static class Data
{
    public static string raycastObject { get; set; }
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
            if ((Input.GetMouseButtonDown(0)) && (Data.raycastObject == "Reaction"))
            {
                GameObject.Find("Label2").GetComponent<Text>().text = "Flask Detected!";
                Instantiate(explosionEffect, potion.position, potion.rotation);
                Destroy(GameObject.Find("Flask"));
                Destroy(GameObject.Find("Reaction"));
            }
            else
            {
                GameObject.Find("Label2").GetComponent<Text>().text = "";
            }
        }
    }
}
