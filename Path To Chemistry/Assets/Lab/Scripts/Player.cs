using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerData jdata;
    float xRotation = 0f;
    void Start()
    {
        print(DateTime.Now.ToString("dd/MM/yyyy"));
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
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            player.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = player.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Add"))
            {
                if (playerData.slotItem[$"Slot{hotbar.slotNum}"] != "")
                {
                    playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]);
                    playerData.slotItem[$"Slot{hotbar.slotNum}"] = "";
                    slotCheck();
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "Add2"))
            {
                if (playerData.slotItem[$"Slot{hotbar.slotNum}"] != "")
                {
                    playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]);
                    playerData.slotItem[$"Slot{hotbar.slotNum}"] = "";
                    for (int i = 1; i <= 9; i = i + 1)
                    {
                        GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
                    }
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (player.raycastObject == "React") && (GameObject.Find("Flask") != null))
            {
                var potion = GameObject.Find("Flask").GetComponent<Transform>();
                var flaskMouth = GameObject.Find("Flask Mouth").GetComponent<Transform>();
                if (playerData.levelAvailable.Contains("Level 1"))
                {
                    if ((playerData.flaskElements.Contains("K") && (playerData.flaskElements.Contains("Water")) && (playerData.flaskElements.Count == 2)))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                        Instantiate(explosionEffect, potion.position, potion.rotation);
                        playerData.flaskElements.Clear();
                        playerData.Level += 1;
                        playerData.levelAvailable.Add($"Level {playerData.Level}");
                    }
                }
                if (playerData.levelAvailable.Contains("Level 2"))
                {
                    if ((playerData.flaskElements.Contains("Hydrochloric Acid")) && (playerData.flaskElements.Contains("Ammonia")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Smoke!";
                        Instantiate(smokeEffect, flaskMouth.position, flaskMouth.rotation);
                        playerData.flaskElements.Clear();
                        playerData.Level += 1;
                        playerData.levelAvailable.Add($"Level {playerData.Level}");
                    }
                }
                if (playerData.levelAvailable.Contains("Level 3"))
                {
                    if ((playerData.flaskElements.Contains("Hydrogen Peroxide")) && (playerData.flaskElements.Contains("Sodium Iodide")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Splash!";
                        playerData.flaskElements.Clear();
                        playerData.Level += 1;
                        playerData.levelAvailable.Add($"Level {playerData.Level}");
                    }
                }
                if (playerData.levelAvailable.Contains("Level 4"))
                {
                    if ((playerData.flaskElements.Contains("Sodium Acetate")) && (playerData.flaskElements.Contains("Water")) && (playerData.flaskElements.Count == 2))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Hot Ice";
                        playerData.flaskElements.Clear();
                        playerData.Level += 1;
                        playerData.levelAvailable.Add($"Level {playerData.Level}");
                    }
                }
                if (playerData.levelAvailable.Contains("Level 5"))
                {
                    if ((playerData.flaskElements.Contains("Potassium Iodide")) && (playerData.flaskElements.Contains("Hydrogen Peroxide")) && (playerData.flaskElements.Contains("Soup")) && (playerData.flaskElements.Count == 3))
                    {
                        GameObject.Find("Label2").GetComponent<Text>().text = "Elephant Toothpaste";
                        playerData.flaskElements.Clear();
                        playerData.Level += 1;
                        playerData.levelAvailable.Add($"Level {playerData.Level}");
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
                foreach (KeyValuePair<string, int> entry in player.moleculeCount)
                {
                    print(entry.Key + entry.Value);
                }
                if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["H"] == 2) && (player.moleculeCount["O"] == 1))
                    {
                        Product("Water");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["Cl"] == 1))
                    {
                        Product("Salt");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["N"] == 1) && (player.moleculeCount["H"] == 3))
                    {
                        Product("Ammonia");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 7) && (player.moleculeCount["H"] == 4) && (player.moleculeCount["O"] == 1))
                    {
                        Product("Charcoal");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 5) && (player.moleculeCount["H"] == 5) && (player.moleculeCount["N"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Glue");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 8) && (player.moleculeCount["H"] == 7) && (player.moleculeCount["N"] == 3) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Luminol");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Fe")) && (playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["Fe"] == 1) && (player.moleculeCount["S"] == 1) && (player.moleculeCount["O"] == 4))
                    {
                        Product("Ink");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["C"] == 5) && (player.moleculeCount["H"] == 8))
                    {
                        Product("Latex");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["C"] == 9) && (player.moleculeCount["H"] == 20))
                    {
                        Product("Crude Oil");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")) && (playerData.Molecule.Contains("H")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 1) && (player.moleculeCount["H"] == 1))
                    {
                        Product("Lye");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 18) && (player.moleculeCount["H"] == 35) && (player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Soap");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 6) && (player.moleculeCount["H"] == 12) && (player.moleculeCount["O"] == 6))
                    {
                        Product("Sugar");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["S"] == 1) && (player.moleculeCount["O"] == 4))
                    {
                        Product("Sulfate");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("K")) && (playerData.Molecule.Contains("I")))
                {
                    if ((player.moleculeCount["K"] == 1) && (player.moleculeCount["I"] == 1))
                    {
                        Product("Potassium Iodide");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["H"] == 2) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Hydrogen Peroxide");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((player.moleculeCount["C"] == 2) && (player.moleculeCount["H"] == 3) && (player.moleculeCount["Na"] == 1) && (player.moleculeCount["O"] == 2))
                    {
                        Product("Sodium Acetate");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((player.moleculeCount["H"] == 1) && (player.moleculeCount["Cl"] == 1))
                    {
                        Product("Hydrochloric Acid");
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("I")))
                {
                    if ((player.moleculeCount["Na"] == 1) && (player.moleculeCount["I"] == 1))
                    {
                        Product("Sodium Iodide");
                        playerData.Molecule.Clear();
                    }
                }
                player.moleculeCount.Clear();
                slotCheck();
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
    public void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
        }
    }
    public void Product(string molecule)
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            if ((playerData.slotItem[$"Slot{i}"] == ""))
            {
                playerData.slotItem[$"Slot{i}"] = molecule;
                break;
            }
        }
    }
    public void Save()
    {
        using (var StrWriter = new StringWriter())
        {
            using (var XmlWriter = new XmlTextWriter(StrWriter))
            {
                var Serializer = new DataContractSerializer(typeof(PlayerData));
                Serializer.WriteObject(XmlWriter, PlayerData.Instance());
                XmlWriter.Flush();
                File.WriteAllText($"{Application.persistentDataPath}/Saves.xml", StrWriter.ToString());
            }
        }
    }
    public void Load()
    {
        if (File.Exists($"{Application.persistentDataPath}/Saves.xml"))
        {
            //PlayerData2 playerData2 = new PlayerData2();
            //var jdata = JsonConvert.SerializeObject(playerData2);
            var Serializer = new DataContractSerializer(typeof(PlayerData));
            var Stream = new FileStream($"{Application.persistentDataPath}/Saves.xml", FileMode.Open);
            //jdata = Serializer.ReadObject(Stream) as PlayerData;
            //PlayerData.LoadData(jdata);
            Stream.Close();
        }
    }
}
