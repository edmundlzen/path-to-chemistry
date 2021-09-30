using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Definename : MonoBehaviour
{
    private bool isSaving = false;
    private void Awake()
    {
        isFirstSave();
        loadPlayerData();
        loadElementsData();
    }

    public void Update()
    {
        if (!isSaving)
        {
            StartCoroutine(autoSave());
        }
    }
    public void Back()
    {
        SceneManager.LoadScene(player.startPlace);
    }
    private void Start()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Nickname != null)
        {
            SceneManager.LoadScene("Leaderboard");
        }
    }

    private IEnumerator autoSave()
    {
        isSaving = true;
        yield return new WaitForSeconds(1);
        Save();
        isSaving = false;
    }

    public void nick()
    {
        var playerData = PlayerData.Instance();
        string words = GameObject.Find("TypedName").GetComponent<Text>().text.Trim();
        int WordCount = words.Length;
        if (WordCount >= 3 && WordCount <= 15)
        {
            playerData.Nickname = words;
            Save();
            SceneManager.LoadScene("Leaderboard");
        }
        else
        {
            GameObject.Find("AlertKia").GetComponent<Text>().text = "*Alert Your nickname's letters should not be less than 3 words and more than 15 words!";
        }
    }
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
                playerData.Inventory.Add(elementData.elements.Keys.ElementAt(i - 1), 0);
            }
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
}
