using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

//при нажатии как определить его MergeInventoryItemUI

public enum ItemUIState
{
    isInInventory,
    isInMerge,
    isResult,
    isUsedInMergeResult
}
public class MergeInventoryItemUI
{
    public InventoryItem? mergeResult;
    public ItemUIState itemUIState;
    public MergeInventoryItemUI(ItemUIState itemUIState, InventoryItem mergeResult = null)
    {
        this.itemUIState = itemUIState;
        this.mergeResult = mergeResult;
    }
}

public class MergeInventory : MonoBehaviour
{
    
    
    public Inventory inventory;

    public GameObject itemUIPrefab;


    [Header("Расположение элементов (контент панели для спавна)")]
    public Transform inventoryContentPanel;
    public GridLayoutGroup inventoryLayoutGroup;

    public Transform mergeContentPanel;
    public GridLayoutGroup mergeLayoutGroup;

    public Transform mergeResultsContentPanel;
    public GridLayoutGroup mergeResultsLayoutGroup;

    [Header("Кнопки")]

    public Transform mergeButton;


    [SerializeField] private List<InventoryItem> tempMergeItems;

    [SerializeField] private ItemRarity? itemRarityBlock;
    [SerializeField] public Dictionary<InventoryItem, MergeInventoryItemUI> mergeInventoryItems; // от конфигурации MergeInventoryItemUI
                                                                                                 // зависит то, как будет отображаться предметы

    [SerializeField] private Dictionary<InventoryItem, List<InventoryItem>> mergeResults;

    

    
    

    public void Start()
    {
        inventory = GameManager.inventory;

        mergeButton.GetComponent<Button>().onClick.AddListener(OnMergeButtonClicked);
        Setup();
    }


    public void Setup()
    {
        mergeResults = new Dictionary<InventoryItem, List<InventoryItem>>();
        tempMergeItems = new List<InventoryItem>();
        mergeInventoryItems = new Dictionary<InventoryItem, MergeInventoryItemUI>();
        mergeInventoryItems.Clear();
        ClearBlackSmithContentPanels();
        foreach (InventoryItem item in inventory.items)
        {
            if (item.itemRarity != ItemRarity.Legendary)
            {
                    SpawnItem(item, inventoryContentPanel, OnInventoryItemClicked);
                    MergeInventoryItemUI mergeInventoryItemUI = new MergeInventoryItemUI(ItemUIState.isInInventory);
                    mergeInventoryItems.Add(item, mergeInventoryItemUI);
            }
        }

    }

    public void FullBlacksmithRefresh()
    {
        ClearBlackSmithContentPanels();
        foreach (KeyValuePair<InventoryItem, MergeInventoryItemUI> mergeInventoryItem in mergeInventoryItems)
        {
            if (itemRarityBlock == null || mergeInventoryItem.Key.itemRarity == itemRarityBlock)
            {
                switch (mergeInventoryItem.Value.itemUIState)
                {
                    case ItemUIState.isInInventory:

                        SpawnItem(mergeInventoryItem.Key, inventoryContentPanel, OnInventoryItemClicked);
                        break;
                    case ItemUIState.isInMerge:

                        SpawnItem(mergeInventoryItem.Key, inventoryContentPanel, OnInventoryItemClicked, true);
                        SpawnItem(mergeInventoryItem.Key, mergeContentPanel, OnMergeItemClicked);
                        break;
                    case ItemUIState.isUsedInMergeResult:

                        SpawnItem(mergeInventoryItem.Key, inventoryContentPanel, OnInventoryItemClicked, true);
                        break;
                }
            }
            if (mergeInventoryItem.Value.itemUIState == ItemUIState.isResult)
            {
                SpawnItem(mergeInventoryItem.Key, mergeResultsContentPanel, OnResultItemClicked);
            }

        }

    }

    public void BlockElementsByRarity(InventoryItem item)
    {
        itemRarityBlock = item.itemRarity;

    }

    public void UnBlockElementsByRarity()
    {
        itemRarityBlock = null;
    }

    public void MoveItemToMergeSlots(ItemUI itemUI)
    {
        if (tempMergeItems.Count == 3)
        {
            foreach (InventoryItem item in tempMergeItems)
            {
                mergeInventoryItems[item].itemUIState = ItemUIState.isUsedInMergeResult;
            }
            tempMergeItems.Clear();
        }

        tempMergeItems.Add(itemUI.item);

        mergeInventoryItems[itemUI.item].itemUIState = ItemUIState.isInMerge;
        if (tempMergeItems.Count == 1)
        {
            BlockElementsByRarity(itemUI.item);
        }

        if (tempMergeItems.Count == 3)
        {
            GenerateMergeResult(tempMergeItems);
            UnBlockElementsByRarity();  
        }
    }

    public void RemoveitemFromMergeSlots(ItemUI itemUI)
    {
        if (mergeInventoryItems[itemUI.item].itemUIState == ItemUIState.isUsedInMergeResult)
        {
            InventoryItem mergeResult = mergeInventoryItems[itemUI.item].mergeResult;
            foreach (InventoryItem item in mergeResults[mergeResult])
            {
                mergeInventoryItems[item].itemUIState = ItemUIState.isInInventory;
                mergeInventoryItems[item].mergeResult = null;
                Debug.Log(item.item.itemName);
            }
            mergeResults.Remove(mergeResult);
            mergeInventoryItems.Remove(mergeResult);

        }
        else if (mergeInventoryItems[itemUI.item].itemUIState == ItemUIState.isInMerge)
        {
            if (mergeInventoryItems[itemUI.item].mergeResult != null)
            {

                InventoryItem mergeResult = mergeInventoryItems[itemUI.item].mergeResult;

                foreach (InventoryItem item in tempMergeItems)
                {
                    mergeInventoryItems[item].mergeResult = null;
                }
                mergeResults.Remove(mergeResult);
                mergeInventoryItems.Remove(mergeResult);
            }
            tempMergeItems.Remove(itemUI.item);
            mergeInventoryItems[itemUI.item].itemUIState = ItemUIState.isInInventory;
        }
        
        if ( tempMergeItems.Count == 0 )
        {
            UnBlockElementsByRarity();
        }
    }

    public void OnInventoryItemClicked(ItemUI itemUI)
    {
        if (mergeInventoryItems[itemUI.item].itemUIState == ItemUIState.isInInventory)
        {
            MoveItemToMergeSlots(itemUI);
        }
        else if (mergeInventoryItems[itemUI.item].itemUIState == ItemUIState.isInMerge 
            || mergeInventoryItems[itemUI.item].itemUIState == ItemUIState.isUsedInMergeResult)
        {
            RemoveitemFromMergeSlots(itemUI);
        }
        FullBlacksmithRefresh();
    }

    public void OnMergeItemClicked(ItemUI itemUI)
    {
        RemoveitemFromMergeSlots(itemUI);
        FullBlacksmithRefresh();
    }

    public void OnResultItemClicked(ItemUI itemUI)
    {

        foreach (InventoryItem item in mergeResults[itemUI.item]) 
        {
            if (mergeInventoryItems[item].itemUIState == ItemUIState.isInMerge)
            {
                tempMergeItems.Remove(item);
            }
            mergeInventoryItems[item].itemUIState = ItemUIState.isInInventory;
            mergeInventoryItems[item].mergeResult = null;
        }
        
        mergeResults.Remove(itemUI.item);
        mergeInventoryItems.Remove(itemUI.item);
        FullBlacksmithRefresh();
    }


    public void OnMergeButtonClicked()
    {
        foreach (KeyValuePair<InventoryItem, List<InventoryItem>> mergeResult in mergeResults)
        {
            foreach (InventoryItem item in mergeResult.Value)
            {
                inventory.RemoveItemWithoutSave(item);
            }
            inventory.AddItemWithoutSave(mergeResult.Key);
        }

        inventory.SortByRarityFromHighest();
        
        Setup();
    }

    public void SpawnItem(InventoryItem inventoryItem, Transform contentPanel, UnityAction<ItemUI> OnItemClickedFunction, bool isSelected = false)
    {
        GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
        ItemUI newItemUI = newItem.GetComponent<ItemUI>();
        newItemUI.Setup(inventoryItem);
        newItemUI.OnItemUIClicked.AddListener(OnItemClickedFunction);
        if (isSelected)
        {
            SetSelectedColor(newItemUI);
        }
        
        // сюда добавить подписку на событие
    }

    public void ClearBlackSmithContentPanels()
    {
        ClearMergeSlots();
        ClearResultSlots();
        ClearInventoorySlots();
    }

    public void ClearMergeSlots()
    {
        foreach (Transform child in mergeContentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearResultSlots()
    {
        foreach (Transform child in mergeResultsContentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearInventoorySlots()
    {
        foreach (Transform child in inventoryContentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateMergeResult(List<InventoryItem> items)
    {
        ItemRarity temp = items[0].itemRarity;
        bool allRaritiesSuccess = true;
        foreach (InventoryItem item in items)
        {
            if (item.itemRarity != temp)
            {
                allRaritiesSuccess = false;
            }
        }
        if (allRaritiesSuccess)
        {
            Dictionary<ItemType, int> itemType = new Dictionary<ItemType, int>();
            Dictionary<BonusType, int> bonusType = new Dictionary<BonusType, int>();

            float bonusValueSum = 0f;
            BonusType bonusTypeResult = new BonusType();
            ItemType itemTypeResult = new ItemType();
            foreach (InventoryItem item in items)
            {
                bonusValueSum += item.bonusValue;
                if (itemType.ContainsKey(item.ItemType))
                {
                    itemType[item.ItemType]++;
                }
                else
                {
                    itemType[item.ItemType] = 1;
                }

                if (bonusType.ContainsKey(item.BonusType))
                {
                    bonusType[item.BonusType]++;
                }
                else
                {
                    bonusType[item.BonusType] = 1;
                }
            }

            double randValue = (((bonusValueSum * 3123 + 1234) / 12) * 3) % 100; // случайное число от 0 до 100

            double cumulative = 0.0;

            foreach (var kvp in itemType)
            {
                cumulative += kvp.Value * 33;
                if (randValue < cumulative)
                {
                    itemTypeResult = kvp.Key;

                    break;
                }
            }
            cumulative = 0.0;
            foreach (var kvp in bonusType)
            {
                cumulative += kvp.Value * 33;

                if (randValue < cumulative)
                {
                    bonusTypeResult = kvp.Key;
                    break;
                }
            }
            bonusValueSum /= items.Count;
            randValue = (((bonusValueSum * 3123 + 1234) / 12) * 3) % 51 + 50;

            TemplateItem templateItem = null;

            foreach (TemplateItem item in GameManager.lootManager.possibleLoot)
            {
                if (item.bonusType == bonusTypeResult && item.itemType == itemTypeResult)
                {
                    templateItem = item;
                }
            }

            if (templateItem != null)
            {
                InventoryItem inventoryItem = new InventoryItem(templateItem, LootManager.GenerateBonusValueByRarity((ItemRarity)(((int)temp) + 1), (int)randValue), (ItemRarity)(((int)temp) + 1));
                inventory.ChooseItemBackGroundImage(inventoryItem);

                mergeInventoryItems.Add(inventoryItem, new MergeInventoryItemUI(ItemUIState.isResult));
                List<InventoryItem> tempList = new List<InventoryItem>(tempMergeItems);
                mergeResults.Add(inventoryItem, tempList);
                foreach (InventoryItem item in mergeResults[inventoryItem])
                {
                    mergeInventoryItems[item].mergeResult = inventoryItem;
                }
                
                
            }
        }


    }

    public void BlackSmithDestroy()
    {
        Destroy(gameObject);
    }

    public void SetSelectedColor(ItemUI item)
    {

        item.itemIcon.color = Color.gray;
        item.itemRarityBackground.color = Color.gray;
        item.itemName.color = Color.gray;
    }

    public void SetUnselectedColor(ItemUI item)
    {

        item.itemIcon.color = Color.white;
        item.itemRarityBackground.color = Color.white;
        item.itemName.color = Color.white;
    }
}