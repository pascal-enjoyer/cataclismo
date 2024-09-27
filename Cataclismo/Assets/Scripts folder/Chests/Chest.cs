using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest", order = 51)]
public class Chest : ScriptableObject
{
    
    public Sprite chestIcon;
    public string chestName;
    public string chestDescription;
    public int chestCost;
    public Sprite chestBackground;

    public List<RarityProbability> rarityProbabilitiesList;

}

[System.Serializable]
public class RarityProbability
{
    public float probability;
    public ItemRarity rarity;
}

