using Newtonsoft.Json;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class player
{
    public static string raycastObject { get; set; }
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
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        Guide();
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
                GameObject.Find("Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Add2"))
            {
                if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
                {
                    if (playerData.Molecule.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                    {
                        playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                    }
                    else
                    {
                        playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
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
                    craftingTable();
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "On"))
            {

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
                if ((playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["H"] == 2) && (playerData.Molecule["O"] == 1))
                    {
                        Product("Water");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("Na")) && (playerData.Molecule.ContainsKey("Cl")))
                {
                    if ((playerData.Molecule["Na"] == 1) && (playerData.Molecule["Cl"] == 1))
                    {
                        Product("Salt");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("N")) && (playerData.Molecule.ContainsKey("H")))
                {
                    if ((playerData.Molecule["N"] == 1) && (playerData.Molecule["H"] == 3))
                    {
                        Product("Ammonia");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 7) && (playerData.Molecule["H"] == 4) && (playerData.Molecule["O"] == 1))
                    {
                        Product("Charcoal");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("N")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 5) && (playerData.Molecule["H"] == 5) && (playerData.Molecule["N"] == 1) && (playerData.Molecule["O"] == 2))
                    {
                        Product("Glue");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("N")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 8) && (playerData.Molecule["H"] == 7) && (playerData.Molecule["N"] == 3) && (playerData.Molecule["O"] == 2))
                    {
                        Product("Luminol");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("Fe")) && (playerData.Molecule.ContainsKey("S")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["Fe"] == 1) && (playerData.Molecule["S"] == 1) && (playerData.Molecule["O"] == 4))
                    {
                        Product("Ink");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")))
                {
                    if ((playerData.Molecule["C"] == 5) && (playerData.Molecule["H"] == 8))
                    {
                        Product("Latex");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")))
                {
                    if ((playerData.Molecule["C"] == 9) && (playerData.Molecule["H"] == 20))
                    {
                        Product("Crude Oil");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("Na")) && (playerData.Molecule.ContainsKey("O")) && (playerData.Molecule.ContainsKey("H")))
                {
                    if ((playerData.Molecule["Na"] == 1) && (playerData.Molecule["O"] == 1) && (playerData.Molecule["H"] == 1))
                    {
                        Product("Lye");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("Na")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 18) && (playerData.Molecule["H"] == 35) && (playerData.Molecule["Na"] == 1) && (playerData.Molecule["O"] == 2))
                    {
                        Product("Soap");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 6) && (playerData.Molecule["H"] == 12) && (playerData.Molecule["O"] == 6))
                    {
                        Product("Sugar");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("S")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["S"] == 1) && (playerData.Molecule["O"] == 4))
                    {
                        Product("Sulfate");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("K")) && (playerData.Molecule.ContainsKey("I")))
                {
                    if ((playerData.Molecule["K"] == 1) && (playerData.Molecule["I"] == 1))
                    {
                        Product("Potassium Iodide");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["H"] == 2) && (playerData.Molecule["O"] == 2))
                    {
                        Product("Hydrogen Peroxide");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("C")) && (playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("Na")) && (playerData.Molecule.ContainsKey("O")))
                {
                    if ((playerData.Molecule["C"] == 2) && (playerData.Molecule["H"] == 3) && (playerData.Molecule["Na"] == 1) && (playerData.Molecule["O"] == 2))
                    {
                        Product("Sodium Acetate");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("H")) && (playerData.Molecule.ContainsKey("Cl")))
                {
                    if ((playerData.Molecule["H"] == 1) && (playerData.Molecule["Cl"] == 1))
                    {
                        Product("Hydrochloric Acid");
                    }
                }
                else if ((playerData.Molecule.ContainsKey("Na")) && (playerData.Molecule.ContainsKey("I")))
                {
                    if ((playerData.Molecule["Na"] == 1) && (playerData.Molecule["I"] == 1))
                    {
                        Product("Sodium Iodide");
                    }
                }
                craftingTable();
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
    void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Element{i}").GetComponent<Text>().text = "";
            GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.Molecule.Count; i = i + 1)
        {
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Element{i}").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = playerData.Molecule.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Element{i}").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Elementnum{i}").GetComponent<Text>().text = "";
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
        Guide();
        flaskCheck();
    }
    void Guide()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Level <= 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = Chemidex.chemRecipes[$"Recipe {playerData.Level}"];
        }
        else if (playerData.Level > 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "Under Constaruction!";
        }
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
