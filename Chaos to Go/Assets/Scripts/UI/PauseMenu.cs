using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PAUSED = false;

    private InputStates btnPause = new InputStates(KeyCode.Escape);
    private InputStates btnPause2 = new InputStates(KeyCode.Pause);

    private bool hidden = false;


    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if(PAUSED && hidden)
        {
            return;
        }

        if (GameOverScreen.IsGameOver())
        {
            Hide();
            return;
        }

        if (btnPause.Check() == InputStates.InputState.JustPressed || btnPause2.Check() == InputStates.InputState.JustPressed)
        {
            if (!PAUSED)
            {
                Show();
                PAUSED = true;
            }
            else
            {
                Hide();
                PAUSED = false;
            }
        }
    }


    public void Hide()
    {
        hidden = true;
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        hidden = false;
        transform.localScale = new Vector3(1, 1, 1);
    }


    public void OnPressContinue()
    {
        PAUSED = false;
        Hide();
    }


    public void OnPressQuit()
    {
        Hide();
        PAUSED = false;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
