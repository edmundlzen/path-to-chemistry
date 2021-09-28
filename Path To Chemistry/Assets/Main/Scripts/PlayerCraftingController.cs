using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCraftingController : MonoBehaviour
{
    private string activeRecipe;
    private GameObject firstRecipeContainer;
    private GameObject firstRecipeMaterialContainer;
    private GameObject recipeInfo;
    private GameObject recipeMaterials;
    private Transform recipes;
    
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

    private void Awake()
    {
        Save();
        Load();
        recipes = transform.Find("Recipes Container").GetChild(0).GetChild(0).GetChild(0);
        firstRecipeContainer = recipes.GetChild(0).gameObject;

        recipeInfo = transform.Find("Recipe Info").gameObject;
        recipeMaterials = recipeInfo.transform.Find("Recipe Materials Container").GetChild(0).GetChild(0).GetChild(0)
            .gameObject;
        firstRecipeMaterialContainer = recipeMaterials.transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        activeRecipe = null;
        UpdateRecipesView();
        
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    
    private void UpdateRecipesView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalRecipes = playerData.survivalPlayerRecipes;
        
        firstRecipeContainer.gameObject.SetActive(false);
        foreach (Transform recipeContainer in recipes)
        {
            if (recipeContainer.GetSiblingIndex() != 0)
            {
                Destroy(recipeContainer.gameObject);
            }
        }

        foreach (var recipe in survivalRecipes)
            if ((bool) recipe.Value["enabled"])
            {
                var item = survivalInventory[recipe.Value["name"].ToString()];
                if (!firstRecipeContainer.activeSelf)
                {
                    firstRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Sprites/" + item["image"]);
                    firstRecipeContainer.transform.Find("Recipe Name").GetComponent<Text>().text =
                        item["name"].ToString();
                    firstRecipeContainer.SetActive(true);
                    firstRecipeContainer.name = item["name"].ToString();
                    continue;
                }

                var newRecipeContainer = Instantiate(firstRecipeContainer);
                newRecipeContainer.transform.SetParent(recipes, false);

                newRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Sprites/" + item["image"].ToString());
                newRecipeContainer.transform.Find("Recipe Name").GetComponent<Text>().text =
                    item["name"].ToString();
                newRecipeContainer.name = item["name"].ToString();
                newRecipeContainer.SetActive(true);
            }
    }

    private void UpdateRecipeInfoView()
    {
        firstRecipeMaterialContainer.SetActive(false);
        foreach (Transform recipeMaterialContainer in recipeMaterials.transform)
            if (recipeMaterialContainer.GetSiblingIndex() != 0)
                Destroy(recipeMaterialContainer.gameObject);
        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(false);
        recipeInfo.transform.Find("Recipe Name").GetComponent<Text>().text = "";

        if (activeRecipe == null) return;

        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalRecipes = playerData.survivalPlayerRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).GetComponent<Image>().sprite =
            Resources.Load<Sprite>("Sprites/" + survivalInventory[activeRecipe]["image"].ToString());
        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(true);
        recipeInfo.transform.Find("Recipe Name").GetComponent<Text>().text = activeRecipe;

        JObject JrecipeMaterials = (JObject) survivalRecipes[activeRecipe]["materials"];
        var dictRecipeMaterials = JrecipeMaterials.ToObject<Dictionary<string, int>>();
        foreach (var material in dictRecipeMaterials)
        {
            if (!firstRecipeMaterialContainer.activeSelf)
            {
                firstRecipeMaterialContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                    .sprite = Resources.Load<Sprite>("Sprites/" + survivalMaterials[material.Key]["image"].ToString());
                firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().text =
                    "x " + material.Value;
                if (material.Value > int.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                {
                    firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.red;
                }
                else
                    firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.black;

                firstRecipeMaterialContainer.name = material.Key;
                firstRecipeMaterialContainer.SetActive(true);
                continue;
            }

            var newRecipeMaterialsContainer = Instantiate(firstRecipeMaterialContainer);
            newRecipeMaterialsContainer.transform.SetParent(recipeMaterials.transform, false);
            newRecipeMaterialsContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                .sprite = Resources.Load<Sprite>("Sprites/" + survivalMaterials[material.Key]["image"].ToString());
            newRecipeMaterialsContainer.transform.Find("Material Count").GetComponent<Text>().text =
                "x " + material.Value;
            if (material.Value > int.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                    Color.red;
            else
                firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                    Color.black;

            newRecipeMaterialsContainer.name = material.Key;
            newRecipeMaterialsContainer.SetActive(true);
        }
    }

    public void CraftButtonHandler()
    {
        if (activeRecipe == null) return;

        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalPlayerRecipes = playerData.survivalPlayerRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        JObject JrecipeMaterials = (JObject) survivalPlayerRecipes[activeRecipe]["materials"];
        var dictRecipeMaterials = JrecipeMaterials.ToObject<Dictionary<string, int>>();
        foreach (var recipeMaterial in dictRecipeMaterials)
        {
            if (recipeMaterial.Value > int.Parse(survivalMaterials[recipeMaterial.Key]["quantity"].ToString()))
            {
                return;
            }
        }
        
        survivalInventory[activeRecipe]["quantity"] =
            int.Parse(survivalInventory[activeRecipe]["quantity"].ToString()) + 1;

        foreach (var recipeMaterial in dictRecipeMaterials)
        {
            survivalMaterials[recipeMaterial.Key]["quantity"] =
                int.Parse(survivalMaterials[recipeMaterial.Key]["quantity"].ToString()) - recipeMaterial.Value;
        }
        UpdateRecipeInfoView();
    }

    public void RecipeClickHandler(GameObject recipeContainer)
    {
        activeRecipe = recipeContainer.name;
        UpdateRecipeInfoView();
    }
}