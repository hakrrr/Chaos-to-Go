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
        }
        catch (Exception) { }
    }

    public void playBtn()
    {
        sounds[0].Play();
    }

    public void playAbort()
    {
        sounds[1].Play();
    }
}
