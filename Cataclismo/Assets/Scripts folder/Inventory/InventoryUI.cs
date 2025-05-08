using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;


    [Header("Equip slots")]
    public Transform braceletOnHandSlot;
    public Transform ringOnHandSlot;
    public Transform gloveOnHandSlot;

    public Transform canvas;

    public GameObject itemUIPrefab; // Префаб ячейки предмета

    public GameObject itemInfoWindowPrefab; //префаб инфы о предмете

    public Transform inventoryContentPanel; // Панель для добавления элементов
    public GridLayoutGroup gridLayoutGroup; // Компонент Grid Layout Group

    public GameObject blacksmith;

    public List<EquipSlot> equipSlots;

    

    void Start()
    {
        inventory = GameManager.Instance.inventory;
        inventory.OnItemAdded.AddListener(RefreshInventoryUI);

        RefreshInventoryUI();
    }


    public void RefreshInventoryUI()
    {
        
        // Очистите старые элементы
        foreach (Transform child in inventoryContentPanel)
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
                        itemUi.Setup(item);
                        eqSlot.equipedItem = eqSlotObject;
                        itemUi.OnItemUIClicked.AddListener(OnInventoryItemClicked);
                    }
                }
            }
            else
            {
                GameObject newItem = Instantiate(itemUIPrefab, inventoryContentPanel);
                ItemUI itemUI = newItem.GetComponent<ItemUI>();
                itemUI.Setup(item);

                itemUI.OnItemUIClicked.AddListener(OnInventoryItemClicked);
            }

        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContentPanel.GetComponent<RectTransform>());

    }

    public void EquipItem(InventoryItem tempItem)
    {
        foreach (EquipSlot slot in equipSlots)
        {
            if (slot.ItemType == tempItem.ItemType)
            {
                if (slot.equipedItem != null)
                {
                    inventory.TakeOffItem(slot.equipedItem.GetComponent<ItemUI>().item);

                }
                inventory.EquipItem(tempItem);  
            }
        }

    }

    public void TakeOffItem(InventoryItem tempItem)
    {
        inventory.TakeOffItem(tempItem);
    }

    public void EnableBlackSmith()
    {
        GameObject curBSmith = Instantiate(blacksmith, transform);

    }

    public void OnInventoryItemClicked(ItemUI item)
    {
        ItemInfoWindow newWindow = Instantiate(itemInfoWindowPrefab, canvas).GetComponent<ItemInfoWindow>();
        newWindow.FillWindow(item.item, this);
        Debug.Log(item.item.ItemName);
    }
    
}
