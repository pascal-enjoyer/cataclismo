using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Template", menuName = "Item Template", order = 51)]
public class TemplateItem : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    public ItemType itemType;
    public BonusType bonusType;
    public GameObject item3dPrefab;
    public Sprite itemOnHandSprite;
    // потом если захотим сделать изменение скина от качетсва добавить поле skinType (1,2,3)
}

public enum ItemType
{
    Ring,
    Bracelet,
    Glove
}

public enum BonusType
{
    Attack,
    Defense
}

