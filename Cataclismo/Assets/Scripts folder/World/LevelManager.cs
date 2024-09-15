using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour

{
    public GameObject uncompletedLevelPrefab, completedLevelPrefab, currentLevelPrefab;
    public GameObject[] levelButtons; // ������ ������ �������

    void Start()
    {


        // ������������� ��������� ������ �������
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
                int levelIndex = i; // ��������� ����� ��� ������������� � ������-�������
                tmpBtn.onClick.AddListener(() => LoadLevel(levelIndex));
                levelButtons[i].SetActive(true);
            }
            else if (i == levelsCompleted)
            {
                levelIcon = Instantiate(currentLevelPrefab, levelButtons[i].transform);
                tmpBtn.interactable = true;
                int levelIndex = i; // ��������� ����� ��� ������������� � ������-�������
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
        // ��������� ������� ������� � PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();

        // ��������� ����� ������, ������� ������� �����
        SceneManager.LoadScene("fightScene", LoadSceneMode.Single); // ��� ����� ����� ������
    }

}
