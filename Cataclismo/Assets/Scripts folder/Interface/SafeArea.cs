using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public RectTransform screenFinalSize;
    public float safeAreaHeight;
    public float safeAreaWidth;

    private void Start()
    {
        screenFinalSize = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        screenFinalSize.anchorMin = anchorMin;
        screenFinalSize.anchorMax = anchorMax;
    }

}
