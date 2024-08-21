using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placesForRings : MonoBehaviour
{
    public Transform placeForRing;
    public GameObject ringPrefab;

    public Transform placeForBracelet;
    public GameObject braceletPrefab;

    public Transform placeForGlove;
    public HandsAnimationController controller;
    public GameObject glovePrefab;

    public void Start()
    {
        SpawnRing();
    }

    public void SpawnRing()
    {
        Instantiate(ringPrefab, placeForRing);
        Instantiate(braceletPrefab, placeForBracelet);

        controller.SetRightHandGlove(Instantiate(glovePrefab, placeForGlove));
        
    }
}
