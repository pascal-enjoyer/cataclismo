using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell", order = 51)]
public class Spell : ScriptableObject
{
    public string spellName;
    public Element[] requiredElements;
    public string spellDescription;
    //���� ��� ������ �������� ��� �������� ����� �� 150% �����, ���� ����� �� ��� ����, �� 25% ����� � �.�
    public SpellType spellType;
    public bool isShield;
    public int spellDamage;

    public float elementalStormBoost;

}
public enum SpellType
{
    Water,
    Fire,
    Earth,
    unique
}
