using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TutorialManager : MonoBehaviour
{
     private bool hidden = false;
    public Text hintText;

    [TextArea(3, 10)]
    public string[] sentences;
    string sentence;
    int currentHint;
    public GameObject[] images; //??? game object?? Animated UI element?

    // Start is called before the first frame update
    void Start()
    {
        StartTutorial();
    }

    void StartTutorial()
    {

        hidden = false;
        PauseMenu.PAUSED = true;
        transform.localScale = new Vector3(1, 1, 1);

        //Debug.Log("Starting Tutorial");
        currentHint = 0;
        sentence = sentences[currentHint];
        hintText.text = sentence;
    }

    public void DisplayNextHint()
    {
        if(currentHint+1 == sentences.Length)
        {
            CloseTutorial();
            return;
        }
        currentHint++;
        sentence = sentences[currentHint];
        hintText.text = sentence;
        //Debug.Log(sentence);
    }

    public void DisplayPreviousHint()
    {
        if(currentHint == 0)
        {
            BackToMenu();
            return;
        }
        currentHint--;
        sentence = sentences[currentHint];
        hintText.text = sentence;
        //Debug.Log(sentence);
    }

    void CloseTutorial()
    {
        hidden = true;
        PauseMenu.PAUSED = false;
        transform.localScale = Vector3.zero;
    }

    void BackToMenu()
    {
        //Hide();
        //PAUSED = false;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
