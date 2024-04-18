using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Configuration : Instancable<Configuration>
{
	public bool testWithoutPlayfab;

	public BuildType buildType;
	public string buildId = "";
	public string ipAddress = "";
	public ushort port => Convert.ToUInt16(portInput.text);
	public bool playFabDebugging = false;
	public string userName => usernameInput.text;

	public bool IsServer => (int)buildType >= (int)BuildType.LOCAL_SERVER;

	public Button playButton;
	public TMP_InputField usernameInput;
	public TMP_InputField portInput;
	
	public void RegisterPlayButton()
    {
		playButton.onClick.AddListener(() => ClientManagerBehaviour.Instance.OnTapPlay());
    }
}

public enum BuildType
{
	//Clients
	PLAYFAB_LOCAL_CLIENT,
	LOCAL_CLIENT,
	REMOTE_CLIENT,
	//Servers
	LOCAL_SERVER,
	REMOTE_SERVER
}