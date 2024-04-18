using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

public class GameOverUIManager : Instancable<GameOverUIManager>
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;
    public GameObject youWin, youLost;
    public bool gameOver;

    public void ShowGameOverScreen(bool isWin)
    {
        gameOverScreen.SetActive(true);
        // gameOverText.text = isWin ? "You Win" : "You lost";
        if (isWin)
        {
            if (youLost.activeSelf)
                return;
            youWin.SetActive(true);
        }
        else
        {
            if (youWin.activeSelf)
                return;
            youLost.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowGameOverScreen(true);
        }
    }

    public void OnTapGoToCharSelection()
    {
        Destroy(NetworkManager.singleton.gameObject);
        Destroy(RoomManager.Instance.gameObject);
        Destroy(GameObject.Find("PLAYFAB"));

        NetworkClient.Disconnect();
        SceneManager.LoadScene("Offline");
    }

    public void OnTapGoToCharSelectionFromPractice()
    {
        SceneManager.LoadScene("Offline");
    }
}
