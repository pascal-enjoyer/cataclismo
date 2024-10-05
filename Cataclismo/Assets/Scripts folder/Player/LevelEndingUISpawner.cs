using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelEndingUISpawner : MonoBehaviour
{
    public GameObject levelEndingUI;
    public GameLevelManager gameLevelManager;
    public GameObject levelEndindSpawned;

    public GameObject ItemUIPrefab;

    public void WinLevelSpawn()
    {
        if (levelEndindSpawned == null)
        {

            levelEndindSpawned = Instantiate(levelEndingUI, transform);
            LevelEndingUI levelEndindUI = levelEndindSpawned.GetComponent<LevelEndingUI>();
            levelEndindUI.gameLevelManager = gameLevelManager;
            levelEndindUI.ItemUIPrefab = ItemUIPrefab;

            levelEndindUI.SetupWin();
        }
    }

    public void LoseLevelSpawn()
    {
        if (levelEndindSpawned == null)
        {
            levelEndindSpawned = Instantiate(levelEndingUI, transform);
            levelEndindSpawned.GetComponent<LevelEndingUI>().gameLevelManager = gameLevelManager;
            levelEndindSpawned.GetComponent<LevelEndingUI>().SetupLoose();
        }
    }
}
