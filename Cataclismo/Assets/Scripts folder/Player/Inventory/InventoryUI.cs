using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // Префаб ячейки предмета
    public Transform contentPanel; // Панель для добавления элементов
    public GridLayoutGroup gridLayoutGroup; // Компонент Grid Layout Group

    public GameObject ringSlot;
    public GameObject braceletSlot;
    public GameObject gloveSlot;

    void Start()
    {
        RefreshInventoryUI();
    }


    public void RefreshInventoryUI()
    {
        // Очистите старые элементы
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Добавьте новые элементы
        foreach (InventoryItem item in inventory.items)
        {
            GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.inventoryParent = this.transform;
            itemUI.Setup(item);
        }

        // Обновите размер Content, чтобы подстроиться под количество элементов
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void EquipItem(InventoryItem tempItem)
    {
            switch (tempItem.itemType)
            {
                case ItemType.Ring:
                    inventory.ring = tempItem;
                    ringSlot.GetComponent<Image>().sprite = tempItem.itemIcon;
                    break;
                case ItemType.Bracelet:
                    inventory.bracelet = tempItem;
                    braceletSlot.GetComponent<Image>().sprite = tempItem.itemIcon;
                    break;
                case ItemType.Glove:
                    inventory.glove = tempItem;
                    gloveSlot.GetComponent<Image>().sprite = tempItem.itemIcon;
                    break;
            }
    }
    
}
