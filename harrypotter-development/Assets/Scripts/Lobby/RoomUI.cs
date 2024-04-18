using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomUI : MonoBehaviour
{
    public Button button;
    [SerializeField] private TextMeshProUGUI matchText;
    public Guid matchID;

    public void Init(Match matchInfo)
    {
        matchID = matchInfo.matchID;

        button.onClick.AddListener(() => LobbyManager.Instance.OnTapRoomButton(matchID));

        if (matchInfo.IsFull)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

        matchText.text = $"{matchInfo.matchName} : {matchInfo.matchID}";
    }

    public void OnClick()
    {

    }
}
