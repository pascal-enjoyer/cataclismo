using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAnimationController : MonoBehaviour
{
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private GameObject rightHandGlove;
    private Animator rightHandGloveAnimator;

    private int isTakeFirstElementHash;
    private int isFirstElementIdleHash;
    private int isFirstElementDropHash;
    private int isLeftMovingIdleHash;
    private int isFirstElementToRightHandHash;

    private int isCatchElementFromLeftHand;
    private int isDropElementFromRightHand;
    private int isRightMovingIdleHash;
    private int isElementIdleHash;
    private int isCastSpellHash;

    private void Start()
    {
        isLeftMovingIdleHash = Animator.StringToHash("movingIdleBool");
        isFirstElementDropHash = Animator.StringToHash("elementDropBool");
        isTakeFirstElementHash = Animator.StringToHash("takeElementBool");
        isFirstElementIdleHash = Animator.StringToHash("elementIdleBool");
        isFirstElementToRightHandHash = Animator.StringToHash("moveElementBool");

        isRightMovingIdleHash = Animator.StringToHash("movingIdleBool");
        isCatchElementFromLeftHand = Animator.StringToHash("catchElementBool");
        isElementIdleHash = Animator.StringToHash("elementIdleBool");
        isDropElementFromRightHand = Animator.StringToHash("elementDropBool");
        isCastSpellHash = Animator.StringToHash("elementCastBool");

        leftHandAnimator.SetBool(isLeftMovingIdleHash, true);
        rightHandAnimator.SetBool(isRightMovingIdleHash, true);
    }

    public void SetRightHandGlove(GameObject glove)
    {
        rightHandGlove = glove;
        rightHandGloveAnimator = glove.GetComponent<Animator>();

        if (rightHandGloveAnimator != null)
        {
            SyncAnimators(rightHandAnimator, rightHandGloveAnimator);
            rightHandGloveAnimator.SetBool(isRightMovingIdleHash, true);
        }
    }

    private void SyncAnimators(Animator mainAnimator, Animator gloveAnimator)
    {
        foreach (AnimatorControllerParameter param in mainAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                gloveAnimator.SetBool(param.nameHash, mainAnimator.GetBool(param.nameHash));
            }
            else if (param.type == AnimatorControllerParameterType.Float)
            {
                gloveAnimator.SetFloat(param.nameHash, mainAnimator.GetFloat(param.nameHash));
            }
            else if (param.type == AnimatorControllerParameterType.Int)
            {
                gloveAnimator.SetInteger(param.nameHash, mainAnimator.GetInteger(param.nameHash));
            }
            else if (param.type == AnimatorControllerParameterType.Trigger)
            {
                if (mainAnimator.GetBool(param.nameHash))
                {
                    gloveAnimator.SetTrigger(param.nameHash);
                }
            }
        }
    }

    public void TakeFirstElement()
    {
        leftHandAnimator.SetBool(isTakeFirstElementHash, true);
        StartCoroutine(WaitForAnimation(leftHandAnimator, isTakeFirstElementHash, () =>
        {
            leftHandAnimator.SetBool(isTakeFirstElementHash, false);
            leftHandAnimator.SetBool(isFirstElementIdleHash, true);
        }));
    }

    public void TakeSecondElement()
    {
        leftHandAnimator.SetBool(isFirstElementToRightHandHash, true);
        rightHandAnimator.SetBool(isCatchElementFromLeftHand, true);
        if (rightHandGloveAnimator != null)
        {
            rightHandGloveAnimator.SetBool(isCatchElementFromLeftHand, true);
        }

        StartCoroutine(WaitForAnimation(leftHandAnimator, isFirstElementToRightHandHash, () =>
        {
            leftHandAnimator.SetBool(isFirstElementToRightHandHash, false);
            leftHandAnimator.SetBool(isFirstElementIdleHash, true);
        }));

        StartCoroutine(WaitForAnimation(rightHandAnimator, isCatchElementFromLeftHand, () =>
        {
            rightHandAnimator.SetBool(isCatchElementFromLeftHand, false);
            if (rightHandGloveAnimator != null)
            {

                rightHandGloveAnimator.SetBool(isCatchElementFromLeftHand, false);
            }

            rightHandAnimator.SetBool(isElementIdleHash, true);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isElementIdleHash, true);
            }
        }));
    }

    public void DropElementFromLeftHand()
    {
        leftHandAnimator.SetBool(isFirstElementDropHash, true);


        StartCoroutine(WaitForAnimation(leftHandAnimator, isFirstElementDropHash, () =>
        {
            leftHandAnimator.SetBool(isFirstElementDropHash, false);

            leftHandAnimator.SetBool(isElementIdleHash, false);
            leftHandAnimator.SetBool(isLeftMovingIdleHash, true);

        }));
    }

    public void DropElementFromRightHand()
    {
        rightHandAnimator.SetBool(isElementIdleHash, false);
        if (rightHandGloveAnimator != null)
        {
            rightHandGloveAnimator.SetBool(isElementIdleHash, false);
        }
        rightHandAnimator.SetBool(isDropElementFromRightHand, true);
        if (rightHandGloveAnimator != null)
        {
            rightHandGloveAnimator.SetBool(isDropElementFromRightHand, true);
        }

        StartCoroutine(WaitForAnimation(rightHandAnimator, isDropElementFromRightHand, () =>
        {
            rightHandAnimator.SetBool(isDropElementFromRightHand, false);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isDropElementFromRightHand, false);
            }


            rightHandAnimator.SetBool(isRightMovingIdleHash, true);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isRightMovingIdleHash, true);
            }

        }));

    }

    public void CastSpell()
    {
        rightHandAnimator.SetBool(isCastSpellHash, true);
        if (rightHandGloveAnimator != null)
        {
            rightHandGloveAnimator.SetBool(isCastSpellHash, true);
        }

        StartCoroutine(WaitForAnimation(rightHandAnimator, isCastSpellHash, () =>
        {
            rightHandAnimator.SetBool(isCastSpellHash, false);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isCastSpellHash, false);
            }
            rightHandAnimator.SetBool(isRightMovingIdleHash, true);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isRightMovingIdleHash, true);
            }
            rightHandAnimator.SetBool(isElementIdleHash, false);
            if (rightHandGloveAnimator != null)
            {
                rightHandGloveAnimator.SetBool(isElementIdleHash, false);
            }
            DropElementFromLeftHand();
        }));
    }

    private IEnumerator WaitForAnimation(Animator animator, int animationHash, System.Action onComplete)
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animationHash);
        onComplete?.Invoke();
    }
}
