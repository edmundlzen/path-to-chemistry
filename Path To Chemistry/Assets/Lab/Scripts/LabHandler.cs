using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LabData
{
    public static bool hasDone = false;
    public static bool isAnimationPlaying = false;
}

public class LabHandler : MonoBehaviour
{
    public GameObject flaskHotbar;
    public GameObject elementConstructorPanel;
    public GameObject Mode;
    private void Start()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find("Wake").GetComponent<Animator>().SetTrigger("Wake");
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        expCheck();
        Guide();
    }
    private void Update()
    {
        var playerData = PlayerData.Instance();
        if (Input.GetMouseButtonDown(0) && player.raycastObject == "Experiments" && !player.Pause && !player.labPause && !LabData.isAnimationPlaying)
        {

            flaskHotbar.SetActive(true);
            player.labPause = true;
            flaskCheck();
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Compound Creator & Compound Reducer" && !player.Pause && !player.labPause && !LabData.isAnimationPlaying)
        {
            Mode.SetActive(true);
            player.labPause = true;
            
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Element Constructor" && !player.Pause && !player.labPause && !LabData.isAnimationPlaying)
        {
            elementConstructorPanel.SetActive(true);
            player.labPause = true;
            elementConstructor.Protons = 0;
            elementConstructor.Electrons = 0;
            elementConstructor.Neutrons = 0;
            GameObject.Find("Energy").GetComponent<Text>().text = Convert.ToString(playerData.Energy);
            GameObject.Find("protonNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Protons);
            GameObject.Find("electronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Electrons);
            GameObject.Find("neutronNum").GetComponent<Text>().text = Convert.ToString(elementConstructor.Neutrons);
            GameObject.Find("Product").GetComponent<Text>().text = "Craft";
        }
    }

    public void cancelMode()
    {
        Mode.SetActive(false);
        player.labPause = false;
    }

    public void craftMode(GameObject CraftUI)
    {
        Mode.SetActive(false);
        CraftUI.SetActive(true);
        craftingTable();
    }

    public void reduceMode(GameObject reduceUI)
    {
        Mode.SetActive(false);
        reduceUI.SetActive(true);
        reduceCheck();
    }

    private void reduceCheck()
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= 5; i++)
        {
            GameObject.Find($"Original/Slot ({i})/Origin").GetComponent<Text>().text = "";
            GameObject.Find($"Original/Slot ({i})/Originnum").GetComponent<Text>().text = "";
        }
        for (int i = 1; i <= playerData.Elements.Count; i++)
        {
            if (playerData.Elements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Original/Slot ({i})/Origin").GetComponent<Text>().text = playerData.Elements.Keys.ElementAt(i - 1);
                GameObject.Find($"Original/Slot ({i})/Originnum").GetComponent<Text>().text = Convert.ToString(playerData.Elements.Values.ElementAt(i - 1));
            }
            else if (playerData.Elements.Values.ElementAt(i - 1) <= 1)
            {
                GameObject.Find($"Original/Slot ({i})/Origin").GetComponent<Text>().text = playerData.Elements.Keys.ElementAt(i - 1);
                GameObject.Find($"Original/Slot ({i})/Originnum").GetComponent<Text>().text = "";
            }
        }
        reducerCheck();
    }

    private void reducerCheck()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Compound.Count < 1)
        {
            Destroy(GameObject.Find($"Compound/Slot/Item/Image"));
            GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = "";
        }
        else
        {
            if (playerData.Compound.Values.ElementAt(0) > 1)
            {
                deepReducerCheck();
                GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = Convert.ToString(playerData.Compound.Values.ElementAt(0));
            }
            else
            {
                deepReducerCheck();
                GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = "";
            }
        }
    }

    private void deepReducerCheck()
    {
        var playerData = PlayerData.Instance();
        if (GameObject.Find($"Compound/Slot/Item/Image") == null)
        {
            GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/C Image"));
            slotImage.name = "Image";
            slotImage.transform.SetParent(GameObject.Find($"Compound/Slot/Item").transform);
        }
        GameObject.Find($"Compound/Slot/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.Compound.Keys.ElementAt(0)}");
    }

    public void History(GameObject HistoryUI)
    {
        if (!player.labPause)
        {
            player.labPause = true;
            HistoryUI.SetActive(true);
            GameObject.Find("Red Dot").GetComponent<Image>().color = Color.clear;
            if (player.History.Count > 0)
            {
                for (int i = 1; i <= player.History.Count; i++)
                {
                    if (GameObject.Find("Nothing") != null)
                    {
                        Destroy(GameObject.Find("Nothing"));
                    }
                    GameObject newAlert = Instantiate(Resources.Load<GameObject>($"Lab/Alertline"));
                    newAlert.name = $"Alert{i}";
                    newAlert.GetComponent<Text>().text = $"{i}. {player.History[i - 1]}";
                    newAlert.transform.SetParent(GameObject.Find("History").transform);
                }
            }
            else
            {
                GameObject newAlert = Instantiate(Resources.Load<GameObject>($"Lab/Alertline"));
                newAlert.name = $"Nothing";
                newAlert.GetComponent<Text>().text = "Nothing here ¯\\_(ツ)_/¯";
                newAlert.transform.SetParent(GameObject.Find("History").transform);
            }
        }
    }

    public void closeHistory(GameObject HistoryUI)
    {
        if (GameObject.Find("Nothing") != null)
        {
            Destroy(GameObject.Find("Nothing"));
        }
        for (int i = 1; i <= player.History.Count; i++)
        {
            Destroy(GameObject.Find($"Alert{i}"));
        }
        player.labPause = false;
        HistoryUI.SetActive(false);
    }

    public void addQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value -= 1;
        }
    }
    public void addCraftElement(GameObject craftQuantity)
    {
        var playerData = PlayerData.Instance();
        if (!player.deepPause)
        {
            if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
            {
                if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
                {
                    craftQuantity.SetActive(true);
                    player.deepPause = true;
                    GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
                }
                else
                {
                    if (playerData.Molecule.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()) && playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + 1 <= 64)
                    {
                        if (playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + 1 <= 64)
                        {
                            playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                        }
                        else
                        {
                            addAlert($"Alert: The compound creator's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            return;
                        }
                    }
                    else
                    {
                        if (playerData.Molecule.Count + 1 <= 10)
                        {
                            playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
                        }
                        else
                        {
                            addAlert("Alert: The compound creator's hotbar are full!");
                            return;
                        }
                    }
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                    slotCheck();
                    craftingTable();
                }
            }
        }
    }
    public void addFlaskElement(GameObject flaskQuantity)
    {
        var playerData = PlayerData.Instance();
        if (!player.deepPause)
        {
            if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
            {
                if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
                {
                    flaskQuantity.SetActive(true);
                    player.deepPause = true;
                    GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
                }
                else
                {
                    if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                    {
                        if (playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + 1 <= 64)
                        {
                            playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + 1;
                        }
                        else
                        {
                            addAlert($"Alert: The flask's hotbar will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                            return;
                        }
                    }
                    else
                    {
                        if (playerData.flaskElements.Count + 1 <= 10)
                        {
                            playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), 1);
                        }
                        else
                        {
                            addAlert("Alert: The flask's hotbar are full!");
                            return;
                        }
                    }
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                    playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                    slotCheck();
                    flaskCheck();
                }
            }
        }
    }
    public void Pause(GameObject pauseMenu)
    {
        if (!player.labPause)
        {
            player.labPause = true;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume(GameObject pauseMenu)
    {
        player.labPause = false;
        pauseMenu.SetActive(false);
    }

    public void mainMenu()
    {
        StartCoroutine(sleepAnime("Main Menu"));
    }

    public void experimentRecipes()
    {
        if (!player.deepPause)
        {
            player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(sleepAnime("Recipes"));
        }
    }

    public void molRecipes()
    {
        if (!player.deepPause)
        {
            player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(sleepAnime("Molecule Recipes"));
        }
    }

    public void Inventory()
    {
        if (!player.labPause)
        {
            player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(sleepAnime("Inventory"));
        }
    }

    public void Terrain()
    {
        StartCoroutine(sleepAnime("Terrain"));
    }
    public void Quiz()
    {
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(sleepAnime("Quiz"));
    }

    public void goLeaderboard()
    {
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(sleepAnime("Nickname"));
    }

    private IEnumerator sleepAnime(string Scene)
    {
        GameObject.Find("Sleep").GetComponent<Animator>().SetTrigger("Sleep");
        yield return new WaitForSeconds(2);
        player.labPause = false;
        SceneManager.LoadScene(Scene);
    }

    public void Slider(float Value)
    {
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Mathf.Floor(Value));
    }

    public void Back1()
    {
        if (!player.deepPause)
        {
            elementConstructorPanel.SetActive(false);
            player.labPause = false;
            player.deepPause = false;
        }
    }
    public void placeFlaskQuantity(GameObject flaskQuantity)
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value));
        GameObject.Find("flaskQuantity/Slider").GetComponent<Slider>().value = 0;
        flaskQuantity.SetActive(false);
        player.deepPause = false;
        if (sliderValue > 0)
        {
            if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null && playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null && playerData.flaskElements.Count <= 10)
            {
                if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    var Balance = Convert.ToInt32(playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]);
                    if (playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + sliderValue <= 64)
                    {
                        playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Balance + sliderValue;
                    }
                    else
                    {
                        playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Balance + (64 - Balance);
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - (64 - Balance);
                        addAlert($"Alert: The flask's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                        Flask();
                        return;
                    }
                }
                else
                {
                    if (playerData.flaskElements.Count + 1 <= 10)
                    {
                        if (sliderValue <= 64)
                        {
                            playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                        }
                        else
                        {
                            sliderValue = 64;
                            playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                            addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                        }
                    }
                    else
                    {
                        addAlert("Alert: The flask's hotbar are full!");
                        return;
                    }
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
                Flask();
            }
        }
    }

    private void Flask()
    {
        var playerData = PlayerData.Instance();
        if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
        {
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
        }
        slotCheck();
        flaskCheck();
    }

    public void placeCraftQuantity(GameObject craftQuantity)
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value);
        GameObject.Find("craftQuantity/Slider").GetComponent<Slider>().value = 0;
        craftQuantity.SetActive(false);
        player.deepPause = false;
        if (sliderValue > 0)
        {
            if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null && playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null && playerData.flaskElements.Count <= 10)
            {
                if (playerData.Molecule.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    var Balance = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]);
                    if (playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + sliderValue <= 64)
                    {
                        playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Convert.ToInt32(playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) + sliderValue;
                    }
                    else
                    {
                        playerData.Molecule[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Balance + (64 - Balance);
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - (64 - Balance);
                        addAlert($"Alert: The compound creator's will only exist one stack of {playerData.slotItem[$"Slot{ hotbar.slotNum}"]["Element"]} Elements!");
                        Workbench();
                        return;
                    }
                }
                else
                {
                    if (playerData.Molecule.Count + 1 <= 10)
                    {
                        if (sliderValue <= 64)
                        {
                            playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                        }
                        else
                        {
                            sliderValue = 64;
                            playerData.Molecule.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                            addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                        }

                    }
                    else
                    {
                        addAlert("Alert: The compound creator's hotbar are full!");
                        return;
                    }
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
                Workbench();
            }
        }
    }
    
    private void Workbench()
    {
        var playerData = PlayerData.Instance();
        if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
        {
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
        }
        slotCheck();
        craftingTable();
    }

    public void Craft()
    {
        var playerData = PlayerData.Instance();
        if (!player.deepPause)
        {
            if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["H"] == 2 && playerData.Molecule["O"] == 1)
                {
                    Product("H2O");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("Cl") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["Na"] == 1 && playerData.Molecule["Cl"] == 1)
                {
                    Product("NaCl");
                }
            }
            else if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("Cl") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["H"] == 1 && playerData.Molecule["Cl"] == 1)
                {
                    Product("HCl");
                }
            }
            else if (playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["N"] == 1 && playerData.Molecule["H"] == 3)
                {
                    Product("NH3");
                }
            }
            else if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["H"] == 2 && playerData.Molecule["O"] == 2)
                {
                    Product("H2O2");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("I") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["Na"] == 1 && playerData.Molecule["I"] == 1)
                {
                    Product("NaI");
                }
            }
            else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 4)
            {
                if (playerData.Molecule["C"] == 2 && playerData.Molecule["H"] == 3 && playerData.Molecule["Na"] == 1 && playerData.Molecule["O"] == 2)
                {
                    Product("C2H3NaO2");
                }
            }
            else if (playerData.Molecule.ContainsKey("K") && playerData.Molecule.ContainsKey("I") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["K"] == 1 && playerData.Molecule["I"] == 1)
                {
                    Product("KI");
                }
            }
            else if (playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["N"] == 2 && playerData.Molecule["H"] == 4)
                {
                    Product("N2H4");
                }
            }
            else if (playerData.Molecule.ContainsKey("Ag") && playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 3)
            {
                if (playerData.Molecule["Ag"] == 1 && playerData.Molecule["N"] == 1 && playerData.Molecule["O"] == 3)
                {
                    Product("AgNO3");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("P") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["Na"] == 3 && playerData.Molecule["P"] == 1)
                {
                    Product("Na3P");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["Na"] == 1 && playerData.Molecule["H"] == 1)
                {
                    Product("NaH");
                }
            }
            else if (playerData.Molecule.ContainsKey("I") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["I"] == 1 && playerData.Molecule["O"] == 3)
                {
                    Product("IO3");
                }
            }
            else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 3)
            {
                if (playerData.Molecule["C"] == 3 && playerData.Molecule["H"] == 8 && playerData.Molecule["O"] == 1)
                {
                    Product("C3H8O");
                }
            }
            else if (playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("N") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 3)
            {
                if (playerData.Molecule["H"] == 1 && playerData.Molecule["N"] == 1 && playerData.Molecule["O"] == 3)
                {
                    Product("HNO3");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("Cl") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 3)
            {
                if (playerData.Molecule["Na"] == 1 && playerData.Molecule["Cl"] == 1 && playerData.Molecule["O"] == 1)
                {
                    Product("NaClO");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("S") && playerData.Molecule.Count == 2)
            {
                if (playerData.Molecule["Na"] == 2 && playerData.Molecule["S"] == 1)
                {
                    Product("Na2S");
                }
            }
            else if (playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("N") && playerData.Molecule.Count == 3)
            {
                if (playerData.Molecule["Na"] == 1 && playerData.Molecule["C"] == 1 && playerData.Molecule["N"] == 1)
                {
                    Product("NaCN");
                }
            }
            else if (playerData.Molecule.ContainsKey("C") && playerData.Molecule.ContainsKey("H") && playerData.Molecule.ContainsKey("Na") && playerData.Molecule.ContainsKey("O") && playerData.Molecule.Count == 4)
            {
                if (playerData.Molecule["C"] == 18 && playerData.Molecule["H"] == 35 && playerData.Molecule["Na"] == 1 && playerData.Molecule["O"] == 2)
                {
                    Product("C18H35NaO2");
                }
            }
            else
            {
                addAlert("Alert: Nothing can be craft. Please refer to the recipes in Chemidex!");
            }
            craftingTable();
        }    
    }
    public void Max()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = GameObject.Find("Slider").GetComponent<Slider>().maxValue;
    }
    public void closeFlaskTab()
    {
        flaskHotbar.SetActive(false);
        player.labPause = false;
    }
    public void closeCraftTab(GameObject craftingTableHotbar)
    {
        craftingTableHotbar.SetActive(false);
        player.labPause = false;
    }
    private void Product(string molecule)
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
        {
            if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == molecule)
            {
                playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                LabData.hasDone = true;
                break;
            }
        }
        if (!LabData.hasDone)
        {
            for (var i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                {
                    playerData.slotItem[$"Slot{i}"]["Element"] = molecule;
                    playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                    LabData.hasDone = true;
                    break;
                }
            }
        }
        if (!LabData.hasDone)
        {
            for (var i = 1; i <= 9; i = i + 1)
            {
                if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                {
                    addAlert("Your hotbar slots are full now!");
                    return;
                }
            }
        }
        LabData.hasDone = false;
        playerData.Molecule.Clear();
        slotCheck();
    }

    public void React()
    {
        var playerData = PlayerData.Instance();
        if (!player.deepPause)
        {
            if (playerData.levelAvailable.Contains("Level 1"))
            {
                if (playerData.flaskElements.ContainsKey("K") && playerData.flaskElements.ContainsKey("H2O") && playerData.flaskElements.Count == 2)
                {
                    if ((playerData.flaskElements["K"] == 1) && (playerData.flaskElements["H2O"] == 1))
                    {
                        AnimeData.animeNum = 1;
                        player.hasAnimated = false;
                        Experience(5);
                        updateLevel(1);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 2"))
            {
                if ((playerData.flaskElements.ContainsKey("HCl")) && (playerData.flaskElements.ContainsKey("NH3")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HCl"] == 1) && (playerData.flaskElements["NH3"] == 1))
                    {
                        AnimeData.animeNum = 5;
                        player.hasAnimated = false;
                        Experience(5);
                        updateLevel(2);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 3"))
            {
                if ((playerData.flaskElements.ContainsKey("IO3")) && (playerData.flaskElements.ContainsKey("C3H8O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["IO3"] == 1) && (playerData.flaskElements["C3H8O"] == 1))
                    {
                        AnimeData.animeNum = 2;
                        player.hasAnimated = false;
                        Experience(8);
                        updateLevel(3);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 4"))
            {
                if ((playerData.flaskElements.ContainsKey("Na3P")) && (playerData.flaskElements.ContainsKey("H2O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["Na3P"] == 1) && (playerData.flaskElements["H2O"] == 1))
                    {
                        AnimeData.animeNum = 4;
                        player.hasAnimated = false;
                        Experience(5);
                        updateLevel(4);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 5"))
            {
                if ((playerData.flaskElements.ContainsKey("HCl")) && (playerData.flaskElements.ContainsKey("Na2S")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HCl"] == 1) && (playerData.flaskElements["Na2S"] == 1))
                    {
                        AnimeData.animeNum = 3;
                        player.hasAnimated = false;
                        Experience(6);
                        updateLevel(5);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 6"))
            {
                if ((playerData.flaskElements.ContainsKey("HCl")) && (playerData.flaskElements.ContainsKey("NaCN")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HCl"] == 1) && (playerData.flaskElements["NaCN"] == 1))
                    {
                        AnimeData.animeNum = 3;
                        player.hasAnimated = false;
                        Experience(6);
                        updateLevel(6);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 7"))
            {
                if ((playerData.flaskElements.ContainsKey("NaH")) && (playerData.flaskElements.ContainsKey("H2O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["NaH"] == 1) && (playerData.flaskElements["H2O"] == 1))
                    {
                        AnimeData.animeNum = 5;
                        player.hasAnimated = false;
                        Experience(6);
                        updateLevel(7);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 8"))
            {
                if ((playerData.flaskElements.ContainsKey("HNO3")) && (playerData.flaskElements.ContainsKey("N2H4")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HNO3"] == 1) && (playerData.flaskElements["N2H4"] == 1))
                    {
                        AnimeData.animeNum = 2;
                        player.hasAnimated = false;
                        Experience(7);
                        updateLevel(8);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 9"))
            {
                if ((playerData.flaskElements.ContainsKey("HCl")) && (playerData.flaskElements.ContainsKey("NaClO")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HCl"] == 1) && (playerData.flaskElements["NaClO"] == 1))
                    {
                        AnimeData.animeNum = 3;
                        player.hasAnimated = false;
                        Experience(7);
                        updateLevel(9);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 10"))
            {
                if ((playerData.flaskElements.ContainsKey("NH3")) && (playerData.flaskElements.ContainsKey("NaClO")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["NH3"] == 1) && (playerData.flaskElements["NaClO"] == 1))
                    {
                        AnimeData.animeNum = 3;
                        player.hasAnimated = false;
                        Experience(8);
                        updateLevel(10);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 11"))
            {
                if ((playerData.flaskElements.ContainsKey("HNO3")) && (playerData.flaskElements.ContainsKey("C3H8O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["HNO3"] == 1) && (playerData.flaskElements["C3H8O"] == 1))
                    {
                        AnimeData.animeNum = 2;
                        player.hasAnimated = false;
                        Experience(8);
                        updateLevel(11);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 12"))
            {
                if ((playerData.flaskElements.ContainsKey("AgNO3")) && (playerData.flaskElements.ContainsKey("NH3")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["AgNO3"] == 1) && (playerData.flaskElements["NH3"] == 1))
                    {
                        addAlert("Animation Under Construction");
                        Experience(7);
                        updateLevel(12);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 13"))
            {
                if ((playerData.flaskElements.ContainsKey("C2H3NaO2")) && (playerData.flaskElements.ContainsKey("H2O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["C2H3NaO2"] == 1) && (playerData.flaskElements["H2O"] == 1))
                    {
                        addAlert("Animation Under Construction");
                        Experience(9);
                        updateLevel(13);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 14"))
            {
                if ((playerData.flaskElements.ContainsKey("KI")) && (playerData.flaskElements.ContainsKey("H2O2")) && (playerData.flaskElements.ContainsKey("C18H35NaO2")) && (playerData.flaskElements.Count == 3))
                {
                    if ((playerData.flaskElements["KI"] == 1) && (playerData.flaskElements["H2O2"] == 1) && (playerData.flaskElements["C18H35NaO2"] == 1))
                    {
                        addAlert("Animation Under Construction");
                        Experience(9);
                        updateLevel(14);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 15"))
            {
                if ((playerData.flaskElements.ContainsKey("H2O2")) && (playerData.flaskElements.ContainsKey("NaI")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["H2O2"] == 1) && (playerData.flaskElements["NaI"] == 1))
                    {
                        addAlert("Animation Under Construction");
                        Experience(9);
                        updateLevel(15);
                        return;
                    }
                }
            }
            if (playerData.levelAvailable.Contains("Level 16"))
            {
                if ((playerData.flaskElements.ContainsKey("NaCl")) && (playerData.flaskElements.ContainsKey("H2O")) && (playerData.flaskElements.Count == 2))
                {
                    if ((playerData.flaskElements["NaCl"] == 1) && (playerData.flaskElements["H2O"] == 1))
                    {
                        addAlert("Animation Under Construction");
                        Experience(11);
                        updateLevel(16);
                        return;
                    }
                }
            }
            addAlert("Alert: Nothing happened. Please refer to the recipes in chemidex!");
        }
    }

    private void Experience(int EXP)
    {
        var playerData = PlayerData.Instance();
        for (int i = 1; i <= EXP; i++)
        {
            playerData.Experience += 1;
            if (playerData.Experience >= playerData.EXPLevel * 3)
            {
                playerData.Experience -= playerData.EXPLevel * 3;
                playerData.EXPLevel += 1;
            }
        }
        expCheck();
    }

    private void expCheck()
    {
        var playerData = PlayerData.Instance();
        GameObject.Find("Experince").GetComponent<Text>().text = Convert.ToString(playerData.EXPLevel);
        GameObject.Find("ExperienceBar").GetComponent<Slider>().maxValue = playerData.EXPLevel * 3;
        GameObject.Find("ExperienceBar").GetComponent<Slider>().value = playerData.Experience;
        var Normalized = GameObject.Find("ExperienceBar").GetComponent<Slider>().normalizedValue;
        if (Normalized >= 0 && Normalized < 0.25)
        {
            GameObject.Find("Fill").GetComponent<Image>().color = Color.red;
        }
        else if (Normalized >= 0.25 && Normalized < 0.75)
        {
            GameObject.Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
        else if (Normalized >= 0.75 && Normalized <= 1)
        {
            GameObject.Find("Fill").GetComponent<Image>().color = Color.green;
        }
    }

    private void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i++)
        {
            GameObject.Find($"flaskHotbar/Slot ({i})/Symbol").GetComponent<Text>().text = "";
            GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = "";
            if (playerData.flaskElements.Count < i)
            {
                Destroy(GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image"));
            }
        }
        for (var i = 1; i <= playerData.flaskElements.Count; i++)
        {
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                flaskDeepCheck(i);
                GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = Convert.ToString(playerData.flaskElements.Values.ElementAt(i - 1));
            }
            else
            {
                flaskDeepCheck(i);
                GameObject.Find($"flaskHotbar/Slot ({i})/Invenum").GetComponent<Text>().text = "";
            }
        }
    }

    private void flaskDeepCheck(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(playerData.flaskElements.Keys.ElementAt(i - 1)))
        {
            if (GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/F Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"flaskHotbar/Slot ({i})/Item").transform);
            }
            GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.flaskElements.Keys.ElementAt(i - 1)}");
        }
        else
        {
            Destroy(GameObject.Find($"flaskHotbar/Slot ({i})/Item/Image"));
            GameObject.Find($"flaskHotbar/Slot ({i})/Symbol").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
        }
    }

    private void craftingTable()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i++)
        {
            GameObject.Find($"craftTable/Slot ({i})/Element").GetComponent<Text>().text = "";
            GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = "";
            if (playerData.Molecule.Count < i)
            {
                Destroy(GameObject.Find($"craftTable/Slot ({i})/Item/Image"));
            }
        }
        for (var i = 1; i <= playerData.Molecule.Count; i++)
        {
            if (playerData.Molecule.Values.ElementAt(i - 1) > 1)
            {
                deepCraftingTable(i);
                GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = Convert.ToString(playerData.Molecule.Values.ElementAt(i - 1));
            }
            else
            {
                deepCraftingTable(i);
                GameObject.Find($"craftTable/Slot ({i})/Elementnum").GetComponent<Text>().text = "";
            }
        }
    }

    private void deepCraftingTable(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(playerData.Molecule.Keys.ElementAt(i - 1)))
        {
            if (GameObject.Find($"craftTable/Slot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/C Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"craftTable/Slot ({i})/Item").transform);
            }
            GameObject.Find($"craftTable/Slot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.Molecule.Keys.ElementAt(i - 1)}");
        }
        else
        {
            Destroy(GameObject.Find($"craftTable/Slot ({i})/Item/Image"));
            GameObject.Find($"craftTable/Slot ({i})/Element").GetComponent<Text>().text = playerData.Molecule.Keys.ElementAt(i - 1);
        }
    }

    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
        {
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{i}"]["Quantity"]);
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
                GameObject.Find($"HotbarSlot ({i})/Symbol").GetComponent<Text>().text = "";
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";

            }
        }
    }

    private void slotDeepCheck(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"])))
        {
            if (GameObject.Find($"HotbarSlot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/H Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"HotbarSlot ({i})/Item").transform);
            }
            GameObject.Find($"HotbarSlot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.slotItem[$"Slot{i}"]["Element"]}");
        }
        else
        {
            Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
            GameObject.Find($"HotbarSlot ({i})/Symbol").GetComponent<Text>().text = $"{playerData.slotItem[$"Slot{i}"]["Element"]}";
        }
    }

    private void Guide()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Level <= 5)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "GUIDE\n"+Chemidex.chemRecipes[$"Recipe {playerData.Level}"].Split('=')[0];
        }
        else if (playerData.Level > 16)
        {
            GameObject.Find("Guide").GetComponent<TextMeshPro>().text = "Under Constaruction!";
        }    
    }
    private void updateLevel(int Standard)
    {
        var nonLevelUp = new List<string>();
        var playerData = PlayerData.Instance();
        playerData.flaskElements.Clear();
        for (int i = 0; i < playerData.levelAvailable.Count(); i++)
        {
            nonLevelUp.Add(playerData.levelAvailable[i]);
        }
        nonLevelUp.RemoveAt(playerData.levelAvailable.Count() - 1);
        if (!nonLevelUp.Contains($"Level {Standard}"))
        {
            playerData.Level += 1;
            playerData.levelAvailable.Add($"Level {playerData.Level}");
        }
        GameObject.Find("Level").GetComponent<Text>().text = playerData.Level.ToString();
        Guide();
        flaskCheck();
        flaskHotbar.SetActive(false);
        player.labPause = false;
    }

    private void addAlert(string Alert)
    {
        int maxQuantity = 50;
        player.History.Add(Alert);
        GameObject.Find("Red Dot").GetComponent<Image>().color = Color.white;
        if (player.History.Count > maxQuantity)
        {
            player.History.RemoveAt(0);
        }
    }
}