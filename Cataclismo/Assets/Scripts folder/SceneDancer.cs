using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SceneDancer : MonoBehaviour
{
    public List<Dancer> dancers;
    public List<DancerSO> dancerSOs;
    Dancer dancer;
    Dancer dancer2;

    //примерно так но это хуйня
    private void Start()
    {
        GameObject spawnedDancer = Instantiate(dancerSOs[0].dancerTexture);
        spawnedDancer.GetComponent<Dancer>().SetupDancer(this, dancerSOs[0]);


        GameObject spawnedDancer2 = Instantiate(dancerSOs[1].dancerTexture);
        spawnedDancer.GetComponent<Dancer>().SetupDancer(this, dancerSOs[1]);

    }
}
