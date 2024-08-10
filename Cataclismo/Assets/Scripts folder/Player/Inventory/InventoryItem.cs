using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public ItemType itemType;
    public ItemRarity itemRarity;
    public BonusType bonusType;
    public int bonusValue; // Значение бонуса
    public Sprite itemIcon; // Иконка предмета
}