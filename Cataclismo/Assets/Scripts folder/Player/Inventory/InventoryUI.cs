using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemUIPrefab; // ������ ��� ����������� ��������
    public Transform contentPanel; // ������, ���� ����� ����������� �������� ���������

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
            itemUI.Setup(item);
        }
    }
}