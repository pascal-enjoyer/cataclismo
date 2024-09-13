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
            if (currentExperience + exp >= experienceToNextPlayerLevel)
            {
                PlayerLevel++;
                currentExperience = (exp + currentExperience) - experienceToNextPlayerLevel;
                experienceToNextPlayerLevel = (int)(experienceToNextPlayerLevel * nextLevelExpMultiplier);
                GainDiamonds(PlayerLevel * 10);
                OnPlayerLevelChanged.Invoke();
            }
            else
            {
                currentExperience += exp;
            }
        }
        else
        {
            coins += exp / 100;
        }

        OnPlayerEconomicChanged.Invoke();
    }

    //����� �������� ����������� �� ��������� ������
    public void GetExpFromLevel(int level)
    {
        GainExperience(new System.Random().Next(50, 500) * level);
        coins += new System.Random().Next(50, 100) * level;

        OnPlayerEconomicChanged.Invoke();
    }

    public void SavePlayerEconomy()
    {
        PlayerPrefs.SetInt(PlayerLevelKey, PlayerLevel);
        PlayerPrefs.SetInt(ExperienceToNextLevelKey, experienceToNextPlayerLevel);
        PlayerPrefs.SetInt(CurrentExperienceKey, currentExperience);
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.SetInt(DiamondsKey, diamonds);

        PlayerPrefs.Save();
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

        // ����� ������� ��������� �������� �� ���������
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
