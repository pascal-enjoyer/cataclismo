using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // Префаб ячейки предмета
    public Transform contentPanel; // Панель для добавления элементов
    public GridLayoutGroup gridLayoutGroup; // Компонент Grid Layout Group

    public Transform ringSlot;
    public Transform ringSlotImage;
    private GameObject ringUIObject;

    public Transform braceletSlot;
    public Transform braceletSlotImage;
    private GameObject braceletUIObject;

    public Transform gloveSlot;
    public Transform gloveSlotImage;
    private GameObject gloveUIObject;

    void Start()
    {
        inventory = GameManager.inventory;
        inventory.OnItemAdded.AddListener(RefreshInventoryUI);

        RefreshInventoryUI();
    }


    public void RefreshInventoryUI()
    {
        // Очистите старые элементы
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        Destroy(ringUIObject);
        Destroy(braceletUIObject);
        Destroy(gloveUIObject);

        // Добавьте новые элементы
        foreach (InventoryItem item in inventory.items)
        {
            GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(item);
        }

        if (inventory.ring != null)
        {
            ringUIObject = Instantiate(itemUIPrefab, ringSlot);
            ItemUI itemUI = ringUIObject.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(inventory.ring);
        }

        if (inventory.bracelet != null)
        {
            braceletUIObject = Instantiate(itemUIPrefab, braceletSlot);
            ItemUI itemUI = braceletUIObject.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(inventory.bracelet);
        }


        if (inventory.glove != null)
        {
            gloveUIObject = Instantiate(itemUIPrefab, gloveSlot);
            ItemUI itemUI = gloveUIObject.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(inventory.glove);
        }
        // Обновите размер Content, чтобы подстроиться под количество элементов
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void EquipItem(InventoryItem tempItem)
    {
        // ошибка в том что instanciate два а destroy один
        InventoryItem wasInSlot;
        switch (tempItem.ItemType)
        {
            case ItemType.Ring:
                wasInSlot = inventory.ring;

                inventory.ring = tempItem;
                inventory.ring.isEquiped = true;
                inventory.items.Remove(tempItem);

                if (wasInSlot != null)
                {
                    wasInSlot.isEquiped = false;
                    inventory.AddItem(wasInSlot);
                }

                break;
            case ItemType.Bracelet:
                wasInSlot = inventory.bracelet;

                inventory.bracelet = tempItem;
                inventory.bracelet.isEquiped = true;
                inventory.items.Remove(tempItem);


                if (wasInSlot != null)
                {
                    wasInSlot.isEquiped = false;
                    inventory.AddItem(wasInSlot);
                }
            break;
            case ItemType.Glove:
                wasInSlot = inventory.glove;

                inventory.glove = tempItem;
                inventory.glove.isEquiped = true;
                inventory.items.Remove(tempItem);


                if (wasInSlot != null)
                {
                    wasInSlot.isEquiped = false;
                    inventory.AddItem(wasInSlot);
                }
            break;
        }
        RefreshInventoryUI();
    }

    public void TakeOffItem(InventoryItem tempItem)
    {
        InventoryItem item = tempItem;
        
        switch (item.ItemType)
        {
            case ItemType.Ring:
                if (inventory.ring == item)
                {
                    inventory.ring = null;
                    item.isEquiped = false;
                    inventory.AddItem(item);
                }
                break;
            case ItemType.Bracelet:
                if (inventory.bracelet == item)
                {
                    inventory.bracelet = null;

                    item.isEquiped = false;
                    inventory.AddItem(item);


                }
                break;
            case ItemType.Glove:
                if (inventory.glove == item)
                {
                    inventory.glove = null;

                    item.isEquiped = false;
                    inventory.AddItem(item);
                }
                break;
        }
        RefreshInventoryUI();
    }
    
}
