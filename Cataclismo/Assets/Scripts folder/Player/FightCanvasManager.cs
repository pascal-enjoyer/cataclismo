using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FightCanvasManager : MonoBehaviour
{
    public GameObject levelEndingUI;
    public GameLevelManager gameLevelManager;
    public GameObject levelEndindSpawned;

    public void WinLevelSpawn()
    {
        if (levelEndindSpawned == null)
        {
            levelEndindSpawned = Instantiate(levelEndingUI, transform);
            levelEndindSpawned.GetComponent<LevelEndingUI>().gameLevelManager = gameLevelManager;
            levelEndindSpawned.GetComponent<LevelEndingUI>().SetupWin();
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
