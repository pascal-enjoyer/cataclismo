using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalStorm : MonoBehaviour
{
    private Animator animator;

    public void DestroyElementalStorm()
    {
        int animHash = Animator.StringToHash("BurningEnd");
        animator = transform.GetComponent<Animator>();

        animator.SetBool(animHash, true);
        StartCoroutine(WaitForAnimation(animator, animHash, () =>
        {
            Destroy(gameObject);
        }));
    }

    private IEnumerator WaitForAnimation(Animator animator, int animationHash, System.Action onComplete)
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animationHash);
        onComplete?.Invoke();
    }
}
