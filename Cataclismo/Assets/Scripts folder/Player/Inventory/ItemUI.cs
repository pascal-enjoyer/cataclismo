using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public InventoryItem item;
    public GameObject itemInfoWindowPrefab;
    public Transform inventoryParent;


    public void Setup(InventoryItem tempItem)
    {
        transform.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        item = tempItem;
        itemIcon.sprite = item.ItemIcon;
        itemName.text = item.bonusValue.ToString();
    }
    
    public void OnButtonClick()
    {
        ItemInfoWindow infoWindow = itemInfoWindowPrefab.GetComponent<ItemInfoWindow>();
        if (infoWindow != null)
        {   
            //infoWindow.FillWindow(item);
            GameObject gameObject = Instantiate(itemInfoWindowPrefab, inventoryParent);
            gameObject.GetComponent<ItemInfoWindow>().FillWindow(item);
        }
    }
}