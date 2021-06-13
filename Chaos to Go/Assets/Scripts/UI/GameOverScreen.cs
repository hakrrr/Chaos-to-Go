using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private static bool gameover = false;
    private static GameOverScreen gameOverScreen;

    [SerializeField]
    private Text scoreLabel;


    public static bool IsGameOver()
    {
        return gameover;
    }


    public static void GameOver(int score)
    {
        PauseMenu.PAUSED = true;
        gameover = true;
        gameOverScreen.Show();
        gameOverScreen.scoreLabel.text = "  " + score;
    }


    public void OnPressRestart()
    {
        gameover = false;
        PauseMenu.PAUSED = false;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void OnPressQuit()
    {
        gameover = false;
        PauseMenu.PAUSED = false;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }


    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen = this;
        gameover = false;
        Hide();
    }

    // For testing
    //int c = 0;
    
    // Update is called once per frame
    void Update()
    {
        // For Testing
        /*if(c == 180)
        {
            GameOverScreen.GameOver(42);
        }
        c++;*/
    }
}
