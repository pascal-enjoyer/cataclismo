using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPlayerData : MonoBehaviour
{
    public Button clearButton;
    public LevelManager levelManager;

    public void Start()
    {
        clearButton.onClick.AddListener(ClearData);
    }

    public void ClearData()
    {
        GameManager.Instance.ClearData();
        levelManager.UpdateLevelButtons();
    }

}
