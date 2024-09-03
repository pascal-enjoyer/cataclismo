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
    
    void Start()
    {
        // �������� ������� ������� �� PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        enemyTransform.GetComponent<ActiveEnemy>().enemy = enemies[currentLevel];
        enemyTransform.GetComponent<ActiveEnemy>().RefreshEnemyStats();
        // ������������� ��� ��� �������� ������
        backgroundRenderer.sprite = levelBackgrounds[currentLevel];

        // ������� ����� ��� �������� ������
        Instantiate(enemyPrefabs[currentLevel], enemyTransform);
        enemyTransform.GetComponent<ActiveEnemy>().OnEnemyDied.AddListener(CompleteCurrentLevel);
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

        // ������������� ������� �� ����� ������ ������, ������� ������� �����
        SceneManager.LoadScene("menu", LoadSceneMode.Single); // �������� �� ��� ����� ����� ������ ������
    }
}
