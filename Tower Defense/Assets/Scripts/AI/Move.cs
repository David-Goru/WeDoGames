using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class Move : State
{

	const float TIME_BETWEEN_REQUESTS = 0.5f;
	float requestTimer = 0f;

	public Move(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		Name = STATE.MOVE;
	}

	public override void Enter()
	{
		Debug.Log("EMPIEZA A ANDAR");
		anim.SetTrigger("moving");
		base.Enter();
		//agent.isStopped = false;
		//agent.SetDestination(target.position);

		PathRequestManager.RequestPath(npc.transform.position, target.position, npc.OnPathFound);
	}

	public override void Update()
	{
		if (npc.IsTargetTrigger)
        {
			//agent.velocity = new Vector3(0, 0, 0);
			//agent.isStopped = true;
			npc.IsTargetTrigger = false;
			nextState = new Attack(npc, anim, target);
			stage = EVENT.EXIT;
		}
        else
        {
			requestTimer += Time.deltaTime;
			if (requestTimer >= TIME_BETWEEN_REQUESTS)
			{
				PathRequestManager.RequestPath(npc.transform.position, target.position, npc.OnPathFound);
				requestTimer = 0f;
			}
		}
		/*
		else if (!agent.pathPending) //Make sure we've reached the destination
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
	}

	public override void Exit()
	{
		requestTimer = 0f;
		anim.ResetTrigger("moving");
		base.Exit();
	}
}
