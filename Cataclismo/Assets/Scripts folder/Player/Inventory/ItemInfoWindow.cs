using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    public InventoryItem item;
    public Text itemName;
    public Text itemDescription;
    public Image itemIcon;
    

    public void FillWindow(InventoryItem tempItem)
    {
        item = tempItem;
        itemName.text = item.name;
        itemDescription.text = item.itemDescription;
        itemIcon.sprite = item.itemIcon;
    }

    public void CloseInfoWindow()
    {
        Destroy(gameObject);
    }

    public void OnEquipButtonClicked()
    {
        transform.parent.GetComponent<InventoryUI>().EquipItem(item);
        CloseInfoWindow();
    }
}
