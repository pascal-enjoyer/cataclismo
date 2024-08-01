
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
        RefreshImage();
    }

    public void RefreshImage()
    {
        if (element != null)
        {
            transform.GetComponent<Image>().sprite = element.icon;
        }
        else
            transform.GetComponent<Image>().sprite = null;
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
