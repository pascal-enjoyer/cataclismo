using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int levelsCompleted;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeGame()
    {
        // �������� ������ � ���������� �������
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= levelsCompleted)
        {
            levelsCompleted = levelIndex + 1; // ��������� � ���������� ������
            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
            PlayerPrefs.Save();
        }
    }

    public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        levelsCompleted = 0;

        PlayerPrefs.Save();
    }
}
