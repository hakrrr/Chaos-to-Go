using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStates
{
    private KeyCode keyCode;
    private bool CURRENT_STATE = false;
    private bool PREV_STATE = false;


    public enum InputState
    {
        Released,
        JustPressed,
        Pressed,
        JustReleased
    }


    public InputStates(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }


    public InputState Check()
    {
        PREV_STATE = CURRENT_STATE;
        CURRENT_STATE = Input.GetKeyDown(keyCode);

        if (!PREV_STATE && !CURRENT_STATE)
            return InputState.Released;
        if (!PREV_STATE && CURRENT_STATE)
            return InputState.JustPressed;
        if (PREV_STATE && CURRENT_STATE)
            return InputState.Pressed;
        if (PREV_STATE && !CURRENT_STATE)
            return InputState.JustReleased;
        return InputState.Released;
    }
}
