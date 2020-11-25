using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class Move : State
{

	public Move(Base_AI _npc, Animator _anim, Transform _target, NavMeshAgent _agent) : base(_npc, _anim, _target, _agent)
	{
		Name = STATE.MOVE;
	}

	public override void Enter()
	{
		//anim.SetTrigger("moving");
		base.Enter();
		agent.isStopped = false;
		agent.SetDestination(target.position);
	}

	public override void Update()
	{
		/*
		if (!agent.pathPending) //Make sure we've reached the destination
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
				{
					nextState = new Attack(npc, anim, target, agent);
					stage = EVENT.EXIT;
				}
			}
		}
		*/
		

		if (npc.IsTargetTrigger)
        {
			agent.velocity = new Vector3(0, 0, 0);
			agent.isStopped = true;
			npc.IsTargetTrigger = false;
			nextState = new Attack(npc, anim, target, agent);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		//anim.ResetTrigger("moving");
		base.Exit();
	}
}
