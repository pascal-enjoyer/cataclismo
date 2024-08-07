using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour

{
    public GameObject[] levelButtons; // ������ ������ �������

    void Start()
    {


        // ������������� ��������� ������ �������
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
                int levelIndex = i; // ��������� ����� ��� ������������� � ������-�������
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
        // ��������� ������� ������� � PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();

        // ��������� ����� ������, ������� ������� �����
        SceneManager.LoadScene("fightScene", LoadSceneMode.Single); // ��� ����� ����� ������
    }

}
