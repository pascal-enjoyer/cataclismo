using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public Inventory inventory;
    public InventoryItem item;

    void Start()
    {
        inventory.AddItem(item);
    }
}