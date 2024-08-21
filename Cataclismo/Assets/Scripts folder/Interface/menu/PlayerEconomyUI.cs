using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomyUI : MonoBehaviour
{
    public Image levelBar;
    public Text coinsCount;
    public Text diamondCount;
    public Text levelCount;
    public PlayerEconomic playerEconomic;

    public void Start()
    {
        playerEconomic = GameManager.playerEconomic;
        playerEconomic.OnPlayerEconomicLoaded.AddListener(RefreshUI);
        RefreshUI();
    }

    public void RefreshUI()
    {
        levelBar.fillAmount = (float)((float)playerEconomic.currentExperience/(float)playerEconomic.experienceToNextPlayerLevel);
        coinsCount.text = playerEconomic.coins.ToString();
        diamondCount.text = playerEconomic.diamonds.ToString();
        levelCount.text = playerEconomic.PlayerLevel.ToString();
    }
}
