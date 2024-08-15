using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    private InventoryItem item;
    private ItemUI itemUi;
    public Text itemName;
    public Text itemDescription;
    public Image itemIcon;
    public Text equipButtonText;
    

    public void FillWindow(InventoryItem tempItem)
    {
        item = tempItem;
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
        itemIcon.sprite = item.ItemIcon;
        equipButtonText.text = item.isEquiped ? "Take off" : "Equip"; 
    }

    public void CloseInfoWindow()
    {
        Destroy(gameObject);
    }

    public void OnEquipButtonClicked()
    {

        switch (item.isEquiped)
        {
            case true:
                transform.parent.GetComponent<InventoryUI>().TakeOffItem(item);
                break;
            case false:
                transform.parent.GetComponent<InventoryUI>().EquipItem(item);
                break;
        }
        CloseInfoWindow();
    }
}
