using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActiveElements : MonoBehaviour
{
    [SerializeField] private List<ElementInBar> activeElements;
    [SerializeField] private List<Transform> activeSlots;

    private void Start()
    {
        RefreshSlots();
    }

    private void RefreshSlots()
    {
        activeSlots.Clear();
        foreach (Transform slot in transform)
        {
            activeSlots.Add(slot);
        }
    }

    private void AddElementInBar(Element element)
    {
        foreach (Transform slot in activeSlots)
        {
            ElementInBar elementInBar = slot.GetComponent<ElementInBar>();
            if (elementInBar.GetElement() == null) 
            {
                elementInBar.SetElement(element);
                elementInBar.RefreshImage();
                break;
            }
        }
    }

    public void AddElementToActiveElements(ElementInBar element)
    {
        activeElements.Add(element);
        AddElementInBar(element.GetElement());



    }
}
