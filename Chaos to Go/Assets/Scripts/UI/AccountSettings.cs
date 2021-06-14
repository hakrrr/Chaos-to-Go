using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountSettings : MonoBehaviour
{
    public static string USER_NAME = "";
    public static string CHANNEL_NAME = "";
    public static string VERIFICATION_CODE = "";

    [SerializeField]
    private InputField fieldUserName;
    [SerializeField]
    private InputField fieldChannelName;
    [SerializeField]
    private InputField fieldVerifiCode;
    [SerializeField]
    private MainMenu mainMenu;


    // Start is called before the first frame update
    void Start()
    {
        fieldUserName.text = USER_NAME;
        fieldChannelName.text = CHANNEL_NAME;
        fieldVerifiCode.text = VERIFICATION_CODE;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    public void OnPressOkay()
    {
        PlayAudio("PickUp");
        USER_NAME = fieldUserName.text;
        CHANNEL_NAME = fieldChannelName.text;
        VERIFICATION_CODE = fieldVerifiCode.text;

        Hide();
        mainMenu.Unfreeze();
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
