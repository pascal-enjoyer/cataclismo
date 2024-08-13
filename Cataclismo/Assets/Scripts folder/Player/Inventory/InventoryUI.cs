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
    public GameObject ringSlotImage;

    public GameObject braceletSlot;
    public GameObject braceletSlotImage;

    public GameObject gloveSlot;
    public GameObject gloveSlotImage;

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
            itemUI.inventoryParent = transform;
            itemUI.Setup(item);
        }

        // Обновите размер Content, чтобы подстроиться под количество элементов
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void EquipItem(InventoryItem tempItem)
    {
        GameObject newItem;
        ItemUI itemUI;
        switch (tempItem.itemType)
        {
            case ItemType.Ring:
            if (inventory.ring == null)
            {
                inventory.ring = tempItem;
                newItem = Instantiate(itemUIPrefab, ringSlot.transform);
                itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.isEquiped = true;
                itemUI.Setup(tempItem);
                inventory.items.Remove(tempItem);
            }
            break;
            case ItemType.Bracelet:
            if (inventory.bracelet == null)
            {
                inventory.bracelet = tempItem;
                newItem = Instantiate(itemUIPrefab, braceletSlot.transform);
                itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.isEquiped = true;
                itemUI.Setup(tempItem);

                    inventory.items.Remove(tempItem);
                }
            break;
            case ItemType.Glove:
            if (inventory.glove == null)
            {
                inventory.glove = tempItem;
                newItem = Instantiate(itemUIPrefab, gloveSlot.transform);
                itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.isEquiped = true;
                itemUI.Setup(tempItem);
                    inventory.items.Remove(tempItem);
                }
            break;
        }
        RefreshInventoryUI();
    }

    public void TakeOffItem(ItemUI tempItem)
    {
        ItemUI itemUI = tempItem;
        InventoryItem item = itemUI.item;
        switch (tempItem.item.itemType)
        {
            case ItemType.Ring:
                if (inventory.ring == item)
                {
                    inventory.ring = null;
                    inventory.items.Add(item);
                    Destroy(tempItem.gameObject);
                    
                }
                break;
            case ItemType.Bracelet:
                if (inventory.bracelet == item)
                {
                    inventory.bracelet = null; 
                    inventory.items.Add(item);
                    Destroy(tempItem.gameObject);
                }
                break;
            case ItemType.Glove:
                if (inventory.glove == item)
                {
                    inventory.glove = null;
                    inventory.items.Add(item);

                    Destroy(tempItem.gameObject);
                }
                break;
        }
        RefreshInventoryUI();
    }
    
}
