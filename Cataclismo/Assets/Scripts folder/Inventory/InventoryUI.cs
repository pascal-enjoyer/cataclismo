using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // Префаб ячейки предмета
    public Transform canvas;
    public GameObject itemInfoWindowPrefab;
    public Transform contentPanel; // Панель для добавления элементов
    public GridLayoutGroup gridLayoutGroup; // Компонент Grid Layout Group

    public GameObject blacksmith;

    public List<EquipSlot> equipSlots;

    

    public Transform braceletOnHandSlot;
    public Transform ringOnHandSlot;
    public Transform gloveOnHandSlot;


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
        foreach (EquipSlot eqSlot in equipSlots)
        {
            if (eqSlot.equipedItem != null)
            {
                Destroy(eqSlot.equipedItem);
            }    
        }

        // Добавьте новые элементы
        foreach (InventoryItem item in inventory.items)
        {
            if (item.isEquiped)
            {
                foreach (EquipSlot eqSlot in equipSlots)
                {
                    if (eqSlot.ItemType == item.ItemType)
                    {
                        GameObject eqSlotObject = Instantiate(itemUIPrefab, eqSlot.transform);
                        ItemUI itemUi = eqSlotObject.GetComponent<ItemUI>();
                        itemUi.inventoryParent = transform;
                        itemUi.Setup(item);
                        itemUi.InventoryUI = this;
                        eqSlot.equipedItem = eqSlotObject;
                    }
                }
            }
            else
            {
                GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
                ItemUI itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.Setup(item);
                itemUI.InventoryUI = this;
            }

        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void EquipItem(InventoryItem tempItem)
    {
        foreach (EquipSlot slot in equipSlots)
        {
            if (slot.ItemType == tempItem.ItemType)
            {
                if (slot.equipedItem != null)
                {
                    slot.equipedItem.GetComponent<ItemUI>().item.isEquiped = false;

                }
                tempItem.isEquiped = true;
            }
        }
        RefreshInventoryUI();
    }

    public void TakeOffItem(InventoryItem tempItem)
    {
        tempItem.isEquiped = false;
        RefreshInventoryUI();
    }

    public void EnableBlackSmith()
    {
        GameObject curBSmith = Instantiate(blacksmith, transform);
        curBSmith.GetComponent<MergeInventory>().inventoryUI = this;

    }
    
}
