using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dancer", menuName = "Dancer", order = 51)]
public class DancerSO : ScriptableObject
{
    public string dancerName;
    public int height;
    public Sex dancerSex;
    public long dickLenght;
    public GameObject dancerTexture;

    public enum Sex
    {
        man,
        woman
    }
}
