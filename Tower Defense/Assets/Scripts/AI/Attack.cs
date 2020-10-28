using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Attack : State
{

	public Attack(Base_AI _npc, Animator _anim, Transform _target, NavMeshAgent _agent) : base(_npc, _anim, _target, _agent)
	{
		Name = STATE.ATTACK;
		//Modify agent properties like speed, etc.
	}

	public override void Enter()
	{
		//anim.SetTrigger("attacking");
		base.Enter();
	}

	public override void Update()
	{
		//base.Update();
		//Start damaging the turret. If turret is destroyed --> change state to move
		Debug.Log("Golpeando");
		if (!target.gameObject.activeSelf)
		{
			Debug.Log("El enemigo ha sido destruido");
            if (npc.Goal.gameObject.activeSelf)
            {
				nextState = new Move(npc, anim, npc.Goal, agent);
				stage = EVENT.EXIT;
			}
            else
            {
				Debug.Log("NEXO DESTRUIDO");
            }
		}
	}

	public override void Exit()
	{
		//anim.ResetTrigger("attacking");
		base.Exit();
	}
}
