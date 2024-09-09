using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ActiveElements : MonoBehaviour
{
    [SerializeField] private List<Element> activeElements;
    [SerializeField] private List<Transform> activeSlots;
    [SerializeField] private List<GameObject> spellPrefabs;

    [SerializeField] private float timeOfSpellCast;

    [SerializeField] private bool isInvokeActive = false;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInfo playerInfo;

    public HandsAnimationController controller;


    public List<GameObject> currentSpells;

    public UnityEvent OnEnemyStatsChanged;

    private void Start()
    {
        
        RefreshSlots();
        RefreshSpells();
        playerInfo = player.GetComponent<PlayerInfo>();
    }

    private void RefreshSpells()
    {
        spellPrefabs = Resources.LoadAll<GameObject>("Prefabs/Spells").ToList();
    }

    private void RefreshSlots()
    {
        activeSlots.Clear();
        foreach (Transform slot in transform)
        {
            if (slot.GetComponent<ElementInBar>() != null) {
                activeSlots.Add(slot);
                ElementInBar elementInBar = slot.GetComponent<ElementInBar>();
                elementInBar.SetElement(null);
                elementInBar.RefreshImage();
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
            if (activeElements.Count == 0)
            {

                controller.TakeFirstElement();
            }
            else
            {
                controller.TakeSecondElement();
            }
            activeElements.Add(element.GetElement());
            AddElementInBar(element.GetElement());
        }
        if (activeElements.Count >= 3 && !isInvokeActive)
        {
            foreach (GameObject spellPrefab in spellPrefabs)
            {
                Spell spell = spellPrefab.GetComponent<SpellInHand>().spell;
                if (AreElementsMatching(spell.requiredElements, activeElements))
                {
                    controller.CastSpell();
                    UseSpell(spellPrefab);
                }
                else
                {
                    controller.DropElementFromLeftHand();
                    controller.DropElementFromRightHand();
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

    public void UseSpell(GameObject spellPrefab)
    {
        Debug.Log("asd");
        GameObject usedSpell;
        Spell spell = spellPrefab.GetComponent<SpellInHand>().spell;
        if (spell.spellType == SpellType.unique)
        {
            if (playerInfo.currentElementalStormBoost == null)
            {
                usedSpell = Instantiate(spellPrefab, transform);
                usedSpell.GetComponent<SpellInHand>().playerInfo = playerInfo;
                playerInfo.currentElementalStormBoost = usedSpell;

            }
        }
        else
        {
            Debug.Log($"Было использовано {spell.spellName} закинание, тип атаки заклинания {spell.spellType},\nурон заклинания {spell.spellDamage}.");

            usedSpell = Instantiate(spellPrefab, player);
            usedSpell.GetComponent<SpellInHand>().playerInfo = playerInfo;
            if (spell.isShield)
            {
                Destroy(playerInfo.currentShield);
                playerInfo.currentShield = usedSpell;
            }
            if (usedSpell.GetComponent<SoftGround>() != null)
            {
                if (playerInfo.currentSoftGround != null)
                {
                    playerInfo.currentSoftGround.GetComponent<SoftGround>().BuffEnemyAttackSpeedBack();
                    Destroy(playerInfo.currentSoftGround);
                    playerInfo.currentSoftGround = usedSpell;
                }
                else
                {
                    playerInfo.currentSoftGround = usedSpell;
                }
            }

            if (usedSpell.GetComponent<SwampFog>() != null)
            {
                if (playerInfo.currentSwampFog != null)
                {
                    playerInfo.currentSwampFog.GetComponent<SwampFog>().DestroyFog();

                    playerInfo.currentSwampFog = usedSpell;
                }
                else
                {
                    playerInfo.currentSwampFog = usedSpell;
                }

                usedSpell.GetComponent<SwampFog>().StartFog();
            }
        }
        
    }

    public void RefreshActiveElements()
    {

        activeElements.Clear();
        RefreshSlots();
        isInvokeActive= false;

    }



}
