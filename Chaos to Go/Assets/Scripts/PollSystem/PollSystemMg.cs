using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TwitchChat
{
    public class PollSystemMg : MonoBehaviour
    {
        [SerializeField]
        private Text IngCDText;
        [SerializeField]
        private Text[] SpawnPoints;
        [SerializeField]
        private Image[] IngredientSlot;
        [SerializeField]
        private Sprite[] IconTextures;


        private Recipes.eIngredients eIng;

        private float IngCD;
        private float MaxCD = 30;
        private int MaxSpawns = 5;

        private string[] ingredients = { "tomato", "chicken", "onion", "carrot", "asparagus" };
        private string[] emotes = { "1", "2", "3", "4", "5" };
        private string[] currentEmotes = new string[3];
        private int[] voteCounter = new int[3] { 0, 0, 0 };
        private spawnInfo[] choices = new spawnInfo[3];

        struct spawnInfo
        {
            public string IngredientName;
            public int SpawnPoint;
            public spawnInfo(string name, int point)
            {
                this.IngredientName = name; 
                this.SpawnPoint = point;
            }
        }

        void Start()
        {
            ResetPoll();
        }

        void Update()
        {
            if (IngCD > 0)
            {
                //CD
                IngCD -= Time.deltaTime;
                IngCDText.GetComponent<Text>().text = IngCD.ToString();
            }
            else
            {
                EvalVote();
                ResetPoll();
            }
        }

        void EvalVote()
        {
            //Print out the votes
            for (int i = 0; i < 3; i++) Debug.Log("Ingredient " + (i+1) +" has " + voteCounter[i] + " votes");
            //max voteCounter => spawnInfo
            int maxValue = voteCounter.Max();
            int maxIndex = voteCounter.ToList().IndexOf(maxValue);
            
            //@Dorota
            spawnInfo result = choices[maxIndex];
            //Find the spawning script and spawn ingredients according to poll results
            GameObject ingredientsManager = GameObject.Find("IngredientsManager");
            IngredientsManager ingredientSpawningScript = ingredientsManager.GetComponent<IngredientsManager>();
            ingredientSpawningScript.SpawnIngredient(result.IngredientName, result.SpawnPoint);

            Debug.Log("Result: " + result.IngredientName + " " + result.SpawnPoint);
            Debug.Log("-------------------------");
        }

        private void ResetPoll()
        {
            //Reset CD
            IngCD = MaxCD;

            //Generate 3 Random Emotes for VoteCasting
            var result = Enumerable.Range(0, emotes.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) currentEmotes[i] = emotes[result[i]];
            
            //Print out current Emotes
            Debug.Log("Current Voting Emotes are: ");
            for (int i = 0; i < 3; i++) Debug.Log(currentEmotes[i]);

            //Generate 3 random ingredients with spawnPoints
            var rngIng = Enumerable.Range(0, ingredients.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            var rngSpawn = Enumerable.Range(1, MaxSpawns).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) choices[i] = new spawnInfo(ingredients[rngIng[i]], rngSpawn[i]);

            Debug.Log("Current Choices: ");
            for (int i = 0; i < 3; i++) Debug.Log(choices[i].IngredientName + " " + choices[i].SpawnPoint);

            //TODO Update Images according to ing + spawnPoints & emotes
            for (int i = 0; i < choices.Length; i++)
            {
                SpawnPoints[i].GetComponent<Text>().text = choices[i].SpawnPoint.ToString();
                var e = Enum.Parse(typeof(Recipes.eIngredients), choices[i].IngredientName);
                IngredientSlot[i].sprite = IconTextures[(int)e - 1];
            }

            //Reset VoteCounter
            for (int i = 0; i < 3; i++) voteCounter[i] = 0;
        }

        private void OnEnable()
        {
            TwitchCommander.Instance.OnMessage += OnMessage;
        }

        private void OnDisable()
        {
            TwitchCommander.Instance.OnMessage -= OnMessage;
        }

        void OnMessage(string msg)
        {
            //Counting the votes if expected emote was commented
            int pos = Array.IndexOf(currentEmotes, msg);
            if (pos > -1) voteCounter[pos]++;
        }
    }
}
