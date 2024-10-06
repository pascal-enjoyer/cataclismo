using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEconomic : MonoBehaviour
{
    public int PlayerLevel = 1;
    public int experienceToNextPlayerLevel = 1000;
    public int currentExperience = 0;
    public int coins = 1000;
    public int diamonds = 100;
    public int maxPlayerLevel = 30;
    public float nextLevelExpMultiplier = 1.1f;
    public UnityEvent OnPlayerEconomicChanged;
    public UnityEvent OnPlayerEconomicLoaded;
    public UnityEvent OnPlayerLevelChanged;

    private const string PlayerLevelKey = "PlayerLevel";
    private const string ExperienceToNextLevelKey = "ExperienceToNextLevel";
    private const string CurrentExperienceKey = "CurrentExperience";
    private const string CoinsKey = "Coins";
    private const string DiamondsKey = "Diamonds";

    

    private void Start()
    {
        OnPlayerEconomicChanged.AddListener(SavePlayerEconomy);
        

        LoadPlayerEconomy();
        Debug.Log($"Player Level {PlayerLevel}, exp {currentExperience}/{experienceToNextPlayerLevel}, {coins} coins, {diamonds} diamonds");
    }

    public void GainExperience(int exp)
    {
        if (PlayerLevel < maxPlayerLevel)
        {
            currentExperience += exp;
            while (currentExperience >= experienceToNextPlayerLevel)
            {
                PlayerLevel++;
                currentExperience -= experienceToNextPlayerLevel;
                experienceToNextPlayerLevel = (int)(experienceToNextPlayerLevel * nextLevelExpMultiplier);

                GainDiamonds(PlayerLevel * 10);

                OnPlayerLevelChanged.Invoke();

            }

        }
        else
        {
            coins += exp / 100;
        }

        OnPlayerEconomicChanged.Invoke();
    }

    //можно добавить зависимость от сложности уровня
    public int GetExpFromLevel(int level)
    {
        int exp = new System.Random().Next(50, 500) * level;
        GainExperience(exp);
        return exp;

    }

    public int GetMoneyFromLevel(int level)
    {
        int money = new System.Random().Next(100, 200) * level;
        GainMoney(money);
        return money;
    }

    public int GetDiamondsFromLevel(int level)
    {
        int diamonds = new System.Random().Next(50, 100) * level;
        GainDiamonds(diamonds);
        return diamonds;
    }

    public void SavePlayerEconomy()
    {
        PlayerPrefs.SetInt(PlayerLevelKey, PlayerLevel);
        PlayerPrefs.SetInt(ExperienceToNextLevelKey, experienceToNextPlayerLevel);
        PlayerPrefs.SetInt(CurrentExperienceKey, currentExperience);
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.SetInt(DiamondsKey, diamonds);

        PlayerPrefs.Save();
        LoadPlayerEconomy();
    }

    public void LoadPlayerEconomy()
    {
        PlayerLevel = PlayerPrefs.GetInt(PlayerLevelKey, 1);
        experienceToNextPlayerLevel = PlayerPrefs.GetInt(ExperienceToNextLevelKey, 1000);
        currentExperience = PlayerPrefs.GetInt(CurrentExperienceKey, 0);
        coins = PlayerPrefs.GetInt(CoinsKey, 1000);
        diamonds = PlayerPrefs.GetInt(DiamondsKey, 100);
        OnPlayerEconomicLoaded.Invoke();
    }

    public void ClearPlayerEconomy()
    {
        PlayerPrefs.DeleteKey(PlayerLevelKey);
        PlayerPrefs.DeleteKey(ExperienceToNextLevelKey);
        PlayerPrefs.DeleteKey(CurrentExperienceKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(DiamondsKey);

        // После очистки загрузите значения по умолчанию
        LoadPlayerEconomy();
    }

 
    public void GainMoney(int count)
    {
        coins += count;
        OnPlayerEconomicChanged.Invoke();
        LoadPlayerEconomy();
    }

    public void GainDiamonds(int diamnondCount)
    {
        diamonds += diamnondCount;
        OnPlayerEconomicChanged.Invoke();
        LoadPlayerEconomy();
    }

}
