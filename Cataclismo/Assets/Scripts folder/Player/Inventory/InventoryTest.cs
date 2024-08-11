using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public Inventory inventory;
    public InventoryItem item1;

    void Start()
    {
        inventory.AddItem(item1);
    }
}