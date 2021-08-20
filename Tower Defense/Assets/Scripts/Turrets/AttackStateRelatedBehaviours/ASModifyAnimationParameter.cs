using UnityEngine;

public class ASModifyAnimationParameter : MonoBehaviour, ITurretAttackState
{
    enum ANIMATION_TYPE { TRIGGER, BOOL }

    [SerializeField] ANIMATION_TYPE animationType = ANIMATION_TYPE.BOOL;
    [SerializeField] Animator anim = null;
    [SerializeField] string parameterName = "";

    public void OnAttackEnter()
    {
        if (animationType == ANIMATION_TYPE.TRIGGER) anim.SetTrigger(parameterName);
        else anim.SetBool(parameterName, true);
    }

    public void OnAttackExit()
    {
        if (animationType == ANIMATION_TYPE.BOOL) anim.SetBool(parameterName, false);
    }
}