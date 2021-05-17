using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameBoard gameBoard;
    [SerializeField]
    private GameConsoleHandler console;
    [SerializeField]
    private Text scoreText;


    private int score = 0;


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


    private void UpdateScoreText()
    {
        scoreText.text = "" + score;
    }


    // Start is called before the first frame update
    void Start()
    {
        console.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
