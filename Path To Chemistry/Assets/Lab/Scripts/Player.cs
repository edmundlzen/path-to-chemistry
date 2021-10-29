using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class player
{
    public static bool Pause = false;
    public static bool deepPause = false;
    public static bool labPause = false;
    public static bool hasAnimated = true;
    public static bool isSaving = false;
    public static string raycastObject = "";
    public static string startPlace = "";
    public static List<string> History = new List<string>();
}

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation;
    private bool isSaving = false;
    private void Awake()
    {
        isFirstSave();
        loadPlayerData();
        loadElementsData();
    }
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (!isSaving)
        {
            StartCoroutine(autoSave());
        }
        if (!player.labPause)
        {
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
                player.raycastObject = hit.transform.name;
            }
        }
    }
    private IEnumerator autoSave()
    {
        player.isSaving = true;
        yield return new WaitForSeconds(1);
        Save();
        player.isSaving = false;
    }
    private void isFirstSave()
    {
        loadElementsData();
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            var elementData = ElementData.Instance();
            for (int i = 1; i <= 118; i++)
            {
                if (playerData.Inventory.ContainsKey(elementData.elements.Keys.ElementAt(i - 1))) continue;
                playerData.Inventory.Add(elementData.elements.Keys.ElementAt(i - 1), 0);
            }
            playerData.levelAvailable.Add($"Level {playerData.Level}");
            Directory.CreateDirectory(directory);
            var Settings = new JsonSerializerSettings();
            Settings.Formatting = Formatting.Indented;
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var Json = JsonConvert.SerializeObject(playerData, Settings);
            var filePath = Path.Combine(directory, "Saves.json");
            File.WriteAllText(filePath, Json);
        }
    }
    private void Save()
    {
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    private void loadElementsData()
    {
        var elementData = JsonConvert.DeserializeObject<ElementData>(allElements.Data);
        ElementData.Instance().UpdateElementData(elementData);
    }
    private void loadPlayerData()
    {
        print(Application.persistentDataPath);
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }
}