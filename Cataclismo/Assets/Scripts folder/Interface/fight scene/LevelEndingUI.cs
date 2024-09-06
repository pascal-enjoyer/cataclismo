using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndingUI : MonoBehaviour
{
    public GameLevelManager gameLevelManager;

    public Transform backgroundslot;
    public GameObject defeatScreenBackground;
    public GameObject winBackground;
    public GameObject defeatText;
    public GameObject winText;
    public Transform backToMenuButton;
    public Transform retryButton;

    public bool isFightWon;
    private void Start()
    {
        backToMenuButton.GetComponent<Button>().onClick.AddListener(BackToMenuButtonClicked);
        retryButton.GetComponent<Button>().onClick.AddListener(RetryButtonClicked);
    }
    public void BackToMenuButtonClicked()
    {
        gameLevelManager.LoadMainMenu();
    }

    public void RetryButtonClicked()
    {
        gameLevelManager.ReloadScene();
    }

    public void SetupWin()
    {

        Instantiate(winText, transform);
        Instantiate(winBackground, backgroundslot);
    }

    public void SetupLoose()
    {

        Instantiate(defeatText, transform);
        Instantiate(defeatScreenBackground, backgroundslot);
    }

}
