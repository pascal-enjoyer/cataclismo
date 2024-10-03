using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;   



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


    [SerializeField] private List<InventoryItem> inventoryItems;


    [SerializeField] private List<ItemUI> selectedUIItems;


    [SerializeField] private Dictionary<ItemUI, List<ItemUI>> mergeResults; 
    [SerializeField] private List<ItemUI> tempMergeItems;



    public UnityEvent OnMergeItemsChanged;
    public UnityEvent OnResultItemsChanged;
    
    

    public void Start()
    {
        inventory = GameManager.inventory;
        tempMergeItems = new List<ItemUI>();
        mergeResults = new Dictionary<ItemUI, List<ItemUI>>();
        SpawnAllItemsUI();

        mergeButton.GetComponent<Button>().onClick.AddListener(OnMergeButtonClicked);
    }

    public void SpawnAllItemsUI()
    {
        inventoryItems = inventory.items;

        if (inventoryContentPanel.childCount > 0)
        {
            foreach (Transform item in inventoryContentPanel)
            {
                if (!selectedUIItems.Contains(item.GetComponent<ItemUI>()))
                    Destroy(item.gameObject);
            }
        }
        if (mergeContentPanel.childCount > 0)
        {
            foreach (Transform item in mergeContentPanel)
            {
                Destroy(item.gameObject);
            }
            tempMergeItems.Clear();
        }
        if (mergeResultsContentPanel.childCount > 0)
        {
            foreach (Transform item in mergeResultsContentPanel)
            {
                Destroy(item.gameObject);
            }
            mergeResults.Clear();
        }
        foreach (InventoryItem item in inventoryItems)
        {

            if (item.itemRarity != ItemRarity.Legendary)
            {
                GameObject newItem = Instantiate(itemUIPrefab, inventoryContentPanel);

                ItemUI itemUI = newItem.GetComponent<ItemUI>();
                itemUI.Setup(item);


                itemUI.OnItemUIClicked.RemoveAllListeners();
                itemUI.OnItemUIClicked.AddListener(OnMergeInventoryItemClicked);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContentPanel.GetComponent<RectTransform>());

        Debug.Log("All items spawned");
    }

    /*    public void DestroyAllInventoryItems()
        {
            foreach (Transform item in inventoryContentPanel)
            {
                Destroy(item.gameObject);
            }
            foreach(KeyValuePair<ItemUI, List<ItemUI>> mergeItems in mergeResults) 
            {
                foreach (ItemUI item in mergeItems.Value)
                {
                    Destroy(item.gameObject);
                }
                Destroy(mergeItems.Key.gameObject);
            }
            foreach (ItemUI itemUI in tempMergeItems)
            {
                Destroy(itemUI.gameObject);
            }
            mergeResults.Clear();
            tempMergeItems.Clear();
            selectedUIItems.Clear();
        }*/

    public void RefreshMergeInventoryUI()
    {
        if (inventoryContentPanel.childCount > 0)
        {
            foreach (Transform item in inventoryContentPanel)
            {
                if (!selectedUIItems.Contains(item.GetComponent<ItemUI>()))
                    Destroy(item.gameObject);
            }
        }
        foreach (InventoryItem item in inventoryItems)
        {
            
            if (item.itemRarity != ItemRarity.Legendary)
            {
                bool needsToBySpawned = true;
                foreach(ItemUI itemui in selectedUIItems)
                {
                    if (itemui.item == item)
                    {
                        needsToBySpawned = false;
                        break;
                    }
                }
                if (needsToBySpawned)
                {
                    GameObject newItem = Instantiate(itemUIPrefab, inventoryContentPanel);

                    ItemUI itemUI = newItem.GetComponent<ItemUI>();
                    itemUI.Setup(item);

                    itemUI.OnItemUIClicked.RemoveAllListeners();
                    itemUI.OnItemUIClicked.AddListener(OnMergeInventoryItemClicked);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContentPanel.GetComponent<RectTransform>());

        Debug.Log("Merge inventory refreshed");
    }

    public void RefreshMergeResultSlots()
    {

        Debug.Log($"Merge result slots refreshed");
    }

    public void BlockElementsByRarity()
    {
        ItemRarity rarityBlock;
        rarityBlock = tempMergeItems[0].item.itemRarity;

        foreach (Transform child in inventoryContentPanel)
        {
            if (child.GetComponent<ItemUI>().item.itemRarity != rarityBlock)
            {
                Destroy(child.gameObject);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContentPanel.GetComponent<RectTransform>());
    }

    public void MoveItemToMerge(ItemUI item)
    {
        if (tempMergeItems.Count == 3)
        {
            foreach (Transform itemui in mergeContentPanel)
            {
                Destroy(itemui.gameObject);
            }
            tempMergeItems.Clear();
        }

        ItemUI tempItemUI = Instantiate(itemUIPrefab, mergeContentPanel).GetComponent<ItemUI>();
        tempItemUI.Setup(item);
        tempItemUI.OnItemUIClicked.RemoveAllListeners();
        tempItemUI.OnItemUIClicked.AddListener(OnMergeItemClicked);
        tempMergeItems.Add(tempItemUI);

        SelectItem(item);
        if (tempMergeItems.Count == 1)
        {
            BlockElementsByRarity();
        }

        

        if (tempMergeItems.Count == 3)
        {

            GenerateMergeResult(tempMergeItems);
            RefreshMergeInventoryUI();
        }

        OnMergeItemsChanged.Invoke();
        Debug.Log($"Item {item.item.ItemName} moved in merge");
    }

    public void RemoveItemFromMerge(ItemUI item)
    {
        if (selectedUIItems.Contains(item))
        {
            foreach (ItemUI itemUI in tempMergeItems)
            {
                if (itemUI.item == item.item)
                {
                    tempMergeItems.Remove(itemUI);
                    if (tempMergeItems.Count == 0)
                        RefreshMergeInventoryUI();
                    UnSelectItem(item);
                    OnMergeItemsChanged.Invoke();
                    Destroy(itemUI.gameObject);
                    break;
                }
            }
        }
        else
        {
            foreach (ItemUI itemUI in selectedUIItems)
            {
                if (itemUI.item == item.item)
                {
                    tempMergeItems.Remove(item);
                    if (tempMergeItems.Count == 0)
                        RefreshMergeInventoryUI();
                    UnSelectItem(itemUI);
                    OnMergeItemsChanged.Invoke();
                    Destroy(item.gameObject);
                    break;
                }
            }
        }
    }

    public void AddItemsToResultDictionary(ItemUI resultItem, List<ItemUI> tempMergeItems)
    {
        mergeResults.Add(resultItem, tempMergeItems);
        OnResultItemsChanged.Invoke();
        OnMergeItemsChanged.Invoke();

        Debug.Log($"Item {resultItem.item.ItemName} and items from merge added to Dictionary");
    }

    public void RemoveItemFromResult(ItemUI item)
    {
        mergeResults.Remove(item);
        OnResultItemsChanged.Invoke();

        Debug.Log($"Item {item.item.ItemName} removed from result");
    }

    public void OnMergeInventoryItemClicked(ItemUI item)
    {
        if (!selectedUIItems.Contains(item))
        {
            MoveItemToMerge(item);
        }
        else 
            RemoveItemFromMerge(item);

        Debug.Log($"Merge inventory item clicked");
    }


    public void OnMergeItemClicked(ItemUI item)
    {

        RemoveItemFromMerge(item);
        Debug.Log($"Merge item item clicked");
    }

    public void OnResultItemClicked(ItemUI item)
    {
        RemoveItemFromResult(item);
        Debug.Log($"Merge result item clicked");
    }

    public void OnMergeButtonClicked()
    {
        foreach (KeyValuePair<ItemUI, List<ItemUI>> mergeResult in mergeResults)
        {
            foreach (ItemUI item in mergeResult.Value)
            {
                inventory.RemoveItem(item.item);
            }
            inventory.AddItem(mergeResult.Key.item);
        }
        SpawnAllItemsUI();
        
    }

    public void UnSelectItem(ItemUI item)
    {
        selectedUIItems.Remove(item);

        SetUnselectedColor(item);
        Debug.Log($"Item {item.item.ItemName} unselected");
             
    }

    public void SelectItem(ItemUI item)
    {
        selectedUIItems.Add(item);

        SetSelectedColor(item);
        Debug.Log($"Item {item.item.ItemName} selected");
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



    public void GenerateMergeResult(List<ItemUI> mergeItems)
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach (ItemUI item in mergeItems)
        {
            items.Add(item.item);
        }
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
                    Debug.Log("itemTypeSuccess");

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
                    Debug.Log("BonusTypeSuccess");
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



                ItemUI itemUI = Instantiate(itemUIPrefab, mergeResultsContentPanel).GetComponent<ItemUI>();
                itemUI.Setup(inventoryItem);

                mergeResults.Add(itemUI, tempMergeItems);

            }
        }


    }

    public void BlackSmithDestroy()
    {
        Destroy(gameObject);
    }
    /*

        public void RefreshInventoryUI()
        {
            // Очистите старые элементы
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }
            // Добавьте новые элементы
            foreach (InventoryItem item in inventory.items)
            {

                if (item.itemRarity != ItemRarity.Legendary)
                {

                        GameObject newItem = Instantiate(itemUIPrefab, contentPanel);
                        ItemUI itemUI = newItem.GetComponent<ItemUI>();
                        itemUI.inventoryParent = transform;
                        itemUI.Setup(item);
                    itemUI.InventoryUI = inventoryUI; 
                    itemUI.OnItemUIClicked.RemoveAllListeners();
                    itemUI.OnItemUIClicked.AddListener(OnMergeItemClicked);

                }
            }
            // Обновите размер Content, чтобы подстроиться под количество элементов
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
            Debug.Log("inventoryRefreshded");
        }*/

    //чел добавил 3 элемента
    //добавляет следующий и эти три не должны отображаться
    //обновляется полностью кроме них инвепнтарь
    //результат уходит в список


    /*
        public void BlockElementsByRarity()
        {
            ItemRarity rarityBlock;
            rarityBlock = mergeItems[0].itemRarity;

            foreach (Transform child in contentPanel)
            {
                if (child.GetComponent<ItemUI>().item.itemRarity != rarityBlock)
                {
                    Destroy(child.gameObject);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
        }

        public void RefreshMergeSlots()
        {
            foreach (Transform child in mergeSlotsContentPanel)
            {
                Destroy(child.gameObject);
            }

            if (mergeResultSlot.childCount > 0)
            {
                Destroy(mergeResultSlot.GetChild(0).gameObject);
            }

            if (mergeResult != null)
            {
                GameObject newItem = Instantiate(itemUIPrefab, mergeResultSlot);
                ItemUI itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.Setup(mergeResult);
                itemUI.InventoryUI = inventoryUI;

            }
            foreach (InventoryItem item in mergeItems)
            {

                GameObject newItem = Instantiate(itemUIPrefab, mergeSlotsContentPanel);
                ItemUI itemUI = newItem.GetComponent<ItemUI>();
                itemUI.inventoryParent = transform;
                itemUI.Setup(item);
                itemUI.InventoryUI = inventoryUI;
                itemUI.OnItemUIClicked.RemoveAllListeners();
                itemUI.OnItemUIClicked.AddListener(OnMergeItemClicked); // где то два раза привязка
            }

            foreach(ItemUI item in SelectedItems)
            {
                if (!mergeItems.Contains(item.item))
                {
                    UnSelectItem(item);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(mergeSlotsContentPanel.GetComponent<RectTransform>());
        }


        public void MoveItemToMergeSlots(ItemUI item)
        {
            if (mergeItems.Count < 3 && !mergeItems.Contains(item.item))
            {
                mergeItems.Add(item.item);
                if (mergeItems.Count == 3)
                {
                    GenerateMergeResult(mergeItems);
                }

                if (mergeItems.Count == 1)
                {
                    BlockElementsByRarity();
                }
                SelectItem(item);
                OnMergeItemsChanged.Invoke();

            }


        }
        public void ExecuteItemFromMergeSlots(ItemUI item)
        {
            if (mergeItems.Remove(item.item))
            {
                UnSelectItem(item);

                OnMergeItemsChanged.Invoke();

                // Если больше нет элементов, обновляем весь инвентарь
                if (mergeItems.Count == 0)
                {
                    RefreshInventoryUI();
                }
                else if (mergeItems.Count == 2)
                {
                    mergeResult = null;
                    RefreshMergeSlots();
                }
            }
        }


        public void OnMergeButtonClicked()
        {
            if (mergeItems.Count == 0)
            {

            }
            else if (mergeResult != null)
            {
                inventory.AddItem(mergeResult);

                mergeResult =null;
                foreach (InventoryItem item in mergeItems)
                {
                    inventory.RemoveItem(item);
                }
                mergeItems.Clear();

                OnResultItemAddedToInventory();
            }
        }

        public void OnResultItemAddedToInventory()
        {
            Destroy(mergeResultSlot.GetChild(0).gameObject);
            RefreshInventoryUI();
            RefreshMergeSlots();
        }

        public void BlackSmithDestroy()
        {
            Destroy(gameObject);
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
            if (allRaritiesSuccess) {
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
                        Debug.Log("itemTypeSuccess");

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
                        Debug.Log("BonusTypeSuccess");
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
                    GameObject newItem = Instantiate(itemUIPrefab, mergeResultSlot);
                    ItemUI itemUI = newItem.GetComponent<ItemUI>();
                    itemUI.inventoryParent = transform;
                    itemUI.Setup(inventoryItem);
                    itemUI.InventoryUI = inventoryUI;
                    mergeResult = inventoryItem;
                }
            }


        }


        public void OnMergeItemClicked(ItemUI item)
        {
            if (!mergeItems.Contains(item.item))
            {

                MoveItemToMergeSlots(item);
                Debug.Log("da");
            }
            else
            {
                Debug.Log("net");
                ExecuteItemFromMergeSlots(item);
            }
        }

        public void UnSelectItem(ItemUI item)
        {
            SelectedItems.Remove(item);
            item.itemIcon.color = Color.white;
            item.itemRarityBackground.color = Color.white;
            item.itemName.color = Color.white;

        }

        public void SelectItem(ItemUI item)
        {
            SelectedItems.Add(item);
            item.itemIcon.color = Color.gray;
            item.itemRarityBackground.color = Color.gray;
            item.itemName.color = Color.gray;
        }*/
}