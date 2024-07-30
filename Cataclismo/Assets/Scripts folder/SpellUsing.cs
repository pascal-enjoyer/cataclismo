using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUsing : MonoBehaviour
{
    public Spell currentSpell;

    public void UseSpell(Spell spell)
    {
        currentSpell= spell;

    }
}
