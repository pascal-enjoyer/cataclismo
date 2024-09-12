using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInShop : MonoBehaviour
{



    public Chest chest;


    public string ChestDescription => chest.chestDescription;
    public string ChestName => chest.chestName;
    public int ChestCost => chest.chestCost;
    public Sprite ChestIcon => chest.chestIcon;

    public Image chestIconImage;
    public Text chestNameText;
    public Text chestCostText;
    public Text chestDescriptionText;
    public Button buyChestButton;

    public List<TemplateItem> lootVariants;

    public List<ItemRarity> itemRarityVariants;
    public List<ItemType> itemTypeVariants;
    public List<BonusType> itemBonusTypeVariants;


    public void Start()
    {
        if ((itemRarityVariants == null || itemRarityVariants.Count == 0) &&
            (itemTypeVariants == null || itemTypeVariants.Count == 0) &&
            (itemBonusTypeVariants == null || itemBonusTypeVariants.Count == 0))
        {
            lootVariants = GameManager.inventory.templates;
        }
        SetupChests();
    }

    public void SetupChests()
    {
        chestIconImage.sprite = ChestIcon;
        chestNameText.text = ChestName;
        chestCostText.text = ChestCost.ToString();
        chestDescriptionText.text = ChestDescription;
    }
}
