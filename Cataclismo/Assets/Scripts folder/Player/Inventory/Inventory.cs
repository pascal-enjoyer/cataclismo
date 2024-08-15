using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItem
{
    public InventoryItem() { }

    public InventoryItem(TemplateItem templateItem, int bonusValue, ItemRarity itemRarity, bool isEquiped = false)
    {
        item = templateItem;
        this.bonusValue = bonusValue;
        this.itemRarity = itemRarity;
        this.isEquiped = isEquiped;
    }


    public TemplateItem item;
    public int bonusValue; // Значение бонуса
    public ItemRarity itemRarity;
    public bool isEquiped = false;


    public Sprite ItemIcon => item.itemIcon;
    public string ItemName => item.itemName;
    public int ItemID => item.itemID;
    public ItemType ItemType => item.itemType;
    public BonusType BonusType => item.bonusType;
    public string ItemDescription => item.itemDescription;
}


public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}



public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public List<TemplateItem> templates = new List<TemplateItem>();

    public InventoryItem ring, bracelet, glove;

    public UnityEvent OnItemAdded;

    private void Start()
    {
        LoadInventory();
    }

    public void AddItem(TemplateItem template, ItemRarity itemRarity, int bonusValue)
    {
        InventoryItem item = new InventoryItem(template, bonusValue, itemRarity);

        items.Add(item);
        SaveInventory();

        OnItemAdded.Invoke();
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        SaveInventory();
        OnItemAdded.Invoke();
    }

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(new Serialization<InventoryItem>(items));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string json = PlayerPrefs.GetString("Inventory");
            items = JsonUtility.FromJson<Serialization<InventoryItem>>(json).items.ToList();
        }
    }

    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> items;
        public Serialization(List<T> items)
        {
            this.items = items;
        }
    }
}