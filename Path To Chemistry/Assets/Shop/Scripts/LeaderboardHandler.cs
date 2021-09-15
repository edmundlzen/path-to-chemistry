using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LeaderboardHandler : MonoBehaviour
{
    private string SecretKey = "3aa281873f2bcb79fe5825755e2ec382d90ba41d";
    private string GameID = "61";
    public GameObject Info;
    public void Start()
    {
        StartCoroutine(setPlayerScore());
        StartCoroutine(getAllPlayers());
    }
    IEnumerator addPlayer(string name)
    {
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        Form.AddField("nickname", name);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/add-player", Form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
                print(request.downloadHandler.text);
        }
    }
    IEnumerator setPlayerScore()
    {
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        Form.AddField("nickname", "soonheng11|1631463028");
        Form.AddField("score", "100");
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/set-player-score", Form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
                print(request.downloadHandler.text);
        }
    }
    IEnumerator getPlayerScore()
    {
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        Form.AddField("nickname", "Lasma|1631501653");
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/get-player-score", Form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
                print(request.downloadHandler.text);
        }
    }
    IEnumerator getAllPlayers()
    {
        string Text = "";
        string text2 = "";
        WWWForm Form = new WWWForm();
        Form.AddField("secret_key", SecretKey);
        Form.AddField("game_id", GameID);
        using (UnityWebRequest request = UnityWebRequest.Post("https://www.wowscores.com/get-all-players", Form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Info.SetActive(true);
            else
            {
                Text += "Ranks\n";
                text2 += "Score\n";
                for (int i = 0; i <= JsonConvert.DeserializeObject<List<object>>(Convert.ToString(JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text)["data"])).Count - 1; i++)
                {
                    Text += $"{i + 1}. {JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(JsonConvert.DeserializeObject<List<object>>(Convert.ToString(JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text)["data"]))[i]))["player_nickname"]}\n";
                    text2 +=$"{ JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(JsonConvert.DeserializeObject<List<object>>(Convert.ToString(JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text)["data"]))[i]))["player_score"]}\n";
                }
                GameObject.Find("Ranks").GetComponent<Text>().text = Text;
                GameObject.Find("Score").GetComponent<Text>().text = text2;
                Info.SetActive(false);
            }
        }
    }
    public void tryAgain()
    {
        StartCoroutine(getAllPlayers());
    }
    public void add()
    {
        StartCoroutine(addPlayer(GameObject.Find("Name").GetComponent<Text>().text));
        StartCoroutine(getAllPlayers());
    }
    public void openUrl()
    {
        Application.OpenURL("https://www.wowscores.com/game/testing");
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