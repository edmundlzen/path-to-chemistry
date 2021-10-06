using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialReducer : MonoBehaviour
{
    private string activeRecipe;
    private NotificationsController notificationsController;
    private GameObject firstRecipeContainer;
    private GameObject firstRecipeMaterialContainer;
    private GameObject recipeInfo;
    private GameObject recipeMaterials;
    private Transform recipes;

    private void Awake()
    {
        notificationsController = GameObject.FindGameObjectsWithTag("NotificationsController")[0].transform.GetComponent<NotificationsController>();
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
        var survivalMaterials = playerData.survivalMaterials;
        var materialReducerRecipes = playerData.materialReducerRecipes;
        
        firstRecipeContainer.gameObject.SetActive(false);
        foreach (Transform recipeContainer in recipes)
        {
            if (recipeContainer.GetSiblingIndex() != 0)
            {
                Destroy(recipeContainer.gameObject);
            }
        }

        foreach (var recipe in materialReducerRecipes)
        {
            if ((bool) recipe.Value["enabled"] && Int32.Parse(survivalMaterials[recipe.Key]["quantity"].ToString()) > 0)
            {
                var item = survivalMaterials[recipe.Value["name"].ToString()];
                if (!firstRecipeContainer.activeSelf)
                {
                    firstRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Sprites/" + item["image"].ToString());
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
        var materialReducerRecipes = playerData.materialReducerRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        if (Int32.Parse(survivalMaterials[activeRecipe]["quantity"].ToString()) <= 0) return;

        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).GetComponent<Image>().sprite =
            Resources.Load<Sprite>("Sprites/" + survivalMaterials[activeRecipe]["image"].ToString());
        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(true);
        recipeInfo.transform.Find("Recipe Name").GetComponent<Text>().text = activeRecipe;

        // JObject JmaterialReducerRecipes = (JObject) materialReducerRecipes[activeRecipe]["elements"];
        var  materialReducerElements = JsonConvert.DeserializeObject<Dictionary<string, int>>(materialReducerRecipes[activeRecipe]["elements"].ToString());
        foreach (var element in materialReducerElements)
        {
            if (!firstRecipeMaterialContainer.activeSelf)
            {
                firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().text =
                    element.Key + " x " + element.Value;
                firstRecipeMaterialContainer.name = element.Key;
                firstRecipeMaterialContainer.SetActive(true);
                continue;
            }

            var newRecipeMaterialsContainer = Instantiate(firstRecipeMaterialContainer);
            newRecipeMaterialsContainer.transform.SetParent(recipeMaterials.transform, false);
            newRecipeMaterialsContainer.transform.Find("Material Count").GetComponent<Text>().text =
                element.Key + " x " + element.Value;
            newRecipeMaterialsContainer.name = element.Key;
            newRecipeMaterialsContainer.SetActive(true);
        }
    }

    public void ReduceButtonHandler()
    {
        if (activeRecipe == null) return;

        var playerData = PlayerData.Instance();
        var elementInventory  = playerData.Inventory;
        var materialReducerRecipes = playerData.materialReducerRecipes;
        var survivalMaterials = playerData.survivalMaterials;
        
        if (Int32.Parse(survivalMaterials[activeRecipe]["quantity"].ToString()) <= 0) return;

        // JObject JrecipeMaterials = (JObject) survivalPlayerRecipes[activeRecipe]["materials"];
        // var dictRecipeMaterials = JrecipeMaterials.ToObject<Dictionary<string, int>>();
        survivalMaterials[activeRecipe]["quantity"] =
            Int32.Parse(survivalMaterials[activeRecipe]["quantity"].ToString()) - 1;
        notificationsController.SendImageNotification(Resources.Load<Sprite>("Sprites/" + survivalMaterials[activeRecipe]["image"]), "- 1", "red");
        var returnElements =
            JsonConvert.DeserializeObject<Dictionary<string, int>>(materialReducerRecipes[activeRecipe]["elements"].ToString());
        foreach (var element in returnElements)
        {
            print(element.Key);
            print(elementInventory.Count);
            elementInventory[element.Key] += 1;
            notificationsController.SendTextImageNotification(element.Key, "+ " + element.Value, "green");
        }
        UpdateRecipeInfoView();
        UpdateRecipesView();
    }

    public void RecipeClickHandler(GameObject recipeContainer)
    {
        activeRecipe = recipeContainer.name;
        UpdateRecipeInfoView();
    }
}