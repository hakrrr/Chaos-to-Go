using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinuteSecondTimer : MonoBehaviour
{
    [SerializeField]
    private uint minutes = 5;
    [SerializeField]
    private uint seconds = 0;
    [SerializeField]
    private bool autostart = true;
    [SerializeField]
    private Text timerText = null;
    [SerializeField]
    private bool givesNoDamnAboutPause = false;

    private bool running = false;
    private float timer = 0.0f;
    private bool expired = false;


    public void StartTimer(uint minutes, uint seconds)
    {
        this.minutes = minutes;
        this.seconds = seconds;
        running = true;
        timer = minutes * 60.0f + seconds;
        expired = false;
    }


    public uint GetMinutes()
    {
        return (uint) (timer / 60.0f);
    }


    public uint GetSeconds()
    {
        uint minutes = GetMinutes();
        return (uint)(timer - (minutes * 60.0f));
    }


    public bool IsExpired()
    {
        return expired;
    }


    private void TimeStep()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            running = false;
            timer = 0.0f;
            expired = true;
        }
    }


    private void UpdateText()
    {
        uint minutes = GetMinutes();
        uint seconds = GetSeconds();
        if(seconds < 10)
        {
            timerText.text = minutes + ":0" + seconds;
            return;
        }
        timerText.text = minutes + ":" + seconds;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (autostart)
        {
            StartTimer(minutes, seconds);
            UpdateText();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!givesNoDamnAboutPause && PauseMenu.PAUSED)
        {
            return;
        }
        if (running)
        {
            TimeStep();
            if(timerText != null)
            {
                UpdateText();
            }
        }
    }
}
