using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RarityProbability
{
    public float probability;
    public ItemRarity rarity;
}

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

    public List<TemplateItem> currentChestLootVariants;
    public List<TemplateItem> allVariants;

    public List<RarityProbability> rarityProbabilitiesList;
    public Dictionary<float, ItemRarity> itemRarityVariants;

    public List<ItemType> itemTypeVariants;
    public List<BonusType> itemBonusTypeVariants;

    public InventoryItem droppedItem;


    public void Start()
    {
        allVariants = GameManager.inventory.templates;

        itemRarityVariants = new Dictionary<float, ItemRarity>();
        foreach (RarityProbability rp in rarityProbabilitiesList)
        {
            itemRarityVariants.Add(rp.probability, rp.rarity);
        }
        if (itemRarityVariants == null || itemRarityVariants.Count == 0)
        {
            SetDefaultRarityProbabilities();
        }
        FilterLootVariants();
        SetupChests();
        buyChestButton.onClick.AddListener(OpenChest);
    }

    public void FilterLootVariants()
    {
        // ���� �� ���� �� ��������� �� ����������, ����� ��� ��������� ��������
        if ((itemRarityVariants == null || itemRarityVariants.Count == 0) &&
            (itemTypeVariants == null || itemTypeVariants.Count == 0) &&
            (itemBonusTypeVariants == null || itemBonusTypeVariants.Count == 0))
        {
            currentChestLootVariants = new List<TemplateItem>(allVariants);
        }
        else
        {
            currentChestLootVariants = new List<TemplateItem>();

            // �������� �� ���� ��������� ���������
            foreach (TemplateItem item in allVariants)
            {
                bool typeMatches = itemTypeVariants == null || itemTypeVariants.Count == 0 || itemTypeVariants.Contains(item.itemType);
                bool bonusTypeMatches = itemBonusTypeVariants == null || itemBonusTypeVariants.Count == 0 || itemBonusTypeVariants.Contains(item.bonusType);

                // ��������� ������� � ������, ���� ��� �������� ���������
                if (typeMatches && bonusTypeMatches)
                {
                    currentChestLootVariants.Add(item);
                }
            }
        }
    }

    public void SetupChests()
    {
        chestIconImage.sprite = ChestIcon;
        chestNameText.text = ChestName;
        chestCostText.text = ChestCost.ToString();
        chestDescriptionText.text = ChestDescription;
    }
    public void OpenChest()
    {
        if (currentChestLootVariants == null || currentChestLootVariants.Count == 0)
        {
            Debug.LogWarning("��� ��������� ��������� ���� � ���� �������.");
            return;
        }

        // ��������� ���������� ��������
        System.Random random = new System.Random();
        int randomIndex = random.Next(0, currentChestLootVariants.Count);
        TemplateItem randomItem = currentChestLootVariants[randomIndex];

        ItemRarity itemRarity = GetRandomRarity();

        // ��������� ������� � ��������� ������ � ��������� ���������
        GameManager.inventory.AddItem(new InventoryItem(randomItem, LootManager.GenerateBonusValueByRarity(itemRarity), itemRarity));

        Debug.Log($"�� ������� ������ � ��������: {randomItem.itemName} � ��������� {itemRarity}");
    }

    // ������������� ����������� ����������� ��� ���������
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


        // �������� �� ������� ������������ � �������� ��������
        foreach (KeyValuePair<float, ItemRarity> rarityEntry in itemRarityVariants)
        {
            cumulativeProbability += rarityEntry.Key;
            if (randomValue <= cumulativeProbability)
            {
                return rarityEntry.Value;
            }
        }

        // ���� ������ �� ��������� (����������� ������), ���������� ���������� ��������
        return ItemRarity.Common;
    }
}
