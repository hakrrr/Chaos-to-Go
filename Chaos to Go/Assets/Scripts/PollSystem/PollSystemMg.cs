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
        private Text TileCDText;
        [SerializeField]
        private Text[] SpawnPoints;
        [SerializeField]
        private Image[] IngredientSlot;
        [SerializeField]
        private Image[] TileSlot;
        [SerializeField]
        private Image[] IngEmoteSlot;
        [SerializeField]
        private Image[] TileEmoteSlot;
        [SerializeField]
        private Sprite[] IngredientTextures;
        [SerializeField]
        private Sprite[] TileTextures;
        [SerializeField]
        private Sprite[] EmoteTextures;

        private enum eEmote
        {
            LUL,
            MonkaS,
            KEKW,
            Kappa,
            TriHard
        }

        private float IngCD;
        private float IngMaxCD = 30;
        private int MaxSpawns = 5;

        private float TileCD;
        private float TileMaxCD = 15;

        private string[] ingredients = { "tomato", "chicken", "onion", "carrot", "asparagus" };
        private string[] tiles = {"topdown",
        "downtop",
        "leftright",
        "rightleft",
        "topleft",
        "topright",
        "downleft",
        "downright",
        "lefttop",
        "leftdown",
        "righttop",
        "rightdown" };
        private string[] emotes = { "LUL", "MonkaS", "KEKW", "Kappa", "TriHard" };
        private string[] ingCurrentEmotes = new string[3];
        private string[] tileCurrentEmotes = new string[3];
        private int[] ingVoteCounter = new int[3] { 0, 0, 0 };
        private int[] tileVoteCounter = new int[3] { 0, 0, 0 };
        private ingSpawnInfo[] choices = new ingSpawnInfo[3];

        struct ingSpawnInfo
        {
            public string IngredientName;
            public int SpawnPoint;
            public ingSpawnInfo(string name, int point)
            {
                this.IngredientName = name; 
                this.SpawnPoint = point;
            }
        }

        void Start()
        {
            IngResetPoll();
            TileResetPoll();
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
                IngEvalVote();
                IngResetPoll();
            }

            if (TileCD > 0)
            {
                //CD
                TileCD -= Time.deltaTime;
                TileCDText.GetComponent<Text>().text = TileCD.ToString();
            }
            else
            {
                TileEvalVote();
                TileResetPoll();
            }
        }

        void IngEvalVote()
        {
            //Print out the votes
            for (int i = 0; i < 3; i++) Debug.Log("Ingredient " + (i+1) +" has " + ingVoteCounter[i] + " votes");
            //max voteCounter => spawnInfo
            int maxValue = ingVoteCounter.Max();
            int maxIndex = ingVoteCounter.ToList().IndexOf(maxValue);
            
            //@Dorota
            ingSpawnInfo result = choices[maxIndex];
            //Find the spawning script and spawn ingredients according to poll results
            GameObject ingredientsManager = GameObject.Find("IngredientsManager");
            IngredientsManager ingredientSpawningScript = ingredientsManager.GetComponent<IngredientsManager>();
            ingredientSpawningScript.SpawnIngredient(result.IngredientName, result.SpawnPoint);

            Debug.Log("Result: " + result.IngredientName + " " + result.SpawnPoint);
            Debug.Log("-------------------------");
        }

        void TileEvalVote()
        {
            //Print out the votes
            for (int i = 0; i < 3; i++) Debug.Log("Tile " + (i + 1) + " has " + tileVoteCounter[i] + " votes");
            //max voteCounter => spawnInfo
            int maxValue = tileVoteCounter.Max();
            int maxIndex = tileVoteCounter.ToList().IndexOf(maxValue);

         /* TODO: Spawn selected tile
          * 
          * //@Dorota
            spawnInfo result = choices[maxIndex];
            //Find the spawning script and spawn ingredients according to poll results
            GameObject ingredientsManager = GameObject.Find("IngredientsManager");
            IngredientsManager ingredientSpawningScript = ingredientsManager.GetComponent<IngredientsManager>();
            ingredientSpawningScript.SpawnIngredient(result.IngredientName, result.SpawnPoint);

            Debug.Log("Result: " + result.IngredientName + " " + result.SpawnPoint);*/
            Debug.Log("---TODO IMPLEMENT SPAWNING OF TILE----");
        }

        private void IngResetPoll()
        {
            //Reset CD
            IngCD = IngMaxCD;

            //Generate 3 Random Emotes for VoteCasting
            var result = Enumerable.Range(0, emotes.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) ingCurrentEmotes[i] = emotes[result[i]];
            
            //Print out current Emotes
            Debug.Log("Current ingVoting Emotes are: ");
            for (int i = 0; i < 3; i++) Debug.Log(ingCurrentEmotes[i]);

            //Generate 3 random ingredients with spawnPoints
            var rngIng = Enumerable.Range(0, ingredients.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            var rngSpawn = Enumerable.Range(1, MaxSpawns).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) choices[i] = new ingSpawnInfo(ingredients[rngIng[i]], rngSpawn[i]);

            Debug.Log("Current Choices: ");
            for (int i = 0; i < 3; i++) Debug.Log(choices[i].IngredientName + " " + choices[i].SpawnPoint);

            //TODO Update Images according to ing + spawnPoints & emotes
            for (int i = 0; i < choices.Length; i++)
            {
                SpawnPoints[i].GetComponent<Text>().text = choices[i].SpawnPoint.ToString();
                var e = Enum.Parse(typeof(Recipes.eIngredients), choices[i].IngredientName);
                IngredientSlot[i].sprite = IngredientTextures[(int)e - 1];
                e = Enum.Parse(typeof(eEmote), ingCurrentEmotes[i]);
                IngEmoteSlot[i].sprite = EmoteTextures[(int)e];
            }

            //Reset VoteCounter
            for (int i = 0; i < 3; i++) ingVoteCounter[i] = 0;
        }

        private void TileResetPoll()
        {
            //Reset CD
            TileCD = TileMaxCD;

            //Generate 3 Random Emotes for VoteCasting
            var result = Enumerable.Range(0, emotes.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) tileCurrentEmotes[i] = emotes[result[i]];

            //Print out current Emotes
            Debug.Log("Current tileVoting Emotes are: ");
            for (int i = 0; i < 3; i++) Debug.Log(tileCurrentEmotes[i]);

            //Generate 3 random ingredients with spawnPoints
            var rngTile = Enumerable.Range(0, tiles.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();

            Debug.Log("Current Choices: ");
            for (int i = 0; i < 3; i++) Debug.Log(tiles[rngTile[i]]);

            //TODO Update Images according to ing + spawnPoints & emotes
            for (int i = 0; i < rngTile.Length; i++)
            {
                SpawnPoints[i].GetComponent<Text>().text = choices[i].SpawnPoint.ToString();
                Debug.Log("!!!!!!!!!!!!!!!!!rngTile "+i+": "+rngTile[i] );
                TileSlot[i].sprite = TileTextures[rngTile[i]];
                var e = Enum.Parse(typeof(eEmote), tileCurrentEmotes[i]);
                TileEmoteSlot[i].sprite = EmoteTextures[(int)e];
            }

            //Reset VoteCounter
            for (int i = 0; i < 3; i++) tileVoteCounter[i] = 0;
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
            int ingPos = Array.IndexOf(ingCurrentEmotes, msg);
            if (ingPos > -1) ingVoteCounter[ingPos]++;

            int tilePos = Array.IndexOf(tileCurrentEmotes, msg);
            if (tilePos > -1) tileVoteCounter[tilePos]++;
        }
    }
}
