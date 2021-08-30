using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SmeltingController : MonoBehaviour
{
    private string activeSmeltingRecipe;
    private GameObject firstSmeltingRecipeContainer;
    private GameObject firstSmeltingRecipeMaterialContainer;
    private GameObject smeltingRecipeInfo;
    private GameObject smeltingRecipeMaterials;
    private Transform smeltingRecipes;

    private void Awake()
    {
        smeltingRecipes = transform.Find("Smelting Recipes Container").GetChild(0).GetChild(0).GetChild(0);
        firstSmeltingRecipeContainer = smeltingRecipes.GetChild(0).gameObject;
        smeltingRecipeInfo = transform.Find("Smelting Recipe Info").gameObject;
        smeltingRecipeMaterials = smeltingRecipeInfo.transform.Find("Smelting Recipe Materials Container").GetChild(0)
            .GetChild(0).GetChild(0)
            .gameObject;
        firstSmeltingRecipeMaterialContainer = smeltingRecipeMaterials.transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        Load();

        activeSmeltingRecipe = null;
        UpdateRecipesView();
    }

    private void Load()
    {
        var directory = $"{Application.persistentDataPath}/Data";
        var filePath = Path.Combine(directory, "Saves.json");
        var fileContent = File.ReadAllText(filePath);
        var playerData = JsonConvert.DeserializeObject<PlayerData>(fileContent);
        PlayerData.Instance().UpdatePlayerData(playerData);
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

    private void UpdateRecipesView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalSmeltingRecipes = playerData.survivalSmeltingRecipes;

        foreach (var recipe in survivalSmeltingRecipes)
            if ((bool) recipe.Value["enabled"])
            {
                var item = survivalInventory[recipe.Value["name"].ToString()];
                if (!firstSmeltingRecipeContainer.activeSelf)
                {
                    firstSmeltingRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                            .sprite =
                        Resources.Load<Sprite>(item["image"].ToString());
                    firstSmeltingRecipeContainer.transform.Find("Smelting Recipe Name").GetComponent<Text>().text =
                        item["name"].ToString();
                    firstSmeltingRecipeContainer.SetActive(true);
                    firstSmeltingRecipeContainer.name = item["name"].ToString();
                    continue;
                }

                var newRecipeContainer = Instantiate(firstSmeltingRecipeContainer);
                newRecipeContainer.transform.SetParent(smeltingRecipes);

                newRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>(item["image"].ToString());
                newRecipeContainer.transform.Find("Smelting Recipe Name").GetComponent<Text>().text =
                    item["name"].ToString();
                newRecipeContainer.name = item["name"].ToString();
                newRecipeContainer.SetActive(true);
            }
    }

    private void UpdateRecipeInfoView()
    {
        firstSmeltingRecipeMaterialContainer.SetActive(false);
        foreach (Transform recipeMaterialContainer in smeltingRecipeMaterials.transform)
            if (recipeMaterialContainer.GetSiblingIndex() != 0)
                Destroy(recipeMaterialContainer.gameObject);
        smeltingRecipeInfo.transform.Find("Smelting Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(false);
        smeltingRecipeInfo.transform.Find("Smelting Recipe Name").GetComponent<Text>().text = "";

        if (activeSmeltingRecipe == null) return;

        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalSmeltingRecipes = playerData.survivalSmeltingRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        smeltingRecipeInfo.transform.Find("Smelting Recipe Output").GetChild(0).GetChild(0).GetComponent<Image>()
                .sprite =
            Resources.Load<Sprite>(survivalInventory[activeSmeltingRecipe]["image"].ToString());
        smeltingRecipeInfo.transform.Find("Smelting Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(true);
        smeltingRecipeInfo.transform.Find("Smelting Recipe Name").GetComponent<Text>().text = activeSmeltingRecipe;

        var JrecipeMaterials = (JObject) survivalSmeltingRecipes[activeSmeltingRecipe]["materials"];
        var dictRecipeMaterials = JrecipeMaterials.ToObject<Dictionary<string, int>>();
        foreach (var material in dictRecipeMaterials)
        {
            if (!firstSmeltingRecipeMaterialContainer.activeSelf)
            {
                firstSmeltingRecipeMaterialContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                    .sprite = Resources.Load<Sprite>(survivalMaterials[material.Key]["image"].ToString());
                firstSmeltingRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().text =
                    "x " + material.Value;
                if (material.Value > int.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                    firstSmeltingRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.red;
                else
                    firstSmeltingRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.black;
                firstSmeltingRecipeMaterialContainer.SetActive(true);
                continue;
            }

            var newRecipeMaterialsContainer = Instantiate(firstSmeltingRecipeMaterialContainer);
            newRecipeMaterialsContainer.transform.SetParent(smeltingRecipeMaterials.transform);
            newRecipeMaterialsContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                .sprite = Resources.Load<Sprite>(survivalMaterials[material.Key]["image"].ToString());
            newRecipeMaterialsContainer.transform.Find("Material Count").GetComponent<Text>().text =
                "x " + material.Value;
            if (material.Value > int.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                firstSmeltingRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                    Color.red;
            else
                firstSmeltingRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                    Color.black;
            newRecipeMaterialsContainer.SetActive(true);
        }
    }

    public void SmeltButtonHandler()
    {
        if (activeSmeltingRecipe == null) return;

        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalSmeltingRecipes = playerData.survivalSmeltingRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        var JrecipeMaterials = (JObject) survivalSmeltingRecipes[activeSmeltingRecipe]["materials"];
        var dictRecipeMaterials = JrecipeMaterials.ToObject<Dictionary<string, int>>();
        foreach (var recipeMaterial in dictRecipeMaterials)
            if (recipeMaterial.Value > int.Parse(survivalMaterials[recipeMaterial.Key]["quantity"].ToString()))
                return;

        survivalInventory[activeSmeltingRecipe]["quantity"] =
            int.Parse(survivalInventory[activeSmeltingRecipe]["quantity"].ToString()) + 1;

        foreach (var recipeMaterial in dictRecipeMaterials)
            survivalMaterials[recipeMaterial.Key]["quantity"] =
                int.Parse(survivalMaterials[recipeMaterial.Key]["quantity"].ToString()) - recipeMaterial.Value;

        // StartCoroutine(Countdown(3f));
        UpdateRecipeInfoView();
    }

    public void SmeltRecipeClickHandler(GameObject recipeContainer)
    {
        activeSmeltingRecipe = recipeContainer.name;
        UpdateRecipeInfoView();
    }

    private IEnumerator Countdown(float duration)
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            print(normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        print("Complete");
    }
}