using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MergeInventory : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // ������ ������ ��������
    public Transform contentPanel; // ������ ��� ���������� ���������
    public GridLayoutGroup gridLayoutGroup; // ��������� Grid Layout Group

    public GridLayoutGroup mergeSlots;

    public Transform mergeResultSlot;

    public List<InventoryItem> inventoryItems;

    public void Start()
    {
        inventory = GameManager.inventory;

        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        // �������� ������ ��������
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // �������� ����� ��������
        foreach (InventoryItem item in inventory.items)
        {
            GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(item);
            itemUI.isInMerge = true;
        }

        // �������� ������ Content, ����� ������������ ��� ���������� ���������
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void MoveItemToMergeSlots(InventoryItem item)
    {

    }


    public void ExecuteItemFromMergeSlots(InventoryItem item)
    {

    }

    public void SpawnResultItem(List<InventoryItem> mergeItems)
    {

    }

    public void MergeItems(InventoryItem mergeResult)
    {

    }
}
