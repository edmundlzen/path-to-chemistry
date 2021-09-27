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
    public static bool hasLoaded = false;
    public static List<string> Answers = new List<string>()
    {
        { "A" },
        { "B" },
        { "C" },
        { "D" }
    };
    public static List<Dictionary<string, string>> Quizzes = new List<Dictionary<string, string>>();
    public static List<Dictionary<string, string>> CopyQuizzes = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>(){
                { "Question", "Which of the following is blue in colour?" },
                { "correctAnswer", "A" },
                { "Answer0", "Copper (II) Nitrate" },
                { "Answer1", "Magnesium Sulphate" },
                { "Answer2", "Iron (II) Nitrate" },
                { "Answer3", "Iron (III) Sulphate" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "A precipitate is formed when 2 cm³ of an unknown salt solution is added to 2 cm³ of nitric acid and 2 cm³ of barium chloride solution. What anion is present in the unknown salt solution?" },
                { "correctAnswer", "D" },
                { "Answer0", "Nitrate ion" },
                { "Answer1", "Carbonate ion" },
                { "Answer2", "Chloride ion" },
                { "Answer3", "Sulphate ion" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which pair(s) of salt solutions can be used to prepare lead (II) sulphate?" },
                { "correctAnswer", "B" },
                { "Answer0", "Lead (II) chloride and potassium sulphate" },
                { "Answer1", "Lead (II) nitrate and sodium sulphate" },
                { "Answer2", "Lead (II) carbonate and sodium sulphate" },
                { "Answer3", "Lead (II) bromide and sodium sulphate" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which salts will form gas bubbles when 2 cm³ of sulphuric acid is added to it?" },
                { "correctAnswer", "C" },
                { "Answer0", "Sodium Chloride" },
                { "Answer1", "Lead (II) Iodide" },
                { "Answer2", "Iron (II) Carbonate" },
                { "Answer3", "Zinc Nitrate" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "An unknown salt X forms a colourless solution in both excess sodium hydroxide solution and excess ammonia solution. What cation is present in salt X?" },
                { "correctAnswer", "A" },
                { "Answer0", "Zinc ion" },
                { "Answer1", "Calcium ion" },
                { "Answer2", "Ammonium ion" },
                { "Answer3", "Magnesium ion" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which of the following pairs of substances can be used to prepare copper (II) chloride?" },
                { "correctAnswer", "A" },
                { "Answer0", "Copper + Hydrochloric" },
                { "Answer1", "Copper (II) nitrate + Hydrochloric Acid" },
                { "Answer2", "Copper (II) Oxide + Hydrochloric Acid" },
                { "Answer3", "Copper (II) sulphate + Hydrochloric Acid" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Salt X releases a brown gas when heated. Which of the following salts might be salt X?" },
                { "correctAnswer", "B" },
                { "Answer0", "Sodium sulphate" },
                { "Answer1", "Magnesium nitrate" },
                { "Answer2", "Calcium carbonate" },
                { "Answer3", "Aluminium chloride" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Salt X forms a white precipitate in excess sodium hydroxide solution but forms a colourless solution when a little of ammonia solution is added. What cation is present in salt X?" },
                { "correctAnswer", "D" },
                { "Answer0", "Zinc ion" },
                { "Answer1", "Magnesium ion" },
                { "Answer2", "Ammonia ion" },
                { "Answer3", "Calcium ion" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "If 2 cm³ of salt X is added to 2 cm³ of nitric acid, colourless gas bubbles are released. What anion is present in salt X?" },
                { "correctAnswer", "C" },
                { "Answer0", "Iodide ion" },
                { "Answer1", "Sulphate ion" },
                { "Answer2", "Carbonate ion" },
                { "Answer3", "Nitrate ion" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which one is atom?" },
                { "correctAnswer", "A" },
                { "Answer0", "Magnesium" },
                { "Answer1", "H2O" },
                { "Answer2", "NaCl" },
                { "Answer3", "CO2" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What is atom?" },
                { "correctAnswer", "A" },
                { "Answer0", "the base constructive structure of a matter" },
                { "Answer1", "combination with two or more matter" },
                { "Answer2", "charged particles" },
                { "Answer3", "Types of microorganisms" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What is the charge of ion sulphate(SO4)?" },
                { "correctAnswer", "D" },
                { "Answer0", "+1" },
                { "Answer1", "+2" },
                { "Answer2", "-1" },
                { "Answer3", "-2" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What is cation charged?" },
                { "correctAnswer", "B" },
                { "Answer0", "-" },
                { "Answer1", "+" },
                { "Answer2", "÷" },
                { "Answer3", "×" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What is mole?" },
                { "correctAnswer", "C" },
                { "Answer0", "mole is atom of the same elments with the same nombor of proton and electron but different number of neutron" },
                { "Answer1", "It's a small mammal animal" },
                { "Answer2", "Mole is a base unit of mesaure of quantity of a substance" },
                { "Answer3", "non of this above" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which one is the emperical fomular of magnesium oxide?" },
                { "correctAnswer", "A" },
                { "Answer0", "MgO" },
                { "Answer1", "Mg2O" },
                { "Answer2", "Mg3O2" },
                { "Answer3", "MgO3" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which one is the correct molar mass of oxygen" },
                { "correctAnswer", "B" },
                { "Answer0", "32" },
                { "Answer1", "16" },
                { "Answer2", "18" },
                { "Answer3", "21" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What does ion oxide charged" },
                { "correctAnswer", "A" },
                { "Answer0", "2-" },
                { "Answer1", "+" },
                { "Answer2", "-" },
                { "Answer3", "2+" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The group number tells us how many _________ there are in an element." },
                { "correctAnswer", "C" },
                { "Answer0", "groups" },
                { "Answer1", "energy levels" },
                { "Answer2", "valence electrons" },
                { "Answer3", "chemicals" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The _________ tells you how many energy levels are used in an element." },
                { "correctAnswer", "D" },
                { "Answer0", "group number" },
                { "Answer1", "atom" },
                { "Answer2", "protons" },
                { "Answer3", "period number" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The element Tellurium has the atomic mass of 127.60 and its atomic number is 52. How many neutrons are in the element Tellurium?" },
                { "correctAnswer", "D" },
                { "Answer0", "179.60" },
                { "Answer1", "127.60" },
                { "Answer2", "52" },
                { "Answer3", "76" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The inner energy level can only hold _________ electrons." },
                { "correctAnswer", "B" },
                { "Answer0", "0" },
                { "Answer1", "2" },
                { "Answer2", "1" },
                { "Answer3", "18" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The largest amount of electrons the 3rd energy level can hold is?" },
                { "correctAnswer", "C" },
                { "Answer0", "unlimited amount" },
                { "Answer1", "2" },
                { "Answer2", "18" },
                { "Answer3", "8" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "The characteristics or properties of metals include" },
                { "correctAnswer", "D" },
                { "Answer0", "shiny and solid" },
                { "Answer1", "ductile and malleable" },
                { "Answer2", "lose electrons" },
                { "Answer3", "all of the above" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "How many valence electrons do elements in group 17 have in their outer energy level?" },
                { "correctAnswer", "A" },
                { "Answer0", "7" },
                { "Answer1", "17" },
                { "Answer2", "1" },
                { "Answer3", "0" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which particle does not contribute mass to the atom?" },
                { "correctAnswer", "C" },
                { "Answer0", "Proton" },
                { "Answer1", "Electron" },
                { "Answer2", "Neutron" },
                { "Answer3", "Nucleus" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which particle contributes the negative charge to the atom?" },
                { "correctAnswer", "B" },
                { "Answer0", "proton" },
                { "Answer1", "electron" },
                { "Answer2", "neutron" },
                { "Answer3", "nucleus" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "14. Most of the mass in an atom is made up of _________?" },
                { "correctAnswer", "D" },
                { "Answer0", "protons and electrons" },
                { "Answer1", "electrons and quarks" },
                { "Answer2", "neutrons and electrons" },
                { "Answer3", "protons and neutrons" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "What is the standard unit of mass at the atomic level?" },
                { "correctAnswer", "A" },
                { "Answer0", "amu" },
                { "Answer1", "centimeters" },
                { "Answer2", "millimeters" },
                { "Answer3", "milligrams" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "Which one is not a Chemistry career?" },
                { "correctAnswer", "B" },
                { "Answer0", "Lecturer" },
                { "Answer1", "Actuary" },
                { "Answer2", "Farmer" },
                { "Answer3", "Anesthetist" },
            }
        },
        {
            new Dictionary<string, string>(){
                { "Question", "These chemicals are used in food processing except.." },
                { "correctAnswer", "B" },
                { "Answer0", "Sodium chloride" },
                { "Answer1", "Sodium sulphate" },
                { "Answer2", "Carbon dioxide" },
                { "Answer3", "Sodium bicarbonate" },
            }
        }
    };
}

public class QuizHandler : MonoBehaviour
{
    public GameObject Overall;
    private void Start()
    {
        if (!QuizData.hasLoaded)
        {
            Load();
            QuizData.Quizzes = QuizData.CopyQuizzes;
            QuizData.Quest = QuizData.Quizzes.Count;
            generateLevel();
            QuizData.hasLoaded = true;
        }
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