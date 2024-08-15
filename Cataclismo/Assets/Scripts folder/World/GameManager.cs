
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Inventory inventory;

    public static LootManager lootManager;

    private int levelsCompleted;

    void Awake()
    {
        if (Instance == null)
        {   
            inventory = transform.GetComponent<Inventory>();
            lootManager = transform.GetComponent<LootManager>();
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
        // Получаем данные о пройденных уровнях
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= levelsCompleted)
        {
            levelsCompleted = levelIndex + 1; // Переходим к следующему уровню
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
