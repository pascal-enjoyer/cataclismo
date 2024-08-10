using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;

    public void Setup(InventoryItem item)
    {
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
    }
}