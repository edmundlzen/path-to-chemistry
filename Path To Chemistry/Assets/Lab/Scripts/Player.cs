using System.Collections.Generic;
using System.IO;
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
        if (!player.isSaving)
        {
            StartCoroutine(autoSave());
        }
        if (!player.labPause)
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
                player.raycastObject = hit.transform.name;
                if (player.raycastObject == "Experiments")
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Lab/Interactable");
                    GameObject.Find("Label1").GetComponent<Text>().text = "Experiments";
                }
                else if (player.raycastObject == "Element Constructor")
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Lab/Interactable");
                    GameObject.Find("Label1").GetComponent<Text>().text = "Element Constructor";
                }
                else if (player.raycastObject == "Compound Creator & Compound Reducer")
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Lab/Interactable");
                    GameObject.Find("Label1").GetComponent<Text>().text = "Compound Creator & Compound Reducer";
                }
                else
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Lab/Crosshair");
                    GameObject.Find("Label1").GetComponent<Text>().text = "";
                }
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
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
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
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }
}