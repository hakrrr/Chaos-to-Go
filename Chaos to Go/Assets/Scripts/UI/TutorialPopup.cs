using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    InputStates btnEsc = new InputStates(KeyCode.Escape);

    private bool hidden = false;


    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hidden && PauseMenu.PAUSED && btnEsc.Check() == InputStates.InputState.JustPressed)
        {
            Hide();
        }
    }


    public void Hide()
    {
        hidden = true;
        PauseMenu.PAUSED = false;
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        hidden = false;
        PauseMenu.PAUSED = true;
        transform.localScale = new Vector3(1, 1, 1);
    }
}
