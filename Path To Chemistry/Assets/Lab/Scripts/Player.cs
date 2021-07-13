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
    public static string craftedMolecule { get; set; }
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
        playerData.craftedMolecule = "";
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
            playerData.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = playerData.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Add"))
            {
                if (hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] != "")
                {
                    playerData.flaskElements.Add(hotbarData.slotItem[$"Slot{hotbarData.slotNum}"]);
                    hotbarData.slotItem[$"Slot{hotbarData.slotNum}"] = "";
                    slotCheck();
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
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "React") && (GameObject.Find("Flask") != null))
            {
                var potion = GameObject.Find("Flask").GetComponent<Transform>();
                if ((playerData.flaskElements.Contains("K") && (playerData.flaskElements.Contains("Water")) && (playerData.flaskElements.Count == 2)))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                    Instantiate(explosionEffect, potion.position, potion.rotation);
                    Destroy(GameObject.Find("Flask"));
                    Destroy(GameObject.Find("Add"));
                    Destroy(GameObject.Find("React"));
                }
                else if ((playerData.flaskElements.Contains("Potassium Iodide")) && (playerData.flaskElements.Contains("Hydrogen Peroxide")) && (playerData.flaskElements.Contains("Soup")) && (playerData.flaskElements.Count == 3))
                {
                    print("Elephant Toothpaste!");
                }
                else if ((playerData.flaskElements.Contains("Sodium Acetate")) && (playerData.flaskElements.Contains("Water")) && (playerData.flaskElements.Count == 2))
                {
                    print("Hot Ice");
                }
                else
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Nothing Happened!";
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Craft"))
            {
                foreach (string Item in playerData.Molecule)
                {
                    if (!playerData.moleculeCount.ContainsKey(Item))
                    {
                        playerData.moleculeCount.Add(Item, 1);
                    }
                    else
                    {
                        int Count = 0;
                        playerData.moleculeCount.TryGetValue(Item, out Count);
                        playerData.moleculeCount.Remove(Item);
                        playerData.moleculeCount.Add(Item, Count + 1);
                    }
                }
                foreach (KeyValuePair<string, int> entry in playerData.moleculeCount)
                {
                    print(entry.Key + entry.Value);
                }
                if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["H"] == 2) && (playerData.moleculeCount["O"] == 1))
                    {
                        playerData.craftedMolecule = "Water";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((playerData.moleculeCount["Na"] == 1) && (playerData.moleculeCount["Cl"] == 1))
                    {
                        playerData.craftedMolecule = "Salt";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("H")))
                {
                    if ((playerData.moleculeCount["N"] == 1) && (playerData.moleculeCount["H"] == 3))
                    {
                        playerData.craftedMolecule = "Ammonia";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 7) && (playerData.moleculeCount["H"] == 4) && (playerData.moleculeCount["O"] == 1))
                    {
                        playerData.craftedMolecule = "Charcoal";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 5) && (playerData.moleculeCount["H"] == 5) && (playerData.moleculeCount["N"] == 1) && (playerData.moleculeCount["O"] == 2))
                    {
                        playerData.craftedMolecule = "Glue";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("N")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 8) && (playerData.moleculeCount["H"] == 7) && (playerData.moleculeCount["N"] == 3) && (playerData.moleculeCount["O"] == 2))
                    {
                        playerData.craftedMolecule = "Luminol";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Fe")) && (playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["Fe"] == 1) && (playerData.moleculeCount["S"] == 1) && (playerData.moleculeCount["O"] == 4))
                    {
                        playerData.craftedMolecule = "Ink";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((playerData.moleculeCount["C"] == 5) && (playerData.moleculeCount["H"] == 8))
                    {
                        playerData.craftedMolecule = "Latex";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")))
                {
                    if ((playerData.moleculeCount["C"] == 9) && (playerData.moleculeCount["H"] == 20))
                    {
                        playerData.craftedMolecule = "Crude Oil";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")) && (playerData.Molecule.Contains("H")))
                {
                    if ((playerData.moleculeCount["Na"] == 1) && (playerData.moleculeCount["O"] == 1) && (playerData.moleculeCount["H"] == 1))
                    {
                        playerData.craftedMolecule = "Lye";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 18) && (playerData.moleculeCount["H"] == 35) && (playerData.moleculeCount["Na"] == 1) && (playerData.moleculeCount["O"] == 2))
                    {
                        playerData.craftedMolecule = "Soup";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 6) && (playerData.moleculeCount["H"] == 12) && (playerData.moleculeCount["O"] == 6))
                    {
                        playerData.craftedMolecule = "Sugar";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("S")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["S"] == 1) && (playerData.moleculeCount["O"] == 4))
                    {
                        playerData.craftedMolecule = "Sulfate";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("K")) && (playerData.Molecule.Contains("I")))
                {
                    if ((playerData.moleculeCount["K"] == 1) && (playerData.moleculeCount["I"] == 1))
                    {
                        playerData.craftedMolecule = "Potassium Iodide";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["H"] == 2) && (playerData.moleculeCount["O"] == 2))
                    {
                        playerData.craftedMolecule = "Hydrogen Peroxide";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("C")) && (playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("O")))
                {
                    if ((playerData.moleculeCount["C"] == 2) && (playerData.moleculeCount["H"] == 3) && (playerData.moleculeCount["Na"] == 1) && (playerData.moleculeCount["O"] == 2))
                    {
                        playerData.craftedMolecule = "Sodium Acetate";
                        playerData.Molecule.Clear();
                    }
                }
                for (int i = 1; i <= 9; i = i + 1)
                {
                    if ((hotbarData.slotItem[$"Slot{i}"] == ""))
                    {
                        hotbarData.slotItem[$"Slot{i}"] = playerData.craftedMolecule;
                        break;
                    }
                }
                playerData.moleculeCount.Clear();
                slotCheck();
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
    public void slotCheck()
    {
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = hotbarData.slotItem[$"Slot{i}"];
        }
    }
}
