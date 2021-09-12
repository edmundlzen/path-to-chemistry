using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class QuizData
{
    public static int Level = 0;
    public static int Index = 0;
    public static float Quest = 0;
    public static float Score = 0;
    public static List<string> Answers = new List<string>()
    {
        { "A" },
        { "B" },
        { "C" },
        { "D" }
    };
    public static List<Dictionary<string, string>> Quizzes = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>(){
                { "Question", "Are Yi Fan gay?" },
                { "correctAnswer", "B" },
                { "Answer0", "No!" },
                { "Answer1", "Yes!" },
                { "Answer2", "No!" },
                { "Answer3", "No!" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Are Benjamin gay?" },
                { "correctAnswer", "C" },
                { "Answer0", "Yes!" },
                { "Answer1", "Yes!" },
                { "Answer2", "No!" },
                { "Answer3", "Yes!" },
            }
        }
    };
}

public class QuizHandler : MonoBehaviour
{
    public GameObject Overall;
    private void Start()
    {
        Load();
        generateLevel();
        QuizData.Quest = QuizData.Quizzes.Count;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Answer()
    {
        if (QuizData.Quizzes.Count > 0)
        {
            QuizData.Level += 1;
            if (EventSystem.current.currentSelectedGameObject.name == QuizData.Quizzes[QuizData.Index]["correctAnswer"])
            {
                QuizData.Score += 1;
            }
            QuizData.Quizzes.RemoveAt(QuizData.Index);
            GameObject.Find("Level").GetComponent<Text>().text = $"Level: {QuizData.Level} / {QuizData.Quest}";
            GameObject.Find("Score").GetComponent<Text>().text = $"Score: {QuizData.Score}";
            generateLevel();
        }
    }
    private void generateLevel()
    {
        var elementData = ElementData.Instance();
        if (QuizData.Quizzes.Count > 0)
        {
            if (QuizData.Quizzes.Count > 1)
            {
                System.Random Randomize = new System.Random();
                QuizData.Index = Randomize.Next(QuizData.Quizzes.Count);
                GameObject.Find("Question").GetComponent<Text>().text = $"Question: {QuizData.Quizzes[QuizData.Index][$"Question"]}";
                for (int i = 0; i <= 3; i++)
                {
                    GameObject.Find($"{QuizData.Answers[i]}/Answer").GetComponent<Text>().text = $"{QuizData.Answers[i]}. {QuizData.Quizzes[QuizData.Index][$"Answer{i}"]}";
                }
            }
            else
            {
                QuizData.Index = 0;
                GameObject.Find("Question").GetComponent<Text>().text = $"Question: {QuizData.Quizzes[QuizData.Index][$"Question"]}";
                for (int i = 0; i <= 3; i++)
                {
                    GameObject.Find($"{QuizData.Answers[i]}/Answer").GetComponent<Text>().text = $"{QuizData.Answers[i]}. {QuizData.Quizzes[QuizData.Index][$"Answer{i}"]}";
                }
            }
        }
        else
        {
            List<string> Reward = new List<string>();
            System.Random Randomize = new System.Random();
            int Index = Randomize.Next(118);
            foreach (var Key in elementData.elements.Keys)
            {
                Reward.Add(Key);
            }
            Overall.SetActive(true);
            GameObject.Find("Evaluation").GetComponent<Text>().text = $"Score: {QuizData.Score / QuizData.Quest * 100}%\nReward: {Reward[Index]}";
            GameObject.Find("Score Bar").GetComponent<Slider>().value = QuizData.Score / QuizData.Quest * 100;
            if (QuizData.Score / QuizData.Quest * 100 >= 67)
            {
                GameObject.Find("Fill").GetComponent<Image>().color = Color.green;
            }
            else if (QuizData.Score / QuizData.Quest * 100 < 67 && QuizData.Score / QuizData.Quest * 100 >= 34)
            {
                GameObject.Find("Fill").GetComponent<Image>().color = Color.yellow;
            }
            else if (QuizData.Score / QuizData.Quest * 100 < 34)
            {
                GameObject.Find("Fill").GetComponent<Image>().color = Color.red;
            }
        }
    }
    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
}