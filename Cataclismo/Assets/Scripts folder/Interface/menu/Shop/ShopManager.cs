using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject chestPrefab;
    public List<Chest> chestList;
    public Transform contentPanel;
    public Transform canvasSlot;
    public GameObject dropFromChestPanel;

    public void Start()
    {
        foreach (Chest chest in chestList)
        {
            GameObject chestGJ = Instantiate(chestPrefab, contentPanel);
            chestGJ.GetComponent<ChestInShop>().SetupChests(chest);
            chestGJ.GetComponent<ChestInShop>().OnItemDropped.AddListener(SpawnChestDropInfo);
        }
    }

    public void SpawnChestDropInfo(InventoryItem item)
    {
        GameObject tempPanel = Instantiate(dropFromChestPanel, canvasSlot);
        tempPanel.GetComponent<DropFromChestWindow>().Setup(item);
    }
    

}
