using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    System.Random r = new System.Random();

    public Inventory inventory;
    public List<TemplateItem> possibleLoot = new List<TemplateItem>();

    public int commonProbability = 50;
    public int uncommonProbability = 20;
    public int rareProbability = 15;
    public int epicProbability = 10;
    public int legendaryProbability = 5;
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
        int randomIndex = r.Next(0, possibleLoot.Count);
        TemplateItem templateItem = possibleLoot[randomIndex];
        int bonusValue = r.Next(50, 100);
        int randItemRarity = r.Next(0, 101);
        ItemRarity itemRarity = ItemRarity.Common;
        if (randItemRarity > 0 && randItemRarity < Math.Abs(commonProbability))
            itemRarity = ItemRarity.Common; 

        if (randItemRarity > Math.Abs(commonProbability) && 
            randItemRarity < Math.Abs(commonProbability)  + Math.Abs(uncommonProbability)) 
            itemRarity = ItemRarity.Uncommon; 

        if (randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) 
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability)) 
            itemRarity = ItemRarity.Rare; 

        if (randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability)) 
            itemRarity = ItemRarity.Epic; 

        if (randItemRarity > Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability)
            && randItemRarity < Math.Abs(uncommonProbability) + Math.Abs(commonProbability) + Math.Abs(rareProbability) + Math.Abs(epicProbability) + Math.Abs(legendaryProbability))
            itemRarity = ItemRarity.Legendary; 

        InventoryItem item = new InventoryItem(templateItem, bonusValue, itemRarity);
        return item;
    }


}