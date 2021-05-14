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
        private float IngCD;
        private float MaxCD = 10;

        private string[] emotes = { "1", "2", "3", "4", "5" };
        private string[] currentEmotes = new string[3];
        private int[] voteCounter = new int[3] { 0, 0, 0 };

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
            for (int i = 0; i < 3; i++) Debug.Log("Ingredient " + (i+1) +" has " + voteCounter[i] + "votes");
        }

        private void ResetPoll()
        {
            //Reset CD
            IngCD = MaxCD;

            //Generate 3 Unique Random Emotes for VoteCasting
            var result = Enumerable.Range(0, emotes.Length).OrderBy(g => Guid.NewGuid()).Take(3).ToArray();
            for (int i = 0; i < 3; i++) currentEmotes[i] = emotes[result[i]];
            
            //Print out current Emotes
            Debug.Log("Current Voting Emotes are: ");
            for (int i = 0; i < 3; i++) Debug.Log(currentEmotes[i]);

            //TODO Generate random ingredients and update the images

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
