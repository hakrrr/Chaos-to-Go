using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    public int MAX_INGREDIENTS = 3;

    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private ParticleSystem particlesBurst;

    [SerializeField]
    private Sprite[] iconTextures;
    [SerializeField]
    private SpriteRenderer[] iconSlots;

    [SerializeField]
    private GameObject pointLabelPrefab;

    private Recipes.eIngredients[] inPot; 


    public void AddIngredient(Recipes.eIngredients ingredient)
    {
        particlesBurst.Play();
        particles.Play();

        for (int i = 0; i < MAX_INGREDIENTS; i++)
        {
            if(inPot[i] == Recipes.eIngredients.empty)
            {
                inPot[i] = ingredient;
                break;
            }
            if (i == MAX_INGREDIENTS - 1)
            {
                if (!CheckRecipe())
                {
                    EmptyCookingPlace(true);
                }
                AddIngredient(ingredient);
            }
        }

        if(inPot[0] != Recipes.eIngredients.empty)
        {
            effect.transform.localScale = new Vector3(3.7f, 0.5f, 3.7f);
        }
    }


    public void EmptyCookingPlace(bool penalty)
    {
        particlesBurst.Play();
        particles.Stop();
        int p = 0;
        foreach(Recipes.eIngredients ingr in inPot){
            if (ingr != Recipes.eIngredients.empty) p += Ingredient.INGREDIENT_PENALTY;
        }
        if (penalty)
        {
            Game.GAME.AddScore(-p);
            PointLabel.SpawnAt(pointLabelPrefab, transform.parent, transform.position, -p);
        }
        effect.transform.localScale = Vector3.zero;
        inPot = new Recipes.eIngredients[MAX_INGREDIENTS];
        for (int i = 0; i < MAX_INGREDIENTS; i++)
        {
            inPot[i] = Recipes.eIngredients.empty;
        }
    }


    private bool CheckRecipe()
    {
        for (int recipeIdx = 0; recipeIdx < Game.GAME.GetFoodOrders().Length; recipeIdx++)
        {
            Recipes.Recipe recipe = Game.GAME.GetFoodOrders()[recipeIdx];
            Recipes.eIngredients[] recipeIngr = new Recipes.eIngredients[3];
            recipeIngr[0] = recipe.ingredient1;
            recipeIngr[1] = recipe.ingredient2;
            recipeIngr[2] = recipe.ingredient3;
            bool recipeFit = true;
            foreach (Recipes.eIngredients ingr in inPot)
            {
                if (CountIngredient(ingr, recipeIngr) != CountIngredient(ingr, inPot))
                {
                    recipeFit = false;
                    break;
                }
            }

            if (recipeFit)
            {
                PointLabel.SpawnAt(pointLabelPrefab, transform.parent, transform.position, recipe.points);
                Game.GAME.AddScore(recipe.points);
                Game.GAME.NextRecipe(recipeIdx);
                EmptyCookingPlace(false);
                return true;
            }
        }
        return false;
    }


    private int CountIngredient(Recipes.eIngredients elem, Recipes.eIngredients[] arr)
    {
        int c = 0;
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] == elem)
            {
                c++;
            }
        }
        return c;
    }


    private void UpdateIcons()
    {
        for(int i = 0; i < 3; i++)
        {
            if (inPot[i] == Recipes.eIngredients.empty) {
                iconSlots[i].sprite = null;
                continue;
            }
            iconSlots[i].sprite = iconTextures[(int)inPot[i] - 1];
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        effect.transform.localScale = Vector3.zero;
        inPot = new Recipes.eIngredients[MAX_INGREDIENTS];
        for(int i = 0; i < MAX_INGREDIENTS; i++)
        {
            inPot[i] = Recipes.eIngredients.empty;
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateIcons();
    }


    void OnMouseDown()
    {
        if (PauseMenu.PAUSED)
        {
            return;
        }
        if (inPot[0] != Recipes.eIngredients.empty && !CheckRecipe())
        {
            EmptyCookingPlace(true);
        }
    }
}
