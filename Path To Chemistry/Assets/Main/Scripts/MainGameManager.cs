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

    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
    
    private void Save()
    {
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }

    void Awake()
    {
        ElementDataSetup();
    }

    void ElementDataSetup()
    {
        var playerData = PlayerData.Instance();
        Load();
        var elementData = ElementData.Instance();
        if (playerData.Inventory.Count == 0)
        {
            for (int i = 0; i < 118; i++)
            {
                playerData.Inventory.Add(elementData.elements.ElementAt(i).Key, 0);
            }
            Save();
        }
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
