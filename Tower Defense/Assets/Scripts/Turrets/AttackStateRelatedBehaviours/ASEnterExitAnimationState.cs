using System.Collections.Generic;
using UnityEngine;

public class ASEnterExitAnimationState : MonoBehaviour, ITurretAttackState
{
    [SerializeField] Animator anim = null;
    [SerializeField] string parameterName = "";

    public void OnAttackEnter()
    {
        anim.SetBool(parameterName, true);
    }

    public void OnAttackExit()
    {
        anim.SetBool(parameterName, false);
    }
}