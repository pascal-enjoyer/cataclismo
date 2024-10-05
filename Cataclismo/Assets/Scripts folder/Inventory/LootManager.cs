using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    static System.Random r = new System.Random();

    public Inventory inventory;
    public List<TemplateItem> possibleLoot = new List<TemplateItem>();

    public float commonProbability = 94;
    public float uncommonProbability = 4;
    public float rareProbability = 1;
    public float epicProbability = 0.75f;
    public float legendaryProbability = 0.25f;
    public int bonusValueMin = 50;
    public int bonusValueMax = 100;

    public List<InventoryItem> lastDroppedLoot;


    public void Start()
    {
        lastDroppedLoot = new List<InventoryItem>();
        inventory = GetComponent<Inventory>();
        possibleLoot = inventory.templates;
    }
    public void DropLoot()
    {
        lastDroppedLoot.Clear();
        InventoryItem newLoot = GenerateRandomLoot();
        inventory.AddItem(newLoot);
        lastDroppedLoot.Add(newLoot);
        inventory.SaveInventory();
    }

    private InventoryItem GenerateRandomLoot()
    {
        int levelNumber = 1; //временно, чтобы лут крутой не падал
        int randomIndex = r.Next(0, possibleLoot.Count);
        TemplateItem templateItem = possibleLoot[randomIndex];
        int bonusValue = r.Next(50, 100);
        float randItemRarity = r.Next(1, 10000) / 100;
        ItemRarity itemRarity = ItemRarity.Common;
        if (randItemRarity > 0 && randItemRarity < Math.Abs(commonProbability))
            itemRarity = ItemRarity.Common;

        else if (randItemRarity > Math.Abs(commonProbability) &&
            randItemRarity < Math.Abs(commonProbability) + Math.Abs(uncommonProbability))
        {
            itemRarity = ItemRarity.Uncommon;
        }

        else if (randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability))
        {
            itemRarity = ItemRarity.Rare;

        }

        else if (levelNumber > 3 && randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability))
        {
            itemRarity = ItemRarity.Epic;

        }

        else if (levelNumber > 3 && randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability) + Math.Abs(legendaryProbability))
        {
            itemRarity = ItemRarity.Legendary;

        }

        bonusValue = GenerateBonusValueByRarity(itemRarity);
        InventoryItem item = new InventoryItem(templateItem, bonusValue, itemRarity);
        return item;
    }

    public static int GenerateBonusValueByRarity(ItemRarity rarity, int ifNotRandom = -1)
    {
        int result = 0;

        int bonusValue = ifNotRandom == -1 ? r.Next(50, 101) : ifNotRandom;
        switch (rarity)
        {
            case ItemRarity.Legendary:
                result = (int)((bonusValue + 100) * 2f);
                break;
            case ItemRarity.Epic:
                result = (int)((bonusValue + 50) * 1.75f);
                break;
            case ItemRarity.Rare:
                result = (int)((bonusValue + 25) * 1.5f);
                break;
            case ItemRarity.Uncommon:
                result = (int)(bonusValue * 1.25f);
                break;
            case ItemRarity.Common:
                result = (bonusValue);
                break;
        }
        return result;
    }
}