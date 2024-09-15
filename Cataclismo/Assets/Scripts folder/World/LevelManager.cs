using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour

{
    public GameObject uncompletedLevelPrefab, completedLevelPrefab, currentLevelPrefab;
    public GameObject[] levelButtons; // Массив кнопок уровней

    void Start()
    {


        // Устанавливаем состояние кнопок уровней
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        foreach (GameObject level in levelButtons)
        {
            foreach (Transform child in level.transform) 
            {
                Destroy(child.gameObject);
            }
        }
        int levelsCompleted = GameManager.Instance.GetLevelsCompleted();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            GameObject levelIcon;
            Button tmpBtn = levelButtons[i].GetComponent<Button>();
            if (i < levelsCompleted)
            {
                levelIcon = Instantiate(completedLevelPrefab, levelButtons[i].transform);
                levelIcon.GetComponent<LevelButton>().Setup((i+1).ToString());
                tmpBtn.interactable = true;
                int levelIndex = i; // Локальная копия для использования в лямбда-функции
                tmpBtn.onClick.AddListener(() => LoadLevel(levelIndex));
                levelButtons[i].SetActive(true);
            }
            else if (i == levelsCompleted)
            {
                levelIcon = Instantiate(currentLevelPrefab, levelButtons[i].transform);
                tmpBtn.interactable = true;
                int levelIndex = i; // Локальная копия для использования в лямбда-функции
                tmpBtn.onClick.AddListener(() => LoadLevel(levelIndex));
                levelButtons[i].SetActive(true);
            }
            else
            {
                levelIcon = Instantiate(uncompletedLevelPrefab, levelButtons[i].transform);
                tmpBtn.interactable = false;
                
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
