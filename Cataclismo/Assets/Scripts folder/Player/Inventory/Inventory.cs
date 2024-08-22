using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItem
{
    public InventoryItem(TemplateItem templateItem, int bonusValue, ItemRarity itemRarity, bool isEquiped = false, int itemLevel = 1)
    {
        item = templateItem;
        this.bonusValue = bonusValue;
        this.itemRarity = itemRarity;
        this.isEquiped = isEquiped;
        this.itemLevel = itemLevel;
    }


    public TemplateItem item;
    public int bonusValue; // Значение бонуса
    public ItemRarity itemRarity;
    public bool isEquiped = false;
    public Sprite rarityBackgroundSprite;
    public int itemLevel = 1;
    public int maxItemLevel = 50;
    public int itemLevelUpgradeCost = 500;
    public int addedCostOfUpgradePerLevel = 100;

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




[System.Serializable]
public class SerializableInventoryItem
{
    public int templateItemID;
    public int bonusValue;
    public ItemRarity itemRarity;
    public bool isEquiped;
    public int itemLevel;

    public SerializableInventoryItem(InventoryItem item)
    {
        templateItemID = item.ItemID;
        bonusValue = item.bonusValue;
        itemRarity = item.itemRarity;
        isEquiped = item.isEquiped;
        itemLevel = item.itemLevel;

    }

    public InventoryItem ToInventoryItem(List<TemplateItem> templates)
    {
        TemplateItem template = templates.FirstOrDefault(t => t.itemID == templateItemID);
        if (template == null)
        {
            
            return null;
        }

        InventoryItem item = new InventoryItem(template, bonusValue, itemRarity, isEquiped, itemLevel);
        return item;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public List<TemplateItem> templates = new List<TemplateItem>();

    public InventoryItem ring, bracelet, glove;

    public Sprite commonSprite, uncommonSprite, rareSprite, epicSprite, legendarySprite;

    public UnityEvent OnItemAdded;

    private void Start()
    {
        LoadInventory();
        // временно


    }


    public void UpgradeItemLevel(InventoryItem item)
    {
        if (item.itemLevel + 1 <= item.maxItemLevel)
        {
            item.itemLevel++;
            item.bonusValue += 10;

            SaveInventory();

            OnItemAdded.Invoke();
        }
    }

    public void AddItem(TemplateItem template, ItemRarity itemRarity, int bonusValue, int itemLevel = 1)
    {
        InventoryItem item = new InventoryItem(template, bonusValue, itemRarity, false, itemLevel);
        ChooseItemBackGroundImage(item);
        items.Add(item);
        SortByRarityFromHighest();
        SaveInventory();
        OnItemAdded.Invoke();
    }

    public void AddItem(InventoryItem item)
    {
        ChooseItemBackGroundImage(item);
        items.Add(item);
        SortByRarityFromHighest();
        SaveInventory();

        OnItemAdded.Invoke();
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        SortByRarityFromHighest();
        SaveInventory();

        OnItemAdded.Invoke();
    }


    public void ClearSave()
    {
        items.Clear();
        ring = null;
        bracelet = null;
        glove = null;

        PlayerPrefs.DeleteKey("Inventory");
        OnItemAdded.Invoke();
    }
    public void SaveInventory()
    {
        List<SerializableInventoryItem> serializableItems = items.Select(i => new SerializableInventoryItem(i)).ToList();

        InventoryData inventoryData = new InventoryData
        {
            items = serializableItems,
            ring = ring != null ? new SerializableInventoryItem(ring) : null,
            bracelet = bracelet != null ? new SerializableInventoryItem(bracelet) : null,
            glove = glove != null ? new SerializableInventoryItem(glove) : null
        };

        string json = JsonUtility.ToJson(inventoryData);
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string json = PlayerPrefs.GetString("Inventory");
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

            items = inventoryData.items
                .Select(s => s.ToInventoryItem(templates))
                .Where(item => item != null)
                .ToList();

            foreach (var item in items)
            {
                ChooseItemBackGroundImage(item);
            }

            ring = inventoryData.ring?.ToInventoryItem(templates);
            bracelet = inventoryData.bracelet?.ToInventoryItem(templates);
            glove = inventoryData.glove?.ToInventoryItem(templates);

            if (ring != null) ChooseItemBackGroundImage(ring);
            if (bracelet != null) ChooseItemBackGroundImage(bracelet);
            if (glove != null) ChooseItemBackGroundImage(glove);

            OnItemAdded.Invoke();
        }
    }


    public void SortByRarityFromHighest()
    {
        items = items.OrderByDescending(item => item.itemRarity)
                     .ThenBy(item => item.ItemID)
                     .ThenBy(item => item.itemLevel)
                     .ToList();

        SaveInventory();
        OnItemAdded.Invoke();
    }

    public void SortByRarityFromLowest()
    {
        items = items.OrderBy(item => item.itemRarity)
                     .ThenBy(item => item.ItemID)
                     .ThenBy(item => item.itemLevel)
                     .ToList();

        SaveInventory();
        OnItemAdded.Invoke();
    }


    private void ChooseItemBackGroundImage(InventoryItem item)
    {
        switch (item.itemRarity)
        {
            case ItemRarity.Common:
                item.rarityBackgroundSprite = commonSprite;
                break;
            case ItemRarity.Uncommon:
                item.rarityBackgroundSprite = uncommonSprite;
                break;
            case ItemRarity.Rare:
                item.rarityBackgroundSprite = rareSprite;
                break;
            case ItemRarity.Epic:
                item.rarityBackgroundSprite = epicSprite;
                break;
            case ItemRarity.Legendary:
                item.rarityBackgroundSprite = legendarySprite;
                break;
        }
    }

    [System.Serializable]
    private class InventoryData
    {
        public List<SerializableInventoryItem> items;
        public SerializableInventoryItem ring;
        public SerializableInventoryItem bracelet;
        public SerializableInventoryItem glove;
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