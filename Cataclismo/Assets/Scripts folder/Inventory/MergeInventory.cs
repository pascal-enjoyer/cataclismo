using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;   

public class MergeInventory : MonoBehaviour
{
    

    public Inventory inventory;
    public GameObject itemUIPrefab; // Префаб ячейки предмета
    public Transform contentPanel; // Панель для добавления элементов
    public GridLayoutGroup gridLayoutGroup; // Компонент Grid Layout Group

    public InventoryUI inventoryUI;

    public Transform mergeSlotsContentPanel;
    public GridLayoutGroup mergeSlots;

    public Transform mergeResultSlot;

    public List<InventoryItem> inventoryItems;

    public List<InventoryItem> mergeItems;
    public Dictionary<InventoryItem, List<InventoryItem>> mergeResults; //до того как соберутся 3 элемента в один список,
    public InventoryItem mergeResult;                              //потом этот список по получившемуся айтему в другой список

    public UnityEvent OnMergeItemsChanged;
    

    public Transform mergeButton;
    

    public void Start()
    {
        inventory = GameManager.inventory;
        mergeItems = new List<InventoryItem>();
        RefreshInventoryUI();
        mergeButton.GetComponent<Button>().onClick.AddListener(OnMergeButtonClicked);
        
        OnMergeItemsChanged.AddListener(RefreshMergeSlots);
        
    }

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
                    itemUI.isInMerge = true;
                    itemUI.isTaked = false;
            }
        }
        // Обновите размер Content, чтобы подстроиться под количество элементов
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

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

        if (mergeResultSlot.childCount >0)
        {
            Destroy(mergeResultSlot.GetChild(0).gameObject);
        }
        if (mergeResult != null)
        {
            GameObject newItem = Instantiate(itemUIPrefab, mergeResultSlot);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(mergeResult);
            itemUI.isInMerge = true;
            itemUI.isTaked = false;
            itemUI.isResult = true;
            itemUI.InventoryUI = inventoryUI;
        }
        foreach (InventoryItem item in mergeItems)
        {
            GameObject newItem = Instantiate(itemUIPrefab, mergeSlotsContentPanel);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.inventoryParent = transform;
            itemUI.Setup(item);
            itemUI.isInMerge = true;
            itemUI.isTaked = true;
        }
        

        LayoutRebuilder.ForceRebuildLayoutImmediate(mergeSlotsContentPanel.GetComponent<RectTransform>());
    }


    public void MoveItemToMergeSlots(InventoryItem item)
    {

        if (mergeItems.Count < 3)
        {
            mergeItems.Add(item);
        }
        if (mergeItems.Count == 3)
        {
            GenerateMergeResult(mergeItems);
        }

        if (mergeItems.Count == 1)
        {
            BlockElementsByRarity();
        }
        OnMergeItemsChanged.Invoke();

    }

    public void SoftRefresh()
    {
        foreach (Transform child in contentPanel)
        {
            if (child.GetComponent<ItemUI>() != null) 
            {
                
            }
        }
    }
    
        public void ExecuteItemFromMergeSlots(InventoryItem item)
    {
        foreach (Transform itemUI in contentPanel)
        {
            if (itemUI.GetComponent<ItemUI>().item == item)
            {
                ItemUI temp = itemUI.GetComponent<ItemUI>();
                
                temp.itemIcon.color = Color.white;
                temp.itemRarityBackground.color = Color.white;
                temp.itemName.color = Color.white;
                break;
            }
        }
        mergeItems.Remove(item);

        if (mergeItems.Count == 0)
        {
            RefreshInventoryUI();
        }
        if (mergeItems.Count == 2)
        {
            mergeResult = null;
            RefreshMergeSlots();
        }
        OnMergeItemsChanged.Invoke();
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
                itemUI.isInMerge = true;
                itemUI.isTaked = false;
                itemUI.isResult = true;
                itemUI.InventoryUI = inventoryUI;
                mergeResult = inventoryItem;
            }
        }

    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Пример ввода предметов
        string[] items = { "Кольцо", "Перчатка", "Кольцо" };

        // Вызываем метод для расчета вероятностей
        Dictionary<string, double> probabilities = CalculateCraftProbability(items);

        // Выбираем предмет в зависимости от вероятности
        string craftedItem = GetCraftedItem(probabilities);

        // Выводим результат
        Console.WriteLine($"Скрафтился предмет: {craftedItem}");
    }

    static Dictionary<string, double> CalculateCraftProbability(string[] items)
    {
        if (items.Length != 3)
        {
            throw new ArgumentException("Должно быть ровно 3 элемента для крафта.");
        }

        // Подсчитываем количество каждого типа предмета
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        foreach (var item in items)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts[item] = 1;
            }
        }

        // Рассчитываем вероятности
        Dictionary<string, double> probabilities = new Dictionary<string, double>();

        foreach (var kvp in itemCounts)
        {
            probabilities[kvp.Key] = kvp.Value / 3.0;
        }

        return probabilities;
    }

    static string GetCraftedItem(Dictionary<string, double> probabilities)
    {
        // Генератор случайных чисел
        Random random = new Random();
        double randValue = random.NextDouble(); // случайное число от 0.0 до 1.0

        double cumulative = 0.0;

        foreach (var kvp in probabilities)
        {
            cumulative += kvp.Value;

            if (randValue < cumulative)
            {
                return kvp.Key;
            }
        }

        // На случай, если что-то пойдет не так (что маловероятно)
        return probabilities.Keys.First();
    }
}
*/