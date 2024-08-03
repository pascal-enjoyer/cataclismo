using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    [SerializeField] private PlayerInfo player;
    [SerializeField] private Transform healthLine;

    private void Start()
    {
        player.OnPlayerGetDamage.AddListener(RefreshLine);   
    }
    public void RefreshLine()
    {
        Image img = healthLine.GetComponent<Image>();
        if (img != null && player != null)
        { 
            img.fillAmount = player.GetCurrentHealth() / player.GetMaxHealth();
        }
    }
}
