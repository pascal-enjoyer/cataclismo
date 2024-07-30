using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ActiveElements : MonoBehaviour
{
    [SerializeField] private List<Element> activeElements;
    [SerializeField] private List<Transform> activeSlots;
    [SerializeField] private List<Spell> spells;

    [SerializeField] private float timeOfSpellCast;

    [SerializeField] private bool isInvokeActive = false;

    private void Start()
    {
        
        RefreshSlots();
        RefreshSpells();
    }

    private void RefreshSpells()
    {
        spells = Resources.LoadAll<Spell>("ScriptableObjects/Spells").ToList();
    }

    private void RefreshSlots()
    {
        activeSlots.Clear();
        foreach (Transform slot in transform)
        {

            activeSlots.Add(slot);
            try
            {
                ElementInBar elementInBar = slot.GetComponent<ElementInBar>();
                elementInBar.SetElement(null);
                elementInBar.RefreshImage();
            }
            catch
            {
                Debug.Log("На элементах нету компоненты ElementInBar");
            }
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
        if (activeElements.Count < 3)
        {
            activeElements.Add(element.GetElement());
            AddElementInBar(element.GetElement());
        }
        if (activeElements.Count >= 3 && !isInvokeActive)
        {
            foreach (Spell spell in spells)
            {
                if (AreElementsMatching(spell.requiredElements, activeElements))
                {
                    UseSpell(spell);
                    
                }
            }
            Invoke(nameof(RefreshActiveElements), 1);
            isInvokeActive = true;
        }


    }

    private bool AreElementsMatching(Element[] requiredElements, List<Element> inputElements)
    {
        if (requiredElements.Length != inputElements.Count)
            return false;

        List<Element> inputCopy = new List<Element>(inputElements);
        foreach (var element in requiredElements)
        {
            if (!inputCopy.Contains(element))
                return false;
            inputCopy.Remove(element);
        }
        return true;
    }

    public void UseSpell(Spell spell)
    {
        Debug.Log($"Было использовано {spell.spellName} закинание, тип атаки заклинания {spell.spellType},\nурон заклинания {spell.spellDamage}.");
    }

    public void RefreshActiveElements()
    {

        activeElements.Clear();
        RefreshSlots();
        isInvokeActive= false;

    }

}
