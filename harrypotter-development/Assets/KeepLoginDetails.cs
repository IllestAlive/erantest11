using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeepLoginDetails : Instancable<KeepLoginDetails>
{
    public TMP_InputField userNameField;
    public TMP_InputField passwordField;
    public Toggle keepToggle;

    private bool KeepDetails
    {
        get => PlayerPrefs.GetInt(PlayerPrefsStrings.KeepLoginDetails, 0) != 0;
        set => PlayerPrefs.SetInt(PlayerPrefsStrings.KeepLoginDetails, value ? 1 : 0);
    }
    
    private string UserName
    {
        get => PlayerPrefs.GetString(PlayerPrefsStrings.LoginUserName, "");
        set => PlayerPrefs.SetString(PlayerPrefsStrings.LoginUserName, value);
    }
    
    private string Password
    {
        get => PlayerPrefs.GetString(PlayerPrefsStrings.LoginPassword, "");
        set => PlayerPrefs.SetString(PlayerPrefsStrings.LoginPassword, value);
    }
    
    private void Start()
    {
        KeepDetails = keepToggle.isOn || KeepDetails;
        
        if (KeepDetails)
        {
            userNameField.text = UserName;
            passwordField.text = Password;
        }

        keepToggle.isOn = KeepDetails;
    }

    public void TrySaveLoginDetails()
    {
        if (KeepDetails)
        {
            UserName = userNameField.text;
            Password = passwordField.text;
        }
    }

    public void OnKeepDetailsToggle()
    {
        KeepDetails = keepToggle.isOn;
    }
}
