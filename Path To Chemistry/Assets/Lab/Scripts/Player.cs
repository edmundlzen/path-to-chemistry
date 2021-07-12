using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class playerData
{
    public static string raycastObject { get; set; }
    public static List<string> flaskElements { get; set; }
    public static string Seat { get; set; }
    public static List<string> Molecule { get; set; }
    public static Dictionary<string, int> moleculeCount { get; set; }
    public static string moleculeCrafted { get; set; }
    public static bool slotAvailable { get; set; }
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
        playerData.Seat = "Main";
        playerData.Molecule = new List<string>();
        playerData.moleculeCount = new Dictionary<string, int>();
        playerData.moleculeCrafted = "";
        playerData.slotAvailable = true;
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
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Add2"))
            {
                if (hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] != "")
                {
                    playerData.Molecule.Add(hotbarData.slotItem[$"Slot{hotbarData.slotNum}"]);
                    hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] = "";
                    for (int i = 1; i <= 9; i = i + 1)
                    {
                        GameObject.Find($"Text{i}").GetComponent<Text>().text = hotbarData.slotItem[$"Slot{i}"];
                    }
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "React"))
            {
                if ((playerData.flaskElements.Contains("K") && (playerData.flaskElements.Contains("H2O1")) && (playerData.flaskElements.Count == 2)))
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
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Craft"))
            {
                foreach (string item in playerData.Molecule)
                {
                    if (!playerData.moleculeCount.ContainsKey(item))
                    {
                        playerData.moleculeCount.Add(item, 1);
                    }
                    else
                    {
                        int count = 0;
                        playerData.moleculeCount.TryGetValue(item, out count);
                        playerData.moleculeCount.Remove(item);
                        playerData.moleculeCount.Add(item, count + 1);
                    }
                }
                foreach (KeyValuePair<string, int> entry in playerData.moleculeCount)
                {
                    playerData.moleculeCrafted += $"{entry.Key}{entry.Value}";
                }
                for (int i = 1; i <= 9; i = i + 1)
                {
                    if ((hotbarData.slotItem[$"Slot{i}"] == "") && (playerData.slotAvailable == true))
                    {
                        hotbarData.slotItem[$"Slot{i}"] = playerData.moleculeCrafted;
                        playerData.slotAvailable = false;
                    }
                }
                for (int i = 1; i <= 9; i = i + 1)
                {
                    GameObject.Find($"Text{i}").GetComponent<Text>().text = hotbarData.slotItem[$"Slot{i}"];
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Left Chair") && playerData.Seat != "Left")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(5f, 0.6f, 6.3f);
                playerData.Seat = "Left";
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Main Chair") && playerData.Seat != "Main")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(7.5f, 0.6f, 6.3f);
                playerData.Seat = "Main";
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Right Chair") && playerData.Seat != "Right")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(9.79f, 0.6f, 6.3f);
                playerData.Seat = "Right";
            }
        }
    }
}
