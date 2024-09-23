
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    //добавить сложность уровня от которой зависит лут


    public Sprite[] levelBackgrounds; // Массив изображений фонов для каждого уровня
    public GameObject[] enemyPrefabs; // Массив префабов врагов для каждого уровня
    public Enemy[] enemies;
    public SpriteRenderer backgroundRenderer; // Спрайт рендерер для фона
    public Transform enemyTransform; // Точка спавна врага
    public PlayerInfo playerInfo;
    public Transform canvas;

    void Start()
    { 
        // Получаем текущий уровень из PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            enemyTransform.GetComponent<ActiveEnemy>().enemy = enemies[currentLevel % enemies.Length];
        enemyTransform.GetComponent<ActiveEnemy>().RefreshEnemyStats();
        // Устанавливаем фон для текущего уровня
            backgroundRenderer.sprite = levelBackgrounds[currentLevel % levelBackgrounds.Length];
        // Спавним врага для текущего уровня
        GameObject enemyGO;
            enemyGO = Instantiate(enemyPrefabs[currentLevel % enemyPrefabs.Length], enemyTransform);
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(CompleteCurrentLevel);
        enemyTransform.GetComponent <ActiveEnemy>().EnemyGameobject = enemyGO;
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(canvas.GetComponent<FightCanvasManager>().WinLevelSpawn);
        playerInfo.OnPlayerDied.AddListener(canvas.GetComponent<FightCanvasManager>().LoseLevelSpawn);
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
            GameManager.playerEconomic.GetExpFromLevel(currentLevel+1);
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("fightScene", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {

        // Переключаемся обратно на сцену выбора уровня, заменяя текущую сцену
        SceneManager.LoadScene("menu", LoadSceneMode.Single); // Замените на имя вашей сцены выбора уровня
    }
}
