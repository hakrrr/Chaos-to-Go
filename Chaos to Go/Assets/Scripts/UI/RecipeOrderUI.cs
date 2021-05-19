using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeOrderUI : MonoBehaviour
{ 
    [SerializeField]
    private Sprite[] iconTextures;
    [SerializeField]
    private Image[] iconSlots;
    [SerializeField]
    private Image[] plusSigns;


    public void ResetRecipeUI(int idx)
    {
        plusSigns[(Game.RECIPE_MAX_SIZE - 1) * idx].enabled = false;
        plusSigns[(Game.RECIPE_MAX_SIZE - 1) * idx + 1].enabled = false;
        iconSlots[Game.RECIPE_MAX_SIZE * idx].enabled = false;
        iconSlots[Game.RECIPE_MAX_SIZE * idx + 1].enabled = false;
        iconSlots[Game.RECIPE_MAX_SIZE * idx + 2].enabled = false;
    }


    public void ShowRecipe(Recipes.Recipe recipe, int idx)
    {
        ResetRecipeUI(idx);
        // Hide plus-icons when necessary
        if(recipe.ingredient1 != Recipes.eIngredients.empty)
        {
            iconSlots[Game.RECIPE_MAX_SIZE * idx].sprite = iconTextures[(int)recipe.ingredient1 - 1];
            iconSlots[Game.RECIPE_MAX_SIZE * idx].enabled = true;
        }
        if(recipe.ingredient2 != Recipes.eIngredients.empty)
        {
            iconSlots[Game.RECIPE_MAX_SIZE * idx + 1].sprite = iconTextures[(int)recipe.ingredient2 - 1];
            iconSlots[Game.RECIPE_MAX_SIZE * idx + 1].enabled = true;
            plusSigns[(Game.RECIPE_MAX_SIZE - 1) * idx].enabled = true;
        }
        if (recipe.ingredient3 != Recipes.eIngredients.empty)
        {
            iconSlots[Game.RECIPE_MAX_SIZE * idx + 2].sprite = iconTextures[(int)recipe.ingredient3 - 1];
            iconSlots[Game.RECIPE_MAX_SIZE * idx + 2].enabled = true;
            plusSigns[(Game.RECIPE_MAX_SIZE - 1) * idx + 1].enabled = true;
        }
    }


    public void Start()
    {
        ResetRecipeUI(0);
        ResetRecipeUI(1);
        ResetRecipeUI(2);
    }
}