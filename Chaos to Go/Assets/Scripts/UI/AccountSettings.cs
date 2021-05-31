using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountSettings : MonoBehaviour
{
    public static string ACCOUNT_NAME = "Steve";
    public static string VERIFICATION_CODE = "1234567890";

    [SerializeField]
    private InputField fieldAccName;
    [SerializeField]
    private InputField fieldVerifiCode;
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
        ACCOUNT_NAME = fieldAccName.text;
        VERIFICATION_CODE = fieldVerifiCode.text;
        Hide();
        mainMenu.Unfreeze();
    }
}
