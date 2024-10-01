using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : MonoBehaviour
{
    public SceneDancer SceneDancer;
    public DancerSO dancerSO;

    public bool readyToAct = false;

    public void SetupDancer(SceneDancer sceneDancer, DancerSO dancerSO)
    {
        SceneDancer = sceneDancer;
        this.dancerSO = dancerSO;

    }

    public void Act()
    {
        readyToAct = true;
    }

    public void DebugDancerInfo()
    {
        Debug.Log(dancerSO.dancerSex.ToString() + " " + dancerSO.dancerName + " " + dancerSO.height + " " + dancerSO.dickLenght);
    }

}






