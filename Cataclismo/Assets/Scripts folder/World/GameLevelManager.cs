using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    public Sprite[] levelBackgrounds; // Массив изображений фонов для каждого уровня
    public GameObject[] enemyPrefabs; // Массив префабов врагов для каждого уровня
    public SpriteRenderer backgroundRenderer; // Спрайт рендерер для фона
    public Transform enemyTransform; // Точка спавна врага

    void Start()
    {
        // Получаем текущий уровень из PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        
        // Устанавливаем фон для текущего уровня
        backgroundRenderer.sprite = levelBackgrounds[currentLevel];

        // Спавним врага для текущего уровня
        Instantiate(enemyPrefabs[currentLevel], enemyTransform);
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(CompleteCurrentLevel);
    }

    public void CompleteCurrentLevel()
    {
        // Получаем текущий уровень из PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        // Отмечаем уровень как пройденный
        if (GameManager.Instance != null)
        {
            GameManager.lootManager.DropLoot();
            GameManager.Instance.CompleteLevel(currentLevel);
        }

        // Переключаемся обратно на сцену выбора уровня, заменяя текущую сцену
        SceneManager.LoadScene("menu", LoadSceneMode.Single); // Замените на имя вашей сцены выбора уровня
    }
}
