using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GrimoireSpawner : MonoBehaviour
{
    public List<Spell> spells;
    public GameObject spellInfoPrefab;

    public Transform contentPanel; // Панель для добавления элементов
    

    private void Start()
    {
        foreach (Spell spell in spells)
        {
            GameObject temp = Instantiate(spellInfoPrefab, contentPanel);
            temp.GetComponent<SpellInfoInGrimoire>().SetupSpellInfo(spell);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }
}
