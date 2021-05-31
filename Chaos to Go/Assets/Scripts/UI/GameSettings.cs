using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static float MUSIC_VOLUME = 1.0f;
    public static float SOUND_VOLUME = 1.0f;

    [SerializeField]
    private Slider sliderMusic;
    [SerializeField]
    private Slider sliderSound;
    [SerializeField]
    private MainMenu mainMenu;


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
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    public void OnPressOkay()
    {
        MUSIC_VOLUME = sliderMusic.value;
        SOUND_VOLUME = sliderSound.value;
        Hide();
        mainMenu.Unfreeze();
    }
}
