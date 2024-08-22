using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    private InventoryItem item;
    public Text itemName;
    public Text itemDescription;

    public Text itemBonus;
    public BonusType itemBonusType;
    public Text upgradePrice;

    public Text itemLevel;
    public Image itemIcon;
    public Text equipButtonText;
    public Transform upgradeButton;
    

    public void FillWindow(InventoryItem tempItem)
    {
        item = tempItem;
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
        itemIcon.sprite = item.ItemIcon;
        equipButtonText.text = item.isEquiped ? "Take off" : "Equip"; 
        itemBonus.text = item.BonusType.ToString() + " + " + item.bonusValue.ToString();
        itemLevel.text = "Item level " + item.itemLevel.ToString();
        upgradePrice.text = (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel).ToString() + " coins"; 
        if (GameManager.playerEconomic.coins < (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel))
        {
            upgradeButton.GetComponent<Button>().interactable = false;
            upgradeButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void RefreshUI()
    {
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
        itemIcon.sprite = item.ItemIcon;
        equipButtonText.text = item.isEquiped ? "Take off" : "Equip";
        itemBonus.text = item.BonusType.ToString() + " + " + item.bonusValue.ToString();
        itemLevel.text = "Item level " + item.itemLevel.ToString();
        upgradePrice.text = (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel).ToString() + " coins";
        if (GameManager.playerEconomic.coins < (item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel))
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
                transform.parent.GetComponent<InventoryUI>().TakeOffItem(item);
                break;
            case false:
                transform.parent.GetComponent<InventoryUI>().EquipItem(item);
                break;
        }
        CloseInfoWindow();
    }


    public void OnUpgradeButtonClicked()
    {// тут какая то хуйня
        if (GameManager.playerEconomic.coins >= item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel - 1)
        {
            GameManager.playerEconomic.coins -= item.itemLevelUpgradeCost + item.addedCostOfUpgradePerLevel * item.itemLevel;
            GameManager.playerEconomic.OnPlayerEconomicLoaded.Invoke();

            GameManager.inventory.UpgradeItemLevel(item);
            transform.parent.GetComponent<InventoryUI>().RefreshInventoryUI();
            RefreshUI();
        }
    }
}
