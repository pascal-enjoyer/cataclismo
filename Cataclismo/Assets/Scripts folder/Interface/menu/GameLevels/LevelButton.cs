using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Text levelNumber;
    public void Setup(string levelNumber)
    {
        this.levelNumber.text = levelNumber;

    }
}
