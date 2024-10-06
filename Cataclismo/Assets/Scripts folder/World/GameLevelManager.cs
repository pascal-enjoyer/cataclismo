
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    //�������� ��������� ������ �� ������� ������� ���


    public Sprite[] levelBackgrounds; // ������ ����������� ����� ��� ������� ������
    public GameObject[] enemyPrefabs; // ������ �������� ������ ��� ������� ������
    public Enemy[] enemies;
    public SpriteRenderer backgroundRenderer; // ������ �������� ��� ����
    public Transform enemyTransform; // ����� ������ �����
    public PlayerInfo playerInfo;
    public Transform canvas;

    public int experience;
    public int money;
    public int diamonds;

    void Start()
    { 
        // �������� ������� ������� �� PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            enemyTransform.GetComponent<ActiveEnemy>().enemy = enemies[currentLevel % enemies.Length];
        enemyTransform.GetComponent<ActiveEnemy>().RefreshEnemyStats();
        // ������������� ��� ��� �������� ������
            backgroundRenderer.sprite = levelBackgrounds[currentLevel % levelBackgrounds.Length];
        // ������� ����� ��� �������� ������
        GameObject enemyGO;
            enemyGO = Instantiate(enemyPrefabs[currentLevel % enemyPrefabs.Length], enemyTransform);
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(CompleteCurrentLevel);
        enemyTransform.GetComponent <ActiveEnemy>().EnemyGameobject = enemyGO;
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(canvas.GetComponent<LevelEndingUISpawner>().WinLevelSpawn);
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
            GameManager.Instance.CompleteLevel(currentLevel);
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
