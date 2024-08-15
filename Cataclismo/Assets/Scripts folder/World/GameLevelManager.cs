using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    public Sprite[] levelBackgrounds; // ������ ����������� ����� ��� ������� ������
    public GameObject[] enemyPrefabs; // ������ �������� ������ ��� ������� ������
    public SpriteRenderer backgroundRenderer; // ������ �������� ��� ����
    public Transform enemyTransform; // ����� ������ �����

    void Start()
    {
        // �������� ������� ������� �� PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        
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
        }

        // ������������� ������� �� ����� ������ ������, ������� ������� �����
        SceneManager.LoadScene("menu", LoadSceneMode.Single); // �������� �� ��� ����� ����� ������ ������
    }
}
