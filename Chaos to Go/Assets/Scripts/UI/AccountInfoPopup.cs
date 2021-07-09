using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccountInfoPopup : MonoBehaviour
{
    [SerializeField]
    private MainMenu mainMenu;

    private bool hidden = false;


    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {

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


    public void OnPressOkay()
    {
        Hide();
        mainMenu.Unfreeze();
        PlayAudio("PickUp");
    }


    private void PlayAudio(string clipname)
    {
        AudioSource[] audioSources = GameObject.Find("Audio").GetComponents<AudioSource>();
        foreach (AudioSource src in audioSources)
        {
            if (src.clip.name.Equals(clipname))
            {
                src.Play();
                return;
            }
        }
    }
}