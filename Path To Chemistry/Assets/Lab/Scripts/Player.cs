using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class player
{
    public static string raycastObject { get; set; }
    public static Dictionary<string, int> moleculeCount { get; set; }
}

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject explosionEffect;
    public GameObject smokeEffect;
    float xRotation = 0f;
    void Start()
    {
        //Load();
        var playerData = PlayerData.Instance();
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
        player.raycastObject = "";
        player.moleculeCount = new Dictionary<string, int>();
        //Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        var playerData = PlayerData.Instance();
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            var selection = hit.transform;
            player.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = player.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Add"))
            {
                if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null) && (playerData.flaskElements.Count <= 10))
                {
                    if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                    {
                        playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                    }
                    else
                    {
                        playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
                    }
                    if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
                    {
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - 1;
                    }
                    else
                    {
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                    }
                    slotCheck();
                    flaskCheck();
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Add2"))
            {
                if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
                {
                    playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString());
                    if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
                    {
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - 1;
                    }
                    else
                    {
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                    }
                    slotCheck();
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "React") && (GameObject.Find("Flask") != null))
            {
                var potion = GameObject.Find("Flask").GetComponent<Transform>();
                var flaskMouth = GameObject.Find("Flask Mouth").GetComponent<Transform>();
                if (playerData.levelAvailable.Contains("Level 1"))
                {
                    if ((playerData.flaskElements.ContainsKey("K") && (playerData.flaskElements.ContainsKey("Water")) && (playerData.flaskElements.Count == 2)))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                        Instantiate(explosionEffect, potion.position, potion.rotation);
                        updateLevel();
                    }
                }
                if (playerData.levelAvailable.Contains("Level 2"))
                {
                    if ((playerData.flaskElements.ContainsKey("Hydrochloric Acid")) && (playerData.flaskElements.ContainsKey("Ammonia")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Smoke!";
                        Instantiate(smokeEffect, flaskMouth.position, flaskMouth.rotation);
                        updateLevel();
                    }
                }
                if (playerData.levelAvailable.Contains("Level 3"))
                {
                    if ((playerData.flaskElements.ContainsKey("Hydrogen Peroxide")) && (playerData.flaskElements.ContainsKey("Sodium Iodide")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Splash!";
                        updateLevel();
                    }
                }
                if (playerData.levelAvailable.Contains("Level 4"))
                {
                    if ((playerData.flaskElements.ContainsKey("Sodium Acetate")) && (playerData.flaskElements.ContainsKey("Water")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Hot Ice";
                        updateLevel();
                    }
                }
                if (playerData.levelAvailable.Contains("Level 5"))
                {
                    if ((playerData.flaskElements.ContainsKey("Potassium Iodide")) && (playerData.flaskElements.ContainsKey("Hydrogen Peroxide")) && (playerData.flaskElements.ContainsKey("Soup")) && (playerData.flaskElements.Count == 3))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Elephant Toothpaste";
                        updateLevel();
                    }
                }
                else
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Nothing Happened!";
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Craft"))
            {
                foreach (string Item in playerData.Molecule)
                {
                    if (!player.moleculeCount.ContainsKey(Item))
                    {
                        player.moleculeCount.Add(Item, 1);
                    }
                    else
                    {
                        int Count = 0;
                        player.moleculeCount.TryGetValue(Item, out Count);
                        player.moleculeCount.Remove(Item);
                        player.moleculeCount.Add(Item, Count + 1);
                    }
                }
                if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["H"] == 2) && (player.moleculeCount["O"] == 1))
                    {
                        Product("Water");
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["Cl"] == 1))
                    {
                        Product("Salt");
                    }
                }
                else if ((playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["N"] == 1) && (player.moleculeCount["H"] == 3))
                    {
                        Product("Ammonia");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 7) && (player.moleculeCount["H"] == 4) && (player.moleculeCount["O"] == 1))
                    {
                        Product("Charcoal");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 5) && (player.moleculeCount["H"] == 5) && (player.moleculeCount["N"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Glue");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 8) && (player.moleculeCount["H"] == 7) && (player.moleculeCount["N"] == 3) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Luminol");
                    }
                }
                else if ((playerData.Molecule.Contains("Fe")) && (playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["Fe"] == 1) && (player.moleculeCount["S"] == 1) && (player.moleculeCount["O"] == 4))
                    {
                        Product("Ink");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["C"] == 5) && (player.moleculeCount["H"] == 8))
                    {
                        Product("Latex");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["C"] == 9) && (player.moleculeCount["H"] == 20))
                    {
                        Product("Crude Oil");
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 1) && (player.moleculeCount["H"] == 1))
                    {
                        Product("Lye");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 18) && (player.moleculeCount["H"] == 35) && (player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Soap");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 6) && (player.moleculeCount["H"] == 12) && (player.moleculeCount["O"] == 6))
                    {
                        Product("Sugar");
                    }
                }
                else if ((playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["S"] == 1) && (player.moleculeCount["O"] == 4))
                    {
                        Product("Sulfate");
                    }
                }
                else if ((playerData.Molecule.Contains("K")) && (playerData.Molecule.Contains("I")))
                {
                    if ((player.moleculeCount["K"] == 1) && (player.moleculeCount["I"] == 1))
                    {
                        Product("Potassium Iodide");
                    }
                }
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["H"] == 2) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Hydrogen Peroxide");
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 2) && (player.moleculeCount["H"] == 3) && (player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Sodium Acetate");
                    }
                }
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((player.moleculeCount["H"] == 1) && (player.moleculeCount["Cl"] == 1))
                    {
                        Product("Hydrochloric Acid");
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("I")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["I"] == 1))
                    {
                        Product("Sodium Iodide");
                    }
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Left Chair") && playerData.Seat != "Left")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(5f, 0.6f, 6.2f);
                playerData.Seat = "Left";
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Main Chair") && playerData.Seat != "Main")
            {
                GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(7.5f, 0.6f, 6.2f);
                playerData.Seat = "Main";
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Right Chair") && playerData.Seat != "Right")
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
    void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            if ((playerData.slotItem[$"Slot{i}"]["Element"] != null) && (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
            else if ((playerData.slotItem[$"Slot{i}"]["Element"] != null) && (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if ((playerData.slotItem[$"Slot{i}"]["Element"] == null) && (playerData.slotItem[$"Slot{i}"]["Quantity"] == null))
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = "";
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
        }
    }
    void Product(string molecule)
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == molecule)
            {
                playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                break;
            }
            else if ((playerData.slotItem[$"Slot{i}"]["Element"] == null) && (playerData.slotItem[$"Slot{i}"]["Quantity"] == null))
            {
                playerData.slotItem[$"Slot{i}"]["Element"] = molecule;
                playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                break;
            }
        }
        slotCheck();
        playerData.Molecule.Clear();
        player.moleculeCount.Clear();
    }
    void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Item{i}").GetComponent<Text>().text = "";
            GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.flaskElements.Count; i = i + 1)
        {
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text = playerData.flaskElements.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
            }
        }
    }
    void updateLevel()
    {
        var playerData = PlayerData.Instance();
        playerData.flaskElements.Clear();
        playerData.Level += 1;
        playerData.levelAvailable.Add($"Level {playerData.Level}");
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        flaskCheck();
    }
    void Save()
    {
        var playerData = PlayerData.Instance();
        string directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }
    void Load()
    {
        string directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }
}
