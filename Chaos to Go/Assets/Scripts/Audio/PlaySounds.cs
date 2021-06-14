using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    private AudioSource[] sounds;
    private void Awake()
    {
        try
        {
            this.sounds = GameObject.Find("Audio").GetComponents<AudioSource>();
            sounds[5].volume *= 0.5f;
        }
        catch (Exception) { }
    }

    public void playBtn()
    {
        if (sounds != null)
           sounds[0].Play();
    }

    public void playAbort()
    {
        if (sounds != null)
           sounds[1].Play();
    }

    public void playReplace()
    {
        if (sounds != null)
            sounds[2].Play();
    }

    public void playBlink()
    {
        if (sounds != null)
            sounds[3].Play();
    }
    public void playPuff()
    {
        if (sounds != null)
            sounds[4].Play();
    }
    public void playSpawn()
    {
        if (sounds != null)
            sounds[5].Play();
    }
    public void playComplete()
    {
        if (sounds != null)
            sounds[6].Play();
    }
    public void playSplash()
    {
        if (sounds != null)
            sounds[7].Play();
    }
}
