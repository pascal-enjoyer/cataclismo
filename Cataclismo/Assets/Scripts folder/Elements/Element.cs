using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Element", order = 51)]

public class Element : ScriptableObject
{
    public string elementName;
    public int elementId;
    public Sprite icon;
}
