using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // ������ ������ ��������
    public Transform contentPanel; // ������ ��� ���������� ���������
    public GridLayoutGroup gridLayoutGroup; // ��������� Grid Layout Group

    public GameObject ringSlot;
    public GameObject braceletSlot;
    public GameObject gloveSlot;

    void Start()
    {
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
            itemUI.inventoryParent = this.transform;
            itemUI.Setup(item);
        }

        // �������� ������ Content, ����� ������������ ��� ���������� ���������
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
