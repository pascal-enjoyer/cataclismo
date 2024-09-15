using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropFromChestWindow : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public Text itemDescription;
    public Text itemRarity;
    public Image itemBackground;

    public Button exitButton;

    public void Setup(InventoryItem item)
    {
        itemIcon.sprite = item.ItemIcon;    
        itemName.text = item.item.itemName;
        itemDescription.text = item.item.itemDescription;
        itemRarity.text = item.itemRarity.ToString();
        itemBackground.sprite = item.rarityBackgroundSprite;
        exitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        Destroy(gameObject);
    }
}
