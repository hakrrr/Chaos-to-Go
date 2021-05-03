using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameConsoleHandler : MonoBehaviour
{
    [SerializeField]
    private Text outputText;
    [SerializeField]
    private InputField inputText;

    private InputStates btnConsole = new InputStates(KeyCode.Tab);
    private InputStates btnEnter = new InputStates(KeyCode.Return);
    private InputStates btnUp = new InputStates(KeyCode.UpArrow);
    private InputStates btnDown = new InputStates(KeyCode.DownArrow);
    private bool visible = true;

    private List<string> storedCommands = new List<string>();
    private int instrIdx = 0;


    private void HandleInputs()
    {
        if (btnConsole.Check() == InputStates.InputState.JustPressed)
        {
            if (visible)
                Hide();
            else
                Show();
        }

        if(btnEnter.Check() == InputStates.InputState.JustPressed)
        {
            string command = inputText.text;
            string[] tokens = command.Split(' ', '\t', '\n');
            string output = "";
            if(command.Length > 0)
            {
                ConsoleCommands.Execute(tokens.Length, tokens, ref output, this);
                if(!storedCommands.Contains(command))
                    storedCommands.Add(command);
                if (storedCommands.Count > 16)
                    storedCommands.RemoveAt(0);
            }
            outputText.text += ">>> " + output + "\n";
        }

        if (btnDown.Check() == InputStates.InputState.JustPressed)
        {
            if(storedCommands.Count > 0)
            {
                instrIdx -= 1;
                if (instrIdx < 0)
                    instrIdx = storedCommands.Count - 1;
                string command = storedCommands[instrIdx];
                inputText.text = command;
            }
        }

        if (btnUp.Check() == InputStates.InputState.JustPressed)
        {
            if (storedCommands.Count > 0)
            {
                instrIdx = (instrIdx + 1) % storedCommands.Count;
                string command = storedCommands[instrIdx];
                inputText.text = command;
            }
        }
    }


    public void Show()
    {
        if (visible)
            return;
        visible = true;
        transform.position -= new Vector3(0.0f, 8192f, 0.0f); // Oldest Rick Trick in the book!
        inputText.enabled = true;
    }


    public void Hide()
    {
        if (!visible)
            return;
        visible = false;
        transform.position += new Vector3(0.0f, 8192f, 0.0f);
        inputText.enabled = false;
    }


    public void Clear()
    {
        outputText.text = "";
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }
}
