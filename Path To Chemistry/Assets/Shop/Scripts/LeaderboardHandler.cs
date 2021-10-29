using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
public static class Protection
{
    public static int proNum;
}

public class AllPlayersData
{
    public int player_id { get; set; }
    public string player_nickname { get; set; }
    public string player_date_created { get; set; }
    public int player_score { get; set; }
}

public class LeaderboardData
{
    public string msg { get; set; }
    public int total { get; set; }
    public int player_id { get; set; }
    public string player_nickname { get; set; }
    public string player_date_created { get; set; }
    public int player_score { get; set; }
    public List<AllPlayersData> data { get; set; }
}

public class LeaderboardHandler : MonoBehaviour
{
    public GameObject Glory;
    public GameObject Info;
    private string SecretKey = "3aa281873f2bcb79fe5825755e2ec382d90ba41d";
    private string GameID = "61";
    private bool isSaving = false;
    public GameObject nicknameInfo;

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

    private IEnumerator autoSave()
    {
        isSaving = true;
        yield return new WaitForSeconds(1);
        Save();
        isSaving = false;
    }
    private void Start()
    {
        StartCoroutine(Leaderboard());
    }

    public void Back()
    {
        SceneManager.LoadScene(player.startPlace);
    }

    IEnumerator Leaderboard()
    {
        var playerData = PlayerData.Instance();
        if (playerData.ID == null)
        {
            WWWForm Form = new WWWForm();
            Form.AddField("secret_key", SecretKey);
            Form.AddField("game_id", GameID);
            Form.AddField("nickname", playerData.Nickname);
            using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/add-player", Form))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Info.SetActive(true);
                }
                else
                {
                    var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
                    playerData.ID = leaderboardData.player_nickname.Split('|')[1];
                    Save();
                    print(playerData.ID);
                }
            }
        }
        WWWForm Form1 = new WWWForm();
        Form1.AddField("secret_key", SecretKey);
        Form1.AddField("game_id", GameID);
        Form1.AddField("nickname", $"{playerData.Nickname}|{playerData.ID}");
        Form1.AddField("score", playerData.EXPLevel);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/set-player-score", Form1))
        {
            yield return request.SendWebRequest();
            var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
            if (request.isNetworkError || request.isHttpError)
            {
                Info.SetActive(true);
            }
            else
            {
                GameObject.Find("Your Score").GetComponent<Text>().text = $"Your Score: {leaderboardData.player_score}";
            }
        }
        WWWForm Form2 = new WWWForm();
        Form2.AddField("secret_key", SecretKey);
        Form2.AddField("game_id", GameID);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/get-all-players", Form2))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Info.SetActive(true);
            }
            else
            {
                var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
                for (int i = 0; i <= leaderboardData.data.Count - 1; i++)
                {
                    GameObject newGlory = Instantiate(Glory);
                    newGlory.name = $"Glory{i + 1}";
                    newGlory.transform.SetParent(GameObject.Find("Content").transform);
                    GameObject.Find($"Glory{i + 1}/Nickname").GetComponent<Text>().text = $"{i + 1}. {leaderboardData.data[i].player_nickname}\n";
                    GameObject.Find($"Glory{i + 1}/Score").GetComponent<Text>().text = $"{leaderboardData.data[i].player_score}\n";
                    GameObject.Find($"Glory{i + 1}/Date").GetComponent<Text>().text = $"{leaderboardData.data[i].player_date_created.Replace("-", "/")}\n";
                }
                GameObject.Find("Statistics").GetComponent<Text>().text = $"Nickname: {playerData.Nickname}\nID: #{ playerData.ID}";
                Info.SetActive(false);
            }
        }
    }
    IEnumerator addPlayer()
    {
        var playerData = PlayerData.Instance();
        if (playerData.ID == null)
        {
            WWWForm Form = new WWWForm();
            Form.AddField("secret_key", SecretKey);
            Form.AddField("game_id", GameID);
            Form.AddField("nickname", playerData.Nickname);
            using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/add-player", Form))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Info.SetActive(true);
                }
                else
                {
                    var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
                    playerData.ID = leaderboardData.player_nickname.Split('|')[1];
                }
            }
        }
    }
    IEnumerator setPlayerScore()
    {
        var playerData = PlayerData.Instance();
        WWWForm Form1 = new WWWForm();
        Form1.AddField("secret_key", SecretKey);
        Form1.AddField("game_id", GameID);
        Form1.AddField("nickname", $"{playerData.Nickname}|{playerData.ID}");
        Form1.AddField("score", playerData.Experience);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/set-player-score", Form1))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Info.SetActive(true);
            }
        }
    }
    IEnumerator getPlayerScore()
    {
        var playerData = PlayerData.Instance();
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        Form.AddField("nickname", $"{playerData.Nickname}|{playerData.ID}");
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/get-player", Form))
        {
            yield return request.SendWebRequest();
            print(request.downloadHandler.text);
        }
    }
    IEnumerator getAllPlayers()
    {
        var playerData = PlayerData.Instance();
        string Nickname = "";
        string Score = "";
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/get-all-players", Form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Info.SetActive(true);
            }
            else
            {
                var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
                Nickname += "Nickname\n";
                Score += "Score\n";
                for (int i = 0; i <= leaderboardData.data.Count - 1; i++)
                {
                    Nickname += $"{i + 1}. {leaderboardData.data[i].player_nickname}\n";
                    Score += $"{leaderboardData.data[i].player_score}\n";
                }
                GameObject.Find("Nickname").GetComponent<Text>().text = Nickname;
                GameObject.Find("Score").GetComponent<Text>().text = Score;
                GameObject.Find("Statistics").GetComponent<Text>().text = $"Nickname: {playerData.Nickname}\nID: #{ playerData.ID}";
                Info.SetActive(false);
            }
        }
    }
    public void tryAgain()
    {
        StartCoroutine(Leaderboard());
    }
    public void openUrl()
    {
        Application.OpenURL("https://www.wowscores.com/game/testing");
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
                if (playerData.Inventory.ContainsKey(elementData.elements.Keys.ElementAt(i - 1))) continue;
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
/*
    private void Start()
    {
        Load();
        var elementData = ElementData.Instance();
        var Ranks = 0;
        var Disqualify = 0;
        var Elements1 = "";
        var Elements2 = "";
        foreach (var i in elementData.elements.Keys)
        {
            if (elementData.rarity.Contains(i))
            {
                Ranks += 1;
                Elements1 += $"{Ranks}. {i}\n";
            }
            else
            {
                Disqualify += 1;
                Elements2 += $"{Disqualify}. {i}\n";
            }
        }
        GameObject.Find("Label").GetComponent<Text>().text = $"{Elements1}\n= = = = = = = = = = =\n{Elements2}";
    }
    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }

    private void Start()
    {
        Load();
        var elementData = ElementData.Instance();
        int Counter = 0;
        string Groups = "";
        List<string> existedGroups = new List<string>();
        foreach (var i in elementData.elements.Keys)
        {
            if (!existedGroups.Contains(elementData.elements[$"{i}"]["group"]))
            {
                Counter += 1;
                Groups += $"{Counter}. {elementData.elements[$"{i}"]["group"]}\n";
                existedGroups.Add(elementData.elements[$"{i}"]["group"]);
            }
        }
        GameObject.Find("Groups").GetComponent<Text>().text = Groups;
    }
*/