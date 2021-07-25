using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject explosionEffect;
    public GameObject smokeEffect;
    float xRotation = 0f;
    void Start()
    {
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
            playerData.raycastObject = selection.name;
            GameObject.Find("Label1").GetComponent<Text>().text = playerData.raycastObject;
            if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Add"))
            {
                if (playerData.slotItem[$"Slot{hotbarData.slotNum}"] != "")
                {
                    playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbarData.slotNum}"]);
                    playerData.slotItem[$"Slot{hotbarData.slotNum}"] = "";
                    slotCheck();
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "Add2"))
            {
                if (playerData.slotItem[$"Slot{hotbarData.slotNum}"] != "")
                {
                    playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbarData.slotNum}"]);
                    playerData.slotItem[$"Slot{hotbarData.slotNum}"] = "";
                    for (int i = 1; i <= 9; i = i + 1)
                    {
                        GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
                    }
                }
            }
            else if ((Input.GetMouseButtonDown(0)) && (playerData.raycastObject == "React") && (GameObject.Find("Flask") != null))
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
                else if ((playerData.Molecule.Contains("H")) && (playerData.Molecule.Contains("Cl")))
                {
                    if ((playerData.moleculeCount["H"] == 1) && (playerData.moleculeCount["Cl"] == 1))
                    {
                        playerData.craftedMolecule = "Hydrochloric Acid";
                        playerData.Molecule.Clear();
                    }
                }
                else if ((playerData.Molecule.Contains("Na")) && (playerData.Molecule.Contains("I")))
                {
                    if ((playerData.moleculeCount["Na"] == 1) && (playerData.moleculeCount["I"] == 1))
                    {
                        playerData.craftedMolecule = "Sodium Iodide";
                        playerData.Molecule.Clear();
                    }
                }
                for (int i = 1; i <= 9; i = i + 1)
                {
                    if ((playerData.slotItem[$"Slot{i}"] == ""))
                    {
                        playerData.slotItem[$"Slot{i}"] = playerData.craftedMolecule;
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
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 9; i = i + 1)
        {
            GameObject.Find($"Text{i}").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"];
        }
    }
}
