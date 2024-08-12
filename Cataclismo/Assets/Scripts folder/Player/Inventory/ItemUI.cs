using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    private InventoryItem item;
    public GameObject itemInfoWindowPrefab;
    public Transform inventoryParent;

    public void Setup(InventoryItem tempItem)
    {
        transform.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        item = tempItem;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
    }
    
    public void OnButtonClick()
    {
        ItemInfoWindow infoWindow = itemInfoWindowPrefab.GetComponent<ItemInfoWindow>();
        if (infoWindow != null)
        {
            infoWindow.FillWindow(item);
            Instantiate(itemInfoWindowPrefab, inventoryParent);
        }
    }
}