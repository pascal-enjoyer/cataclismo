using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placesForRings : MonoBehaviour
{
    public Transform placeForRing;
    public GameObject ringPrefab;
   

    public void SpawnRing()
    {
        Instantiate(ringPrefab, placeForRing);
    }
}
