using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject identifyQuantity;
    public GameObject flaskHotbar;
    public GameObject craftingTableHotbar;
    public GameObject elementConstructorPanel;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.raycastObject == "Add")
        {
            identifyQuantity.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Add2")
        {
            craftingTableHotbar.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetMouseButtonDown(0) && player.raycastObject == "Add3")
        {
            elementConstructorPanel.SetActive(true);
            Time.timeScale = 0;
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
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Convert.ToInt32(Value));
    }

    public void Back1()
    {
        elementConstructorPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Done()
    {
        flaskHotbar.SetActive(true);
        identifyQuantity.SetActive(false);
        var playerData = PlayerData.Instance();
        var silderValue = Convert.ToInt32(GameObject.Find("Slider").GetComponent<Slider>().value);
        if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null &&
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null && playerData.flaskElements.Count <= 10)
        {
            if (playerData.flaskElements.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]
                .ToString()))
                playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] =
                    Convert.ToInt32(
                        playerData.flaskElements[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]) +
                    silderValue;
            else
                playerData.flaskElements.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(),
                    silderValue);
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
            {
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] =
                    Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - silderValue;
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

    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
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

    private void flaskCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 10; i = i + 1)
        {
            GameObject.Find($"Item{i}").GetComponent<Text>().text = "";
            GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
        }

        for (var i = 1; i <= playerData.flaskElements.Count; i = i + 1)
            if (playerData.flaskElements.Values.ElementAt(i - 1) > 1)
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text =
                    playerData.flaskElements.Values.ElementAt(i - 1).ToString();
            }
            else
            {
                GameObject.Find($"Item{i}").GetComponent<Text>().text = playerData.flaskElements.Keys.ElementAt(i - 1);
                GameObject.Find($"Invenum{i}").GetComponent<Text>().text = "";
            }
    }
}