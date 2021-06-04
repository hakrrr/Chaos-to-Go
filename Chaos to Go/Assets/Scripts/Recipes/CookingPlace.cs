﻿using System.Collections;
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
        }

        if(inPot[0] != Recipes.eIngredients.empty)
        {
            effect.transform.localScale = new Vector3(3.7f, 0.5f, 3.7f);
        }
        if(inPot[1] != Recipes.eIngredients.empty)
        {
            CheckRecipe();
        }

        int c_empty = 0;
        for(int i = 0; i < MAX_INGREDIENTS; i++)
        {
            if(inPot[i] != Recipes.eIngredients.empty)
            {
                c_empty++;
            }
        }
        if(c_empty == MAX_INGREDIENTS)
        {
            inPot = new Recipes.eIngredients[MAX_INGREDIENTS];
            for (int i = 0; i < MAX_INGREDIENTS; i++)
            {
                inPot[i] = Recipes.eIngredients.empty;
            }
        }
    }


    public void EmptyCookingPlace()
    {

    }


    private void CheckRecipe()
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
                Game.GAME.AddScore(recipe.points);
                Game.GAME.NextRecipe(recipeIdx);
                effect.transform.localScale = Vector3.zero;
                particles.Stop();
                particlesBurst.Play();
                inPot = new Recipes.eIngredients[MAX_INGREDIENTS];
                for (int i = 0; i < MAX_INGREDIENTS; i++)
                {
                    inPot[i] = Recipes.eIngredients.empty;
                }
                return;
            }
        }
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
}
