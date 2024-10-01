using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    private InventoryItem item;
    public Text itemName;
    public Text itemDescription;

    public ItemUI itemUI;

    public Text itemBonus;
    public BonusType itemBonusType;
    public Text upgradePrice;

    public Text itemLevel;
    public Image itemIcon;
    public Text equipButtonText;
    public Transform upgradeButton;

    public Transform equipButton;

    public InventoryUI inventoryUI;
        



    public void FillWindow(InventoryItem tempItem)
    {
        item = tempItem;
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
        itemIcon.sprite = item.ItemIcon;/*
        if (itemUI != null && itemUI.isResult)
        {
            upgradeButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }*/
        equipButtonText.text = item.isEquiped ? "Take off" : "Equip";
        itemBonus.text = item.BonusType.ToString() + " + " + item.bonusValue.ToString();
        itemLevel.text = "Item level " + item.itemLevel.ToString();
        upgradePrice.text = (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel).ToString() + " coins";
        if (GameManager.playerEconomic.coins < (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel) || item.itemLevel >= item.maxItemLevel)
        {
            upgradeButton.GetComponent<Button>().interactable = false;
            upgradeButton.GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void RefreshUI()
    {
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
        itemIcon.sprite = item.ItemIcon; /*
        if (itemUI != null && itemUI.isResult)
        {
            upgradeButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }*/
        equipButtonText.text = item.isEquiped ? "Take off" : "Equip";
        itemBonus.text = item.BonusType.ToString() + " + " + item.bonusValue.ToString();
        itemLevel.text = "Item level " + item.itemLevel.ToString();
        upgradePrice.text = (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel).ToString() + " coins";
        if (GameManager.playerEconomic.coins < (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel) || item.itemLevel >= item.maxItemLevel)
        {
            upgradeButton.GetComponent<Button>().interactable = false;
            upgradeButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void CloseInfoWindow()
    {
        Destroy(gameObject);
    }

    public void OnEquipButtonClicked()
    {

        switch (item.isEquiped)
        {
            case true:
                inventoryUI.TakeOffItem(item);
                break;
            case false:
                inventoryUI.EquipItem(item);
                break;
        }
        CloseInfoWindow();
    }


    public void OnUpgradeButtonClicked()
    {// тут какая то хуйня
        if (GameManager.playerEconomic.coins >= item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel - 1 && (item.itemLevel < item.maxItemLevel))
        {
            GameManager.playerEconomic.coins -= item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel;
            GameManager.playerEconomic.OnPlayerEconomicLoaded.Invoke();
            GameManager.playerEconomic.OnPlayerEconomicChanged.Invoke();
            GameManager.inventory.UpgradeItemLevel(item);
            inventoryUI.RefreshInventoryUI();
            RefreshUI();
        }
    }
}
