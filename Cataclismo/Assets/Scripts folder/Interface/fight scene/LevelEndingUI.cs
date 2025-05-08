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
    public Transform droppedEconomicPanel;

    [Header("Префабы")]
    public GameObject ItemUIPrefab;

    public GameObject moneyDropPrefab;
    public GameObject expDropPrefab;
    public GameObject diamondDropPrefab;



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

        droppedLoot = GameManager.Instance.lootManager.lastDroppedLoot;
        LevelData levelData = GameManager.Instance.levelInfoController.GetSelectedLevelData();
        int droppedExperienceGLM = levelData.exp;
        int droppedMoneyGLM = levelData.money;
        int droppedDiamondsGLM = levelData.diamonds;
        if (droppedMoneyGLM > 0) 
        {
            Instantiate(moneyDropPrefab, droppedEconomicPanel).GetComponent<afterFightEcomonicDrop>().placeForText.text = droppedMoneyGLM.ToString();
        }

        if (droppedExperienceGLM > 0)
        {
            Instantiate(expDropPrefab, droppedEconomicPanel).GetComponent<afterFightEcomonicDrop>().placeForText.text = droppedExperienceGLM.ToString();
        }


        if (droppedDiamondsGLM > 0)
        {
            Instantiate(diamondDropPrefab, droppedEconomicPanel).GetComponent<afterFightEcomonicDrop>().placeForText.text = droppedDiamondsGLM.ToString();
        }


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
