using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public GameObject mergeButton;

    //public Transform mergeResultSlot;

    [SerializeField] private List<InventoryItem> inventoryItems;

    [SerializeField] private List<ItemUI> selectedUIItems;

    //public List<InventoryItem> mergeItems;
    //public List<ItemUI> SelectedItems;

    [SerializeField] private Dictionary<ItemUI, List<ItemUI>> mergeResults; //до того как соберутся 3 элемента в один список,
                                                                            //потом этот список по получившемуся айтему в другой список
                                                                            //public InventoryItem mergeResult
    [SerializeField] private List<ItemUI> tempMergeItems;



    public UnityEvent OnMergeItemsChanged;
    public UnityEvent OnResultItemsChanged;
    
    

    public void Start()
    {
        inventory = GameManager.inventory;

        tempMergeItems = new List<ItemUI>();
        mergeResults = new Dictionary<ItemUI, List<ItemUI>>();
        inventoryItems = inventory.items;

        RefreshMergeInventoryUI();
        OnMergeItemsChanged.AddListener(RefreshMergeSlotsUI);

        /*
        RefreshInventoryUI();
        mergeButton.GetComponent<Button>().onClick.AddListener(OnMergeButtonClicked);
        
        OnMergeItemsChanged.AddListener(RefreshMergeSlots);*/

    }

    public void SpawnAllItemsUI()
    {

    }

    public void RefreshMergeInventoryUI()
    {
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
        
        Debug.Log("Merge inventory refreshed");
    }


    public void RefreshMergeSlotsUI()
    {
        // Очищаем предыдущие элементы из панели слияния
        foreach (Transform child in mergeContentPanel)
        {
            Destroy(child.gameObject);
        }

        // Добавляем текущие предметы из tempMergeItems в панель
        foreach (ItemUI itemUI in tempMergeItems)
        {
            Instantiate(itemUI, mergeContentPanel);
            itemUI.OnItemUIClicked.RemoveAllListeners();
            itemUI.OnItemUIClicked.AddListener(OnMergeItemClicked);
            /*            GameObject newItem = Instantiate(itemUIPrefab, mergeContentPanel);
                        ItemUI newItemUI = newItem.GetComponent<ItemUI>();

                        // Настраиваем UI для предмета
                        newItemUI.Setup(itemUI);

                        // Добавляем обработчик для клика на элемент
                        newItemUI.OnItemUIClicked.RemoveAllListeners();
                        newItemUI.OnItemUIClicked.AddListener(OnMergeItemClicked);*/
        }

        // Обновляем расположение элементов в панели
        LayoutRebuilder.ForceRebuildLayoutImmediate(mergeContentPanel.GetComponent<RectTransform>());

        Debug.Log($"Merge slots refreshed");
    }

    public void RefreshMergeResultSlots()
    {

        Debug.Log($"Merge result slots refreshed");
    }

    public void MoveItemToMerge(ItemUI item)
    {
        tempMergeItems.Add(item);

        SelectItem(item);
        OnMergeItemsChanged.Invoke();
        Debug.Log($"Item {item.item.ItemName} moved in merge");
    }

    public void RemoveItemFromMerge(ItemUI item)
    {
        tempMergeItems.Remove(item);

        UnSelectItem(item);
        OnMergeItemsChanged.Invoke();
        Debug.Log($"Item {item.item.ItemName} removed from merge");
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
        if (selectedUIItems.Contains(item))
        {
            RemoveItemFromMerge(item);
        }
        else
        {
            Debug.Log("No item error");
        }
        Debug.Log($"Merge item item clicked");
    }

    public void OnResultItemClicked(ItemUI item)
    {
        RemoveItemFromResult(item);
        Debug.Log($"Merge result item clicked");
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