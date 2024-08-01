using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectingElement : MonoBehaviour
{
    [SerializeField] private List<Transform> elements = new List<Transform>();

    public ActiveElements activeElementsPanel;

    private void Start()
    {
        RefreshElements();
    }

    public void RefreshElements()
    {
        elements.Clear();
        foreach (Transform t in transform)
        {
            elements.Add(t);
            t.GetComponent<Button>().onClick.AddListener(delegate { OnElementClicked(t); });
        }
    }

    public void OnElementClicked(Transform element)
    {
        activeElementsPanel.AddElementToActiveElements(element.GetComponent<ElementInBar>());
    }

    
}
