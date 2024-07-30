
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementInBar : MonoBehaviour
{
    [SerializeField] private Element element;

    public ElementInBar(Element element)
    {
        this.element = element;
    }

    public void Start()
    {
        if (element != null)
        {
            RefreshImage();
        }
    }

    public void RefreshImage()
    {
        transform.GetComponent<Image>().sprite = element.icon;
    }

    public Element GetElement()
    {
        return element;
    }


    public void SetElement(Element element)
    {
        this.element = element;
    }
}
