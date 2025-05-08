using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ChestInShop : MonoBehaviour
{



    public Chest chest;


    public Image chestIconImage;
    public Text chestNameText;
    public Text chestCostText;
    public Text chestDescriptionText;
    public Button buyChestButton;
    public Image chestBackground;

    public List<TemplateItem> currentChestLootVariants;
    public List<TemplateItem> allVariants;

    public List<RarityProbability> rarityProbabilitiesList;
    public Dictionary<float, ItemRarity> itemRarityVariants;

    public List<ItemType> itemTypeVariants;
    public List<BonusType> itemBonusTypeVariants;

    public InventoryItem droppedItem;

    public UnityEvent<InventoryItem> OnItemDropped;

    public void FilterLootVariants()
    {
        // ≈сли ни один из критериев не установлен, берем все возможные варианты
        if ((itemRarityVariants == null || itemRarityVariants.Count == 0) &&
            (itemTypeVariants == null || itemTypeVariants.Count == 0) &&
            (itemBonusTypeVariants == null || itemBonusTypeVariants.Count == 0))
        {
            currentChestLootVariants = new List<TemplateItem>(allVariants);
        }
        else
        {
            currentChestLootVariants = new List<TemplateItem>();

            // ѕроходим по всем возможным вариантам
            foreach (TemplateItem item in allVariants)
            {
                bool typeMatches = itemTypeVariants == null || itemTypeVariants.Count == 0 || itemTypeVariants.Contains(item.itemType);
                bool bonusTypeMatches = itemBonusTypeVariants == null || itemBonusTypeVariants.Count == 0 || itemBonusTypeVariants.Contains(item.bonusType);

                // ƒобавл€ем предмет в список, если все критерии совпадают
                if (typeMatches && bonusTypeMatches)
                {
                    currentChestLootVariants.Add(item);
                }
            }
        }
    }

    public void SetupChests(Chest chest)
    {
        this.chest = chest;
        chestIconImage.sprite = chest.chestIcon;
        chestNameText.text = chest.chestName;
        chestCostText.text = chest.chestCost.ToString();
        chestDescriptionText.text = chest.chestDescription;
        chestBackground.sprite = chest.chestBackground;

        allVariants = GameManager.Instance.inventory.templates;
        itemRarityVariants = new Dictionary<float, ItemRarity>();
        rarityProbabilitiesList = chest.rarityProbabilitiesList;

        foreach (RarityProbability rp in rarityProbabilitiesList)
        {
            itemRarityVariants.Add(rp.probability, rp.rarity);
        }
        if (itemRarityVariants == null || itemRarityVariants.Count == 0)
        {
            SetDefaultRarityProbabilities();
        }
        FilterLootVariants();
        buyChestButton.onClick.AddListener(OpenChest);
    }
    public void OpenChest()
    {
        if (GameManager.Instance.playerEconomic.diamonds - chest.chestCost >= 0)
        {
            GameManager.Instance.playerEconomic.GainDiamonds(-chest.chestCost);
            if (currentChestLootVariants == null || currentChestLootVariants.Count == 0)
            {
                Debug.LogWarning("Ќет доступных вариантов лута в этом сундуке.");
                return;
            }

            // √енераци€ случайного предмета
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, currentChestLootVariants.Count);
            TemplateItem randomItem = currentChestLootVariants[randomIndex];

            ItemRarity itemRarity = GetRandomRarity();

            // ƒобавл€ем предмет в инвентарь игрока с выбранной редкостью
            droppedItem = new InventoryItem(randomItem, LootManager.GenerateBonusValueByRarity(itemRarity), itemRarity);
            GameManager.Instance.inventory.AddItem(droppedItem);

            OnItemDropped.Invoke(droppedItem);
            Debug.Log($"¬ы открыли сундук и получили: {randomItem.itemName} с редкостью {itemRarity}");
        }
        
    }

    // ”станавливаем стандартные веро€тности дл€ редкостей
    private void SetDefaultRarityProbabilities()
    {
        itemRarityVariants = new Dictionary<float, ItemRarity>
        {
            { 50f, ItemRarity.Common },
            { 30f, ItemRarity.Uncommon },
            { 15f, ItemRarity.Rare },
            { 4f, ItemRarity.Epic },
            { 1f, ItemRarity.Legendary }
        };
    }
    private ItemRarity GetRandomRarity()
    {
        float randomValue = Random.Range(0f, 100f);
        float cumulativeProbability = 0f;


        // ѕроходим по словарю веро€тностей и выбираем редкость
        foreach (KeyValuePair<float, ItemRarity> rarityEntry in itemRarityVariants)
        {
            cumulativeProbability += rarityEntry.Key;
            if (randomValue <= cumulativeProbability)
            {
                return rarityEntry.Value;
            }
        }

        // ≈сли ничего не выбралось (пограничный случай), возвращаем наименьшую редкость
        return ItemRarity.Common;
    }

}
