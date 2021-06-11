using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private AccountSettings accountSettings;
    [SerializeField]
    private GameSettings gameSettings;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPressQuit()
    {
        Application.Quit();
    }


    public void OnPressStart()
    {
        Hide();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void OnPressAccount()
    {
        Freeze();
        accountSettings.Show();
    }


    public void OnPressSettings()
    {
        Freeze();
        gameSettings.Show();
    }


    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }


    public void Show()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    public void Freeze()
    {
        foreach(Button btn in buttons)
        {
            btn.enabled = false;
        }
    }


    public void Unfreeze()
    {
        foreach (Button btn in buttons)
        {
            btn.enabled = true;
        }
    }
}
