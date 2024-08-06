using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    [SerializeField] private GameObject popUpText;
    
    public void PopUp(float dmg)
    {
        popUpText.GetComponent<TextMesh>().text = $"-{dmg}";
        GameObject tmpDmg = Instantiate(popUpText, transform);
        tmpDmg.transform.localPosition = new Vector3 (0, transform.localScale.y * 0.5f, 0);
        Destroy(tmpDmg, 4f);
    }

}
