using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool PAUSED = false;

    private InputStates btnPause = new InputStates(KeyCode.Escape);
    private InputStates btnPause2 = new InputStates(KeyCode.Pause);


    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (btnPause.Check() == InputStates.InputState.JustPressed ||
            btnPause2.Check() == InputStates.InputState.JustPressed)
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
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
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
        Application.Quit();
    }
}
