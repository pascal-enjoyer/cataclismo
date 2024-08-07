using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour

{
    public GameObject[] levelButtons; // Массив кнопок уровней

    void Start()
    {


        // Устанавливаем состояние кнопок уровней
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        int levelsCompleted = GameManager.Instance.GetLevelsCompleted();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button tmpBtn = levelButtons[i].GetComponent<Button>();
            if (i <= levelsCompleted)
            {
                
                tmpBtn.interactable = true;
                int levelIndex = i; // Локальная копия для использования в лямбда-функции
                tmpBtn.onClick.AddListener(() => LoadLevel(levelIndex));
                levelButtons[i].SetActive(true);
            }
            else
            {
                tmpBtn.interactable = false;
                levelButtons[i].SetActive(false);
                
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        // Сохраняем текущий уровень в PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();

        // Загружаем сцену уровня, заменяя текущую сцену
        SceneManager.LoadScene("fightScene", LoadSceneMode.Single); // Имя вашей сцены уровня
    }

}
