
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    //добавить сложность уровня от которой зависит лут


    LevelData currentLevelData;

    public SpriteRenderer backgroundRenderer; // Спрайт рендерер для фона
    public Transform enemyTransform; // Точка спавна врага
    public PlayerInfo playerInfo;
    public Transform canvas;

    void Start()
    {
        ActiveEnemy currentEnemy = enemyTransform.GetComponent<ActiveEnemy>();
        // Получаем текущий уровень из PlayerPrefs
        currentLevelData = GameManager.levelInfoController.GetSelectedLevelData();
        GameObject enemyGO;
        enemyGO = Instantiate(currentLevelData.enemyPrefabs[0], enemyTransform);


        currentEnemy.enemy = enemyGO.GetComponent<Enemy>().EnemyData;
        currentEnemy.RefreshEnemyStats();
        // Устанавливаем фон для текущего уровня
        backgroundRenderer.sprite = currentLevelData.levelBackground;
        // Спавним врага для текущего уровня

        currentEnemy.OnEnemyDied.AddListener(CompleteCurrentLevel);
        currentEnemy.EnemyGameobject = enemyGO;
        currentEnemy.OnEnemyDied.AddListener(canvas.GetComponent<LevelEndingUISpawner>().WinLevelSpawn);
        playerInfo.OnPlayerDied.AddListener(canvas.GetComponent<LevelEndingUISpawner>().LoseLevelSpawn);
    }



    public void CompleteCurrentLevel()
    {
        // Получаем текущий уровень из PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        // Отмечаем уровень как пройденный
        if (GameManager.Instance != null)
        {
            GameManager.lootManager.DropLoot();
            GameManager.Instance.CompleteLevel(currentLevel); //переделать на дроп с currentLevelData, заполнить levelData
            experience = GameManager.playerEconomic.GetExpFromLevel(currentLevel+1);
            money = GameManager.playerEconomic.GetMoneyFromLevel(currentLevel + 1);
            if ((currentLevel + 1) % 5 == 0)
                diamonds = GameManager.playerEconomic.GetDiamondsFromLevel(currentLevel + 1);
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
