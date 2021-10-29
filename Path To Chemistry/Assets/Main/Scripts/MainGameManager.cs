using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    public bool useButton = false;
    public Transform playerCamera;
    public GameObject interactButton;
    public Slider hpBar;
    private bool isSaving;

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
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
    }

    private void Awake()
    {
        isFirstSave();
        loadPlayerData();
        loadElementsData();
    }
    
    private IEnumerator autoSave()
    {
        isSaving = true;
        yield return new WaitForSeconds(1);
        Save();
        isSaving = false;
        yield return null;
    }

    public void InteractButtonDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interactable(playerCamera);
            }
        }
    }
    
    public void UseButtonDown()
    {
        useButton = true;
    }

    public void UseButtonUp()
    {
        useButton = false;
    }

    void PlayerDead()
    {
        // Do stuff you do when player dead here.
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("death");
    }

    void Update()
    {
        if (!isSaving)
        {
            StartCoroutine(autoSave());
        }
        var playerHealth = PlayerData.Instance().survivalHealth;
        float smoothTime = 0.3f;
        float yVelocity = 0.0f;
        hpBar.value = playerHealth > 0 ? playerHealth / 100f : 0f; // Mathf.SmoothDamp(hpBar.value, playerHealth / 100f, ref yVelocity, smoothTime)
        if (playerHealth <= 0)
        {
            Time.timeScale = 0f;
            PlayerData.Instance().survivalHealth = 100;
            PlayerDead();
            return;
        }
        if (useButton)
        {
            if (GameObject.FindGameObjectsWithTag("HandItem")[0].TryGetComponent<Placeable>(out Placeable placable)) 
            {
                placable.Use(playerCamera);
            } else if (GameObject.FindGameObjectsWithTag("HandItem")[0].TryGetComponent<IUsable>(out IUsable usable))
            {
                usable.Use(playerCamera);
            }
        }
        
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 2))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactButton.SetActive(true);
                return;
            }
        }
        interactButton.SetActive(false);
    }
}
