using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMg : MonoBehaviour
{

    private AudioSource[] sounds;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        sounds = GetComponents<AudioSource>();
    }

    public void setVolumeMusic()
    {
        float value = GameObject.Find("SliderMusic").GetComponent<Slider>().value;
        sounds[sounds.Length - 1].volume = value;
    }
    public void setVolumeSound()
    {
        for (int i = 0; i < sounds.Length-1; i++)
        {
            float value = GameObject.Find("SliderSounds").GetComponent<Slider>().value;
            sounds[i].volume = value;
        }
    }

}
