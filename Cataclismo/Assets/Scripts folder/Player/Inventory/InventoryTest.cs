using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public Inventory inventory;

    void Start()
    {
        InventoryItem item1 = new InventoryItem
        {
            itemName = "Common Ring",
            itemType = ItemType.Ring,
            itemRarity = ItemRarity.Common,
            bonusType = BonusType.Attack,
            bonusValue = 5,
/*            itemIcon = *//* ������� ������ */
        };

        InventoryItem item2 = new InventoryItem
        {
            itemName = "Epic Glove",
            itemType = ItemType.Glove,
            itemRarity = ItemRarity.Epic,
            bonusType = BonusType.Defense,
            bonusValue = 20,
/*            itemIcon = *//* ������� ������ */
        };

        inventory.AddItem(item1);
        inventory.AddItem(item2);
    }
}