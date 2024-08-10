using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        // �������� UI ��� ���������� ������ ��������
        // ��������, �������� ����� RefreshInventoryUI()
    }
}