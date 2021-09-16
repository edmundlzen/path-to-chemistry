using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public static class player
{
    public static string raycastObject = "";
    public static bool hasAnimated = true;
}

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation;

    private void Start()
    {
        /*
        Load();
        if (playerData.Seat == "Left")
        {
            GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(5f, 0.6f, 6.2f);
        }
        else if (playerData.Seat == "Main")
        {
            GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(7.5f, 0.6f, 6.2f);
        }
        else if (playerData.Seat == "Right")
        {
            GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(9.79f, 0.6f, 6.2f);
        }
        Cursor.lockState = CursorLockMode.Locked;
        */
    }
    private void Update()
    {
        var playerData = PlayerData.Instance();
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            var selection = hit.transform;
            player.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = player.raycastObject;
            if (Input.GetMouseButtonDown(0) && player.raycastObject == "Left Chair" && playerData.Seat != "Left")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(5f, 0.6f, 6.2f);
                playerData.Seat = "Left";
            }
            else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Main Chair" && playerData.Seat != "Main")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(7.5f, 0.6f, 6.2f);
                playerData.Seat = "Main";
            }
            else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Right Chair" && playerData.Seat != "Right")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(9.79f, 0.6f, 6.2f);
                playerData.Seat = "Right";
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }
    }
    private void Save()
    {
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    private void Load()
    {
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }
}