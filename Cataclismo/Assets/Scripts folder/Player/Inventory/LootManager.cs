using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    System.Random r = new System.Random();

    public Inventory inventory;
    public List<TemplateItem> possibleLoot = new List<TemplateItem>();

    public float commonProbability = 75;
    public float uncommonProbability = 17;
    public float rareProbability = 6;
    public float epicProbability = 1;
    public float legendaryProbability = 0.5f;
    public int bonusValueMin = 50;
    public int bonusValueMax = 100;



    public void Start()
    {
        inventory = GetComponent<Inventory>();
        possibleLoot = inventory.templates;
    }
    public void DropLoot()
    {
        InventoryItem newLoot = GenerateRandomLoot();
        inventory.AddItem(newLoot);
        inventory.SaveInventory();
    }

    private InventoryItem GenerateRandomLoot()
    {
        int levelNumber = 5; //временно, чтобы лут крутой не падал
        int randomIndex = r.Next(0, possibleLoot.Count);
        TemplateItem templateItem = possibleLoot[randomIndex];
        int bonusValue = r.Next(50, 100);
        float randItemRarity = r.Next(1, 10000)/100;
        ItemRarity itemRarity = ItemRarity.Common;
        if (randItemRarity > 0 && randItemRarity < Math.Abs(commonProbability))
            itemRarity = ItemRarity.Common;

        else if (randItemRarity > Math.Abs(commonProbability) &&
            randItemRarity < Math.Abs(commonProbability) + Math.Abs(uncommonProbability))
        {
            itemRarity = ItemRarity.Uncommon;
            bonusValue = (int)(bonusValue * 1.25f);
        }

        else if (randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability))
        {
            itemRarity = ItemRarity.Rare;

            bonusValue = (int)((bonusValue + 25) * 1.5f);
        }

        else if (levelNumber > 10 && randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability))
        {
            itemRarity = ItemRarity.Epic;

            bonusValue = (int)((bonusValue + 50) * 1.75f);
        }

        else if (levelNumber > 10 && randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability) + Math.Abs(legendaryProbability))
        {
            itemRarity = ItemRarity.Legendary;

            bonusValue = (int)((bonusValue + 100) * 2f);
        }
        
        InventoryItem item = new InventoryItem(templateItem, bonusValue, itemRarity);
        return item;
    }


}