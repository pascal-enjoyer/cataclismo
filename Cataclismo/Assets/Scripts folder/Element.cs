using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Element", order = 51)]

public class Element : ScriptableObject
{
    [SerializeField] public string elementName;
    [SerializeField] public int elementId;
    [SerializeField] public Sprite icon;
}
