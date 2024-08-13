using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    public InventoryItem item;
    public ItemUI itemUI;
    public Text itemName;
    public Text itemDescription;
    public Image itemIcon;
    public Text equipButtonText;
    

    public void FillWindow(ItemUI tempItem)
    {
        itemUI = tempItem;
        item = itemUI.item;
        itemName.text = item.name;
        itemDescription.text = item.itemDescription;
        itemIcon.sprite = item.itemIcon;
        equipButtonText.text = itemUI.isEquiped ? "Take off" : "Equip"; 
        
    }

    public void CloseInfoWindow()
    {
        Destroy(gameObject);
    }

    public void OnEquipButtonClicked()
    {
        switch (itemUI.isEquiped)
        {
            case true:
                transform.parent.GetComponent<InventoryUI>().TakeOffItem(itemUI);
                break;
            case false:
                transform.parent.GetComponent<InventoryUI>().EquipItem(item);
                break;
        }
        CloseInfoWindow();
    }
}
