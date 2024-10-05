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
    public List<InventoryItem> droppedLoot;
    public Transform droppedLootContentPanel;
    public GameObject ItemUIPrefab;


    public Text droppedMoney;
    public Text droppedExperience;
    public Transform placeForEndingText;

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

        droppedLoot = GameManager.lootManager.lastDroppedLoot;
        
        Instantiate(winText, placeForEndingText);
        Instantiate(winBackground, backgroundslot);

        foreach (InventoryItem item in droppedLoot)
        {
           GameObject newItem = Instantiate(ItemUIPrefab, droppedLootContentPanel);
           newItem.GetComponent<ItemUI>().Setup(item);
        }
    }

    public void SetupLoose()
    {

        Instantiate(defeatText, placeForEndingText);
        Instantiate(defeatScreenBackground, backgroundslot);
    }

}
