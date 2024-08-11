using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Item", order = 51)]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public ItemRarity itemRarity;
    public BonusType bonusType;
    public int bonusValue; // Значение бонуса
    public Sprite itemIcon; // Иконка предмета
}

public enum ItemType
{
    Ring,
    Bracelet,
    Glove
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum BonusType
{
    Attack,
    Defense
}
