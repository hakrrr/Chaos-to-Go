using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game GAME;
    public static GameBoard BOARD;

    public const int RECIPE_MAX_SIZE = 3;

    [SerializeField]
    private GameBoard gameBoard;
    [SerializeField]
    private GameConsoleHandler console;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private RecipeOrderUI recipeOrderUI;
    [SerializeField]
    private MinuteSecondTimer gameTimer;


    private int score = 0;
    private Recipes.Recipe[] foodOrders = new Recipes.Recipe[3];


    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }


    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }


    public int GetScore()
    {
        return score;
    }


    public void ShowDebugConsole()
    {
        console.Show();
    }


    public void SetRecipe(int recipeIdx, Recipes.Recipe recipe)
    {
        recipeOrderUI.ShowRecipe(recipe, recipeIdx);
        foodOrders[recipeIdx] = recipe;
    }


    public Recipes.Recipe[] GetFoodOrders()
    {
        return foodOrders;
    }


    public void NextRecipe(int recipeIdx)
    {
        SetRecipe(recipeIdx, Recipes.RECIPES[(int)Random.Range(0.0f, Recipes.RECIPES.Length)]);
    }


    private void UpdateScoreText()
    {
        scoreText.text = "" + score;
    }


    // Start is called before the first frame update
    void Start()
    {
        GAME = this;
        console.transform.localScale = new Vector3(1, 1, 1);
    }


    bool recipesSet = false;
    // Update is called once per frame
    void Update()
    {
        // For demonstration purposes...
        if (!recipesSet)
        {
            NextRecipe(0);
            NextRecipe(1);
            NextRecipe(2);
            recipesSet = true;
        }

        if (gameTimer.IsExpired())
        {
            GameOverScreen.GameOver(score);
        }
    }
}
