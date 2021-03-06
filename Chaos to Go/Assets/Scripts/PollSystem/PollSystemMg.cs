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
        private RectTransform TwitchTutorial;
        [SerializeField]
        private Text TileCDText;
        [SerializeField]
        private Image[] SpawnPoints;
        [SerializeField]
        private Image[] IngredientSlot;
        [SerializeField]
        private Image[] TileSlot;
        [SerializeField]
        private Image[] IngEmoteSlot;
        [SerializeField]
        private Image[] TileEmoteSlot;
        [SerializeField]
        private Sprite[] SpawnPointTextures;
        [SerializeField]
        private Sprite[] IngredientTextures;
        [SerializeField]
        private Sprite[] TileTextures;
        [SerializeField]
        private Sprite[] EmoteTextures;
        [SerializeField]
        private GameObject[] IngredientVotingBars;
        [SerializeField]
        private GameObject[] TileVotingBars;

        public TileSelectionMenu tileSelectionMenu;

        private enum eEmote
        {
            LUL,
            monkaS,
            KEKW,
            Kappa,
            TriHard
        }

        //This variable effects the ingredient spawning. In 1/x cases a recipe-requiered ingredient will  spawn
        public int RecipeSpawnBias = 1000;

        private float IngCD;
        public float IngMaxCD;
        private int MaxSpawns = 4;

        private float TileCD;
        public float TileMaxCD;


        // private string[] ingredients = { "tomato", "chicken", "onion", "carrot", "asparagus" };
        private string[] ingredients = { "asparagus", "carrot", "chicken", "onion", "tomato" };
        // eDir   left, top, right, down
        private int[][] tiles_dir = {
            new int[] { 1, 3 },
            new int[] { 3, 1 },
            new int[] { 0, 2 },
            new int[] { 2, 0 },
            new int[] { 1, 0 },
            new int[] { 1, 2 },
            new int[] { 3, 0 },
            new int[] { 3, 2 },
            new int[] { 0, 1 },
            new int[] { 0, 3 },
            new int[] { 2, 1 },
            new int[] { 2, 3 }, };
        //private int[][] tiles_dir = new int[12][2] { }

        private string[] tiles = {
        /*0*/ "topdown",
        /*1*/  "downtop",
        /*2*/"leftright",
        /*3*/"rightleft",
        /*4*/"topleft",
        /*5*/"topright",
        /*6*/"downleft",
        /*7*/"downright",
        /*8*/"lefttop",
        /*9*/"leftdown",
        /*10*/"righttop",
        /*11*/"rightdown" };
        private string[] emotes = { "LUL", "monkaS", "KEKW", "Kappa", "TriHard" };
        private string[] ingCurrentEmotes = new string[3];
        private string[] tileCurrentEmotes = new string[3];
        private int[] ingVoteCounter = new int[3] { 0, 0, 0 };
        private int[] tileVoteCounter = new int[3] { 0, 0, 0 };
        private ingSpawnInfo[] choices = new ingSpawnInfo[3];
        private int[] rngTile = new int[3];

        
        private string[] twitchTutorials = { "IMPACT THE GAME AS VIEWER!", "VOTE BY WRITING THE EMOTES IN THE CHAT!", "ANY MESSAGE THAT CONTAINS THE EMOTE COUNTS!", "CHANGE UP YOUR MESSAGES TO BYPASS THE 30 SECOND SPAM PROTECTION!", "<spaceholder for emotes in use>" };


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
            twitchTutorials[twitchTutorials.Length-1] = "EMOTES IN USE: " + string.Join(" ", emotes);
            IngResetPoll();
            TileResetPoll();
            UpdatePollBars();
            StartCoroutine(CycleTwitchTutorial(0));
        }

        void Update()
        {


            if (IngCD > 0)
            {
                //CD
                if (!PauseMenu.PAUSED) IngCD -= Time.deltaTime;
                IngCDText.GetComponent<Text>().text = ((int)IngCD).ToString();
            }
            else
            {
                IngEvalVote();
                IngResetPoll();
            }

            if (TileCD > 0)
            {
                //CD
                if (!PauseMenu.PAUSED) TileCD -= Time.deltaTime;
                TileCDText.GetComponent<Text>().text = ((int)TileCD).ToString();
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

            GetComponent<PlaySounds>().playSpawn();
            //     Debug.Log("Result: " + result.IngredientName + " " + result.SpawnPoint);
            //    Debug.Log("-------------------------");
        }

        void TileEvalVote()
        {
            //Print out the votes
            //    for (int i = 0; i < 3; i++) Debug.Log("Tile " + (i + 1) + " has " + tileVoteCounter[i] + " votes");
            //max voteCounter => spawnInfo
            int maxValue = tileVoteCounter.Max();
            int maxIndex = tileVoteCounter.ToList().IndexOf(maxValue);


            tileSelectionMenu.AddBaseTile((BaseTile.eDirection)tiles_dir[rngTile[maxIndex]][0], (BaseTile.eDirection)tiles_dir[rngTile[maxIndex]][1]);
            GetComponent<PlaySounds>().playBlink();
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

            //this creates a bias towards ingredients that are used in recipes
            for (int i = 0; i < rngIng.Length; i++)
            {
                if (Random.Range(0, RecipeSpawnBias) == 0)
                {
                    int rndRec = Random.Range(0, Game.GAME.GetFoodOrders().Length);
                    Recipes.Recipe[] recipes = Game.GAME.GetFoodOrders();
                    int rndIng = Random.Range(0, 3);
                    int ingToSpawn = -1;
                    if (rndIng == 0)
                    {
                        ingToSpawn = ((int)recipes[rndRec].ingredient1) - 1;

                    }
                    if (rndIng == 1)
                    {
                        ingToSpawn = ((int)recipes[rndRec].ingredient2) - 1;

                    }
                    if (rndIng == 2)
                    {
                        ingToSpawn = ((int)recipes[rndRec].ingredient3) - 1;

                    }
                    if (ingToSpawn != -1)
                    {
                        rngIng[rndIng] = ingToSpawn;
                    }
                }
            }
            for (int i = 0; i < 3; i++) choices[i] = new ingSpawnInfo(ingredients[rngIng[i]], rngSpawn[i]);

            Debug.Log("Current Choices: ");
            for (int i = 0; i < 3; i++) Debug.Log(choices[i].IngredientName + " " + choices[i].SpawnPoint);

            //TODO Update Images according to ing + spawnPoints & emotes
            for (int i = 0; i < choices.Length; i++)
            {
                SpawnPoints[i].sprite = SpawnPointTextures[choices[i].SpawnPoint - 1];
                var e = Enum.Parse(typeof(Recipes.eIngredients), choices[i].IngredientName);
                IngredientSlot[i].sprite = IngredientTextures[(int)e - 1];
                e = Enum.Parse(typeof(eEmote), ingCurrentEmotes[i]);
                IngEmoteSlot[i].sprite = EmoteTextures[(int)e];
            }

            //Reset VoteCounter
            for (int i = 0; i < 3; i++) ingVoteCounter[i] = 0;
            UpdatePollBars();
        }

        private void TileResetPoll()
        {
            //Reset CD
            TileCD = TileMaxCD;

            //Generate 3 Random Emotes for VoteCasting
            var result = Enumerable.Range(0, emotes.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) tileCurrentEmotes[i] = emotes[result[i]];

            //Print out current Emotes
            //Debug.Log("Current tileVoting Emotes are: ");
            //for (int i = 0; i < 3; i++) Debug.Log(tileCurrentEmotes[i]);

            //Generate 3 random tiles
            //rngTile = Enumerable.Range(0, tiles.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            rngTile = new int[] { -1, -1, -1 };
            for (int i = 0; i < 3; i++)
            {
                //this while clause rerolls if a currently disabled (upwards facing) tile is chosen
                while (rngTile[i] == -1 || rngTile[i] == 1 || rngTile[i] == 8 || rngTile[i] == 10 || rngTile[i] == 1 || rngTile[i] == 6 || rngTile[i] == 7)
                {
                    rngTile[i] = Random.Range(0, tiles.Length);
                }

            }

            //Debug.Log("Current Choices: ");
            //for (int i = 0; i < 3; i++) Debug.Log(tiles[rngTile[i]]);

            //TODO Update Images according to ing + spawnPoints & emotes
            for (int i = 0; i < rngTile.Length; i++)
            {
                //Debug.Log("rngTile "+i+": "+rngTile[i] );
                TileSlot[i].sprite = TileTextures[rngTile[i]];
                var e = Enum.Parse(typeof(eEmote), tileCurrentEmotes[i]);
                TileEmoteSlot[i].sprite = EmoteTextures[(int)e];
            }

            //Reset VoteCounter
            for (int i = 0; i < 3; i++) tileVoteCounter[i] = 0;
            UpdatePollBars();
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


            UpdatePollBars();
        }

        private void UpdatePollBars()
        {

            int ingVoteSum = 0;
            for (int i = 0; i < ingVoteCounter.Length; i++)
            {
                ingVoteSum += ingVoteCounter[i];
            }


            for (int i = 0; i < IngredientVotingBars.Length; i++)
            {

                float scale = 0;
                if (ingVoteSum != 0)
                {
                    scale = ((float)ingVoteCounter[i]) / ((float)ingVoteSum);
                }
                StartCoroutine(ScaleBarOverTime(IngredientVotingBars[i], scale));
                // SetBar(IngredientVotingBars[i], scale);

            }

            int tileVoteSum = 0;
            for (int i = 0; i < tileVoteCounter.Length; i++)
            {
                tileVoteSum += tileVoteCounter[i];
            }


            for (int i = 0; i < TileVotingBars.Length; i++)
            {
                float scale = 0;
                if (tileVoteSum != 0)
                {
                    scale = ((float)tileVoteCounter[i]) / ((float)tileVoteSum);
                }
                StartCoroutine(ScaleBarOverTime(TileVotingBars[i], scale));
                //SetBar(TileVotingBars[i], scale);

            }
        }
        // can be used instead of scalebarovertime for instant poll updates.
        private void SetBar(GameObject bar, float scale)
        {
            float scale_normalized = scale;
            scale = 0.5f + 1.5f * scale;
            float time = 0.3f;
            Vector3 originalScale = bar.transform.localScale;
            bar.transform.localScale = new Vector3(originalScale.x, scale, originalScale.z);

            Vector3 originalPos = bar.GetComponent<RectTransform>().anchoredPosition;
            bar.GetComponent<RectTransform>().anchoredPosition = new Vector3(86.5f - (scale_normalized * 33.5f), originalPos.y, originalPos.z);

        }

        IEnumerator ScaleBarOverTime(GameObject bar, float scale)
        {
            float scale_normalized = scale;
            scale = 0.5f + 1.5f * scale;
            //Debug.Log("scaling in progress: "+scale);
            //pos y def. 112
            float time = 0.3f;
            Vector3 originalScale = bar.transform.localScale;
            Vector3 destinationScale = new Vector3(originalScale.x, scale, originalScale.z);

            Vector3 originalPos = bar.GetComponent<RectTransform>().anchoredPosition;
            Vector3 destinationPos = new Vector3(86.5f - (scale_normalized * 33.5f), originalPos.y, originalPos.z);




            float currentTime = 0.0f;

            do
            {
                bar.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                bar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);
        }

        IEnumerator CycleTwitchTutorial(int i)
        {
            //Debug.Log("StartedCoroutine with i: " + i);
            TwitchTutorial.GetComponent<TMPro.TextMeshProUGUI>().text = twitchTutorials[(i % twitchTutorials.Length)];
            yield return new WaitForSeconds(5);
            //Debug.Log("Waiting done  i mod is : " + (i % twitchTutorials.Length));
            StartCoroutine(CycleTwitchTutorial(++i));
        }
    }
}

