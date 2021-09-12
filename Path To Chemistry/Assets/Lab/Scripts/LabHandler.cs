using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject identifyQuantity;
    public GameObject craftQuantity;
    public GameObject flaskHotbar;
    public GameObject craftingTableHotbar;
    public GameObject elementConstructorPanel;
    private void Start()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        Guide();
    }
    private void Update()
    {
        var playerData = PlayerData.Instance();
        if (Input.GetMouseButtonDown(0) && player.raycastObject == "Experiment")
        {
            flaskHotbar.SetActive(true);
            Time.timeScale = 0;
            flaskCheck();
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Add2")
        {
            craftingTableHotbar.SetActive(true);
            Time.timeScale = 0;
            craftingTable();
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Add3")
        {
            elementConstructorPanel.SetActive(true);
            Time.timeScale = 0;
            elementConstructor.Protons = 0;
            elementConstructor.Electrons = 0;
            elementConstructor.Neutrons = 0;
            GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
            GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
            GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }
    public void addFlaskQuantity()
    {
        if (GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void addCraftQuantity()
    {
        if (GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeFlaskQuantity()
    {
        if (GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value -= 1;
        }
    }
    public void removeCraftQuantity()
    {
        if (GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value -= 1;
        }
    }
    public void addCraftElement()
    {
        var playerData = PlayerData.Instance();
        if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
        {
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
            {
                craftQuantity.SetActive(true);
                GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
            }
            else
            {
                if (playerData.Molecule.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                }
                else
                {
                    playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                slotCheck();
                craftingTable();
            }
        }
    }
    public void addFlaskElement()
    {
        var playerData = PlayerData.Instance();
        if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
        {
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
            {
                identifyQuantity.SetActive(true);
                GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
            }
            else
            {
                if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                }
                else
                {
                    playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                slotCheck();
                flaskCheck();
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Slider(float Value)
    {
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Mathf.Floor(Value));
    }

    public void Back1()
    {
        elementConstructorPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void placeFlaskQuantity()
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value));
        GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value = 0;
        identifyQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null && playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null && playerData.flaskElements.Count <= 10)
            {
                if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + sliderValue;
                }
                else
                {
                    playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
                if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
                {
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                }
                slotCheck();
                flaskCheck();
            }
        }
    }
    public void placeCraftQuantity()
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value);
        GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value = 0;
        craftQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.Molecule.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
            {
                playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + sliderValue;
            }
            else
            {
                playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
            }
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
            {
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
            }
            slotCheck();
            craftingTable();
        }
    }
    public void Craft()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["H"] == 2 && playerData.Molecule["O"] == 1) Product("Water");
        }
        else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("Cl"))
        {
            if (playerData.Molecule["Na"] == 1 && playerData.Molecule["Cl"] == 1) Product("Salt");
        }
        else if (playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("H"))
        {
            if (playerData.Molecule["N"] == 1 && playerData.Molecule["H"] == 3) Product("Ammonia");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 7 && playerData.Molecule["H"] == 4 && playerData.Molecule["O"] == 1)
                Product("Charcoal");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 5 && playerData.Molecule["H"] == 5 &&
                playerData.Molecule["N"] == 1 && playerData.Molecule["O"] == 2) Product("Glue");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 8 && playerData.Molecule["H"] == 7 &&
                playerData.Molecule["N"] == 3 && playerData.Molecule["O"] == 2) Product("Luminol");
        }
        else if (playerData.Molecule.ContainsKey("Fe") && playerData.Molecule.ContainsKey("S") &&
                 playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["Fe"] == 1 && playerData.Molecule["S"] == 1 &&
                playerData.Molecule["O"] == 4) Product("Ink");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H"))
        {
            if (playerData.Molecule["C"] == 5 && playerData.Molecule["H"] == 8) Product("Latex");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H"))
        {
            if (playerData.Molecule["C"] == 9 && playerData.Molecule["H"] == 20) Product("Crude Oil");
        }
        else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("O") &&
                 playerData.Molecule.ContainsKey("H"))
        {
            if (playerData.Molecule["Na"] == 1 && playerData.Molecule["O"] == 1 &&
                playerData.Molecule["H"] == 1) Product("Lye");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 18 && playerData.Molecule["H"] == 35 &&
                playerData.Molecule["Na"] == 1 && playerData.Molecule["O"] == 2) Product("Soap");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 6 && playerData.Molecule["H"] == 12 &&
                playerData.Molecule["O"] == 6) Product("Sugar");
        }
        else if (playerData.Molecule.ContainsKey("S") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["S"] == 1 && playerData.Molecule["O"] == 4) Product("Sulfate");
        }
        else if (playerData.Molecule.ContainsKey("K") && playerData.Molecule.ContainsKey("I"))
        {
            if (playerData.Molecule["K"] == 1 && playerData.Molecule["I"] == 1) Product("Potassium Iodide");
        }
        else if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["H"] == 2 && playerData.Molecule["O"] == 2) Product("Hydrogen Peroxide");
        }
        else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") &&
                 playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("O"))
        {
            if (playerData.Molecule["C"] == 2 && playerData.Molecule["H"] == 3 &&
                playerData.Molecule["Na"] == 1 && playerData.Molecule["O"] == 2) Product("Sodium Acetate");
        }
        else if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("Cl"))
        {
            if (playerData.Molecule["H"] == 1 && playerData.Molecule["Cl"] == 1) Product("Hydrochloric Acid");
        }
        else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("I"))
        {
            if (playerData.Molecule["Na"] == 1 && playerData.Molecule["I"] == 1) Product("Sodium Iodide");
        }
        craftingTable();
    }
    public void maxFlask()
    {
        GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value = GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().maxValue;
    }
    public void maxCraft()
    {
        GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value = GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().maxValue;
    }
    public void closeFlaskTab()
    {
        flaskHotbar.SetActive(false);
        Time.timeScale = 1;
    }
    public void closeCraftTab()
    {
        craftingTableHotbar.SetActive(false);
        Time.timeScale = 1;
    }
    private void Product(string molecule)
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
            if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == molecule)
            {
                playerData.slotItem[$"Slot{i}"]["Quantity"] =
                    Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                break;
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null &&
                     playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                playerData.slotItem[$"Slot{i}"]["Element"] = molecule;
                playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                break;
            }

        slotCheck();
        playerData.Molecule.Clear();
    }
    private void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = "";
            GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text = "";
        }
        for (var i = 1; i <= playerData.Molecule.Count; i = i + 1)
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text =
                    playerData.Molecule.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Slot{i}/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Elementnum").GetComponent<Text>().text = "";
            }
    }
    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
        {
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null &&
                Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text =
                    playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null &&
                     Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text =
                    playerData.slotItem[$"Slot{i}"]["Element"].ToString();
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text =
                    playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null &&
                     playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                GameObject.Find($"Text{i}").GetComponent<Text>().text = "";
                GameObject.Find($"ItemNum{i}").GetComponent<Text>().text = "";
            }
        }
    }
    public void React()
    {
        var playerData = PlayerData.Instance();
        if (playerData.levelAvailable.Contains("Level 1"))
            if (playerData.flaskElements.ContainsKey("K") && playerData.flaskElements.ContainsKey("Water") && playerData.flaskElements.Count == 2)
            {
                if ((playerData.flaskElements["K"] == 1) && (playerData.flaskElements["Water"] == 1))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Explosion!";
                    player.hasAnimated = false;
                    updateLevel();
                }
            }
        if (playerData.levelAvailable.Contains("Level 2"))
            if ((playerData.flaskElements.ContainsKey("Hydrochloric Acid")) && (playerData.flaskElements.ContainsKey("Ammonia")) && (playerData.flaskElements.Count == 2))
            {
                if ((playerData.flaskElements["Hydrochloric Acid"] == 1) && (playerData.flaskElements["Ammonia"] == 1))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Smoke!";
                    //Instantiate(smokeEffect, flaskMouth.position, flaskMouth.rotation);
                    updateLevel();
                }
            }
        if (playerData.levelAvailable.Contains("Level 3"))
            if (playerData.flaskElements.ContainsKey("Hydrogen Peroxide") && playerData.flaskElements.ContainsKey("Sodium Iodide") && playerData.flaskElements.Count == 2)
            {
                if ((playerData.flaskElements["Hydrogen Peroxide"] == 1) && (playerData.flaskElements["Sodium Iodide"] == 1))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Splash!";
                    updateLevel();
                }
            }
        if (playerData.levelAvailable.Contains("Level 4"))
            if (playerData.flaskElements.ContainsKey("Sodium Acetate") && playerData.flaskElements.ContainsKey("Water") && playerData.flaskElements.Count == 2)
            {
                if ((playerData.flaskElements["Sodium Acetate"] == 1) && (playerData.flaskElements["Water"] == 1))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Hot Ice";
                    updateLevel();
                }
            }
        if (playerData.levelAvailable.Contains("Level 5"))
        {
            if (playerData.flaskElements.ContainsKey("Potassium Iodide") && playerData.flaskElements.ContainsKey("Hydrogen Peroxide") && playerData.flaskElements.ContainsKey("Soup") && playerData.flaskElements.Count == 3)
            {
                if ((playerData.flaskElements["Potassium Iodide"] == 1) && (playerData.flaskElements["Hydrogen Peroxide"] == 1) && (playerData.flaskElements["Soup"] == 1))
                {
                    GameObject.Find("Label2").GetComponent<Text>().text = "Elephant Toothpaste";
                    updateLevel();
                }
            }
        }
        flaskHotbar.SetActive(false);
        Time.timeScale = 1;
    }
    private void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = "";
            GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text = "";
        }
        for (var i = 1; i <= playerData.flaskElements.Count; i = i + 1)
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text =
                    playerData.flaskElements.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Slot{i}/Item").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Slot{i}/Invenum").GetComponent<Text>().text = "";
            }
    }
    private void Guide()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Level <= 5)
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text =
                Chemidex.chemRecipes[$"Recipe {playerData.Level}"];
        else if (playerData.Level > 5)
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "Under Constaruction!";
    }
    private void updateLevel()
    {
        var playerData = PlayerData.Instance();
        playerData.flaskElements.Clear();
        playerData.Level += 1;
        playerData.levelAvailable.Add($"Level {playerData.Level}");
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        Guide();
        flaskCheck();
    }
}