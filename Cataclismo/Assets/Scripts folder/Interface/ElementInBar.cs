
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

            Image tempImg = transform.GetComponent<Image>();

            var tmpClr = tempImg.color;
            tmpClr.a = 255f;
            tempImg.color = tmpClr;
            tempImg.sprite = element.icon;

        }
        else
        {

            Image tempImg = transform.GetComponent<Image>();
            tempImg.sprite = null;
            var tmpClr = tempImg.color;
            tmpClr.a = 0f;
            tempImg.color = tmpClr;
        }
            
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
