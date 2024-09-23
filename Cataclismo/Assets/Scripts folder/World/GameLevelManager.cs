
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
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(canvas.GetComponent<FightCanvasManager>().WinLevelSpawn);
        playerInfo.OnPlayerDied.AddListener(canvas.GetComponent<FightCanvasManager>().LoseLevelSpawn);
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
            GameManager.playerEconomic.GetExpFromLevel(currentLevel+1);
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
