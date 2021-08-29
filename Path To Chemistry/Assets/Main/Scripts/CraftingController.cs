using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    private Transform recipes;
    private GameObject firstRecipeContainer;
    private string activeRecipe;
    private GameObject recipeInfo;
    private GameObject firstRecipeMaterialContainer;
    private GameObject recipeMaterials;
    
    void Awake()
    {
        recipes = transform.Find("Recipes Container").GetChild(0).GetChild(0).GetChild(0);
        firstRecipeContainer = recipes.GetChild(0).gameObject;

        recipeInfo = transform.Find("Recipe Info").gameObject;
        recipeMaterials = recipeInfo.transform.Find("Recipe Materials Container").GetChild(0).GetChild(0).GetChild(0).gameObject;
        firstRecipeMaterialContainer = recipeMaterials.transform.GetChild(0).gameObject;
    }
    
    void OnEnable()
    {
        activeRecipe = null;
        UpdateRecipesView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateRecipesView()
    {
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalRecipes = playerData.survivalRecipes;

        foreach (var recipe in survivalRecipes)
        {
            if ((bool) recipe.Value["enabled"])
            {
                var item = survivalInventory[recipe.Value["name"].ToString()];
                if (!firstRecipeContainer.activeSelf)
                {
                    firstRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>(item["image"].ToString());
                    firstRecipeContainer.transform.Find("Recipe Name").GetComponent<Text>().text =
                        item["name"].ToString();
                    firstRecipeContainer.SetActive(true);
                    firstRecipeContainer.name = item["name"].ToString();
                    continue;
                }
                var newRecipeContainer = Instantiate(firstRecipeContainer);
                newRecipeContainer.transform.SetParent(recipes);
                
                newRecipeContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>(item["image"].ToString());
                newRecipeContainer.transform.Find("Recipe Name").GetComponent<Text>().text =
                    item["name"].ToString();
                newRecipeContainer.name = item["name"].ToString();
                newRecipeContainer.SetActive(true);
            }
        }
    }

    void UpdateRecipeInfoView()
    {
        firstRecipeMaterialContainer.SetActive(false);
        foreach (Transform recipeMaterialContainer in recipeMaterials.transform)
        {
            if (recipeMaterialContainer.GetSiblingIndex() != 0)
            {
                GameObject.Destroy(recipeMaterialContainer.gameObject);
            }
        }
        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(false);
        recipeInfo.transform.Find("Recipe Name").GetComponent<Text>().text = "";

        if (activeRecipe == null) return;

        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalRecipes = playerData.survivalRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).GetComponent<Image>().sprite =
            Resources.Load<Sprite>(survivalInventory[activeRecipe]["image"].ToString());
        recipeInfo.transform.Find("Recipe Output").GetChild(0).GetChild(0).gameObject.SetActive(true);
        recipeInfo.transform.Find("Recipe Name").GetComponent<Text>().text = activeRecipe;
        
        foreach (var material in (Dictionary<string, int>) survivalRecipes[activeRecipe]["materials"])
        {
                if (!firstRecipeMaterialContainer.activeSelf)
                {
                    firstRecipeMaterialContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(survivalMaterials[material.Key]["image"].ToString());
                    firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().text = "x " + material.Value;
                    if (material.Value > Int32.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                    {
                        firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                            Color.red;
                    }
                    else
                    {
                        firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                            Color.black;
                    }
                    firstRecipeMaterialContainer.SetActive(true);
                    continue;
                }

                var newRecipeMaterialsContainer = Instantiate(firstRecipeMaterialContainer);
                newRecipeMaterialsContainer.transform.SetParent(recipeMaterials.transform);
                newRecipeMaterialsContainer.transform.Find("Image Container").GetChild(0).GetComponent<Image>()
                        .sprite = Resources.Load<Sprite>(survivalMaterials[material.Key]["image"].ToString());
                newRecipeMaterialsContainer.transform.Find("Material Count").GetComponent<Text>().text = "x " + material.Value;
                if (material.Value > Int32.Parse(survivalMaterials[material.Key]["quantity"].ToString()))
                {
                    firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.red;
                }
                else
                {
                    firstRecipeMaterialContainer.transform.Find("Material Count").GetComponent<Text>().color =
                        Color.black;
                }
                newRecipeMaterialsContainer.SetActive(true);
        }
    }

    public void CraftButtonHandler()
    {
        if (activeRecipe == null) return;
        
        var playerData = PlayerData.Instance();
        var survivalInventory = playerData.survivalInventory;
        var survivalRecipes = playerData.survivalRecipes;
        var survivalMaterials = playerData.survivalMaterials;

        foreach (var recipeMaterial in (Dictionary<string, int>) survivalRecipes[activeRecipe]["materials"])
        {
            if (recipeMaterial.Value < Int32.Parse(survivalMaterials[recipeMaterial.Key]["quantity"].ToString()))
            {
                return;
            }
        }

        survivalInventory[activeRecipe]["quantity"] = Int32.Parse(survivalInventory[activeRecipe]["quantity"].ToString()) + 1;
    }

    public void RecipeClickHandler(GameObject recipeContainer)
    {
        activeRecipe = recipeContainer.name;
        UpdateRecipeInfoView();
    }
}
