
using System.Collections.Generic;
using UnityEngine;

//хранится инфа о всех уровнях и после выбора уровня передается в GameLevelManager
public class LevelInfoController : MonoBehaviour
{
    public List<LevelData> levels = new List<LevelData>();

    private Dictionary<int, LevelData> levelsByNumbers;

    public LevelData defaultLevel;

    private int selectedLevel;

    public int SelectedLevel => selectedLevel;


    public bool isDebugging;

    private void Start()
    {
        levelsByNumbers = new Dictionary<int, LevelData>();
        foreach (LevelData level in levels)
        {
            levelsByNumbers.Add(level.levelIndex, level);
        }
    }

    public void SelectLevel(int selectedLevel)
    {
        if (levelsByNumbers.ContainsKey(selectedLevel))
        {
            this.selectedLevel = selectedLevel;
            if (isDebugging)
                Debug.Log(selectedLevel);
        }
    }

    public LevelData GetSelectedLevelData()
    {   
        if (isDebugging)
            Debug.Log(levelsByNumbers[selectedLevel]);
        if (!levelsByNumbers.ContainsKey(selectedLevel))
        {
            Debug.LogError($"There is no {selectedLevel} level in levels list on {gameObject.name}, loading default level.");
            return defaultLevel;
            
        }
        return levelsByNumbers[selectedLevel];
    }

}
