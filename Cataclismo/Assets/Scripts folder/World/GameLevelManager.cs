
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    //�������� ��������� ������ �� ������� ������� ���


    LevelData currentLevelData;

    public SpriteRenderer backgroundRenderer; // ������ �������� ��� ����
    public Transform enemyTransform; // ����� ������ �����
    public PlayerInfo playerInfo;
    public Transform canvas;

    void Start()
    {
        ActiveEnemy currentEnemy = enemyTransform.GetComponent<ActiveEnemy>();
        // �������� ������� ������� �� PlayerPrefs
        currentLevelData = GameManager.levelInfoController.GetSelectedLevelData();
        GameObject enemyGO;
        enemyGO = Instantiate(currentLevelData.enemyPrefabs[0], enemyTransform);


        currentEnemy.enemy = enemyGO.GetComponent<Enemy>().EnemyData;
        currentEnemy.RefreshEnemyStats();
        // ������������� ��� ��� �������� ������
        backgroundRenderer.sprite = currentLevelData.levelBackground;
        // ������� ����� ��� �������� ������

        currentEnemy.OnEnemyDied.AddListener(CompleteCurrentLevel);
        currentEnemy.EnemyGameobject = enemyGO;
        currentEnemy.OnEnemyDied.AddListener(canvas.GetComponent<LevelEndingUISpawner>().WinLevelSpawn);
        playerInfo.OnPlayerDied.AddListener(canvas.GetComponent<LevelEndingUISpawner>().LoseLevelSpawn);
    }



    public void CompleteCurrentLevel()
    {
        // �������� ������� ������� �� PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        // �������� ������� ��� ����������
        if (GameManager.Instance != null)
        {
            GameManager.lootManager.DropLoot();
            GameManager.Instance.CompleteLevel(currentLevel); //���������� �� ���� � currentLevelData, ��������� levelData
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

        // ������������� ������� �� ����� ������ ������, ������� ������� �����
        SceneManager.LoadScene("menu", LoadSceneMode.Single); // �������� �� ��� ����� ����� ������ ������
    }
}
