
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData", order = 51)]
public class LevelData : ScriptableObject
{


    public int levelIndex; // при выборе на карте игры передается имменно индекс (0, 1, 2 ... n)
    public List<GameObject> enemyPrefabs;
    public Sprite levelBackground;

    public int exp;
    public int money;
    public int diamonds;

    public bool isBossLevel;
}
