using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public InventoryItem item;
    public Transform inventoryParent;
    public Image itemRarityBackground;
    public InventoryUI InventoryUI;
    public bool isInMerge = false;
    public bool isTaked = false;
    public bool isResult = false;

    

    public enum ItemUIStage
    {
        none,
        IsInMerge,
        IsTaked,
        Result
    };

    public void Setup(InventoryItem tempItem)
    {
        transform.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        item = tempItem;

        itemIcon.sprite = item.ItemIcon;
        itemName.text = "Lv." + item.itemLevel.ToString();
        itemRarityBackground.sprite = item.rarityBackgroundSprite;
    }
    
    public void OnButtonClick()
    {
        if (isInMerge)
        {
            if (isResult)
            {
                
                ItemInfoWindow infoWindow = InventoryUI.itemInfoWindowPrefab.GetComponent<ItemInfoWindow>();
                if (infoWindow != null)
                {
                    GameObject gameObject = Instantiate(InventoryUI.itemInfoWindowPrefab, inventoryParent);
                    ItemInfoWindow itemInfoWindow = gameObject.GetComponent<ItemInfoWindow>();
                    
                    itemInfoWindow.itemUI = this;
                    itemInfoWindow.FillWindow(item);
                    itemInfoWindow.inventoryUI = InventoryUI;

                }
            }
            else if (!isTaked)
            {
                if (inventoryParent.GetComponent<MergeInventory>().mergeItems.Count < 3)
                {
                    inventoryParent.GetComponent<MergeInventory>().MoveItemToMergeSlots(item);
                    itemIcon.color = Color.gray;
                    itemRarityBackground.color = Color.gray;
                    itemName.color = Color.gray;
                    isTaked = true;
                }
            }
            else 
            {
                itemIcon.color = Color.white;
                itemRarityBackground.color = Color.white;
                itemName.color = Color.white;
                isTaked = false;

                inventoryParent.GetComponent<MergeInventory>().ExecuteItemFromMergeSlots(item);
            }

        }
        else
        {
            ItemInfoWindow infoWindow = InventoryUI.itemInfoWindowPrefab.GetComponent<ItemInfoWindow>();
            if (infoWindow != null)
            {
                GameObject gameObject = Instantiate(InventoryUI.itemInfoWindowPrefab, inventoryParent.GetComponent<InventoryUI>().canvas);
                ItemInfoWindow itemInfoWindow = gameObject.GetComponent<ItemInfoWindow>();
                itemInfoWindow.FillWindow(item);
                itemInfoWindow.inventoryUI = InventoryUI;

            }
        }
    }

}