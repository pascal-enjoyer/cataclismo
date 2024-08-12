using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public InventoryItem ring;
    public InventoryItem bracelet;
    public InventoryItem glove;

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        // Обновите UI при добавлении нового предмета
        // Например, вызовите метод RefreshInventoryUI()
    }
}