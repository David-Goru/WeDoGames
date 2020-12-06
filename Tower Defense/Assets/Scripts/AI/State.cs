using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class State
{
	public enum STATE
	{
		MOVE, ATTACK
	};

	public enum EVENT
	{
		ENTER, UPDATE, EXIT
	};

	public STATE Name;
	protected EVENT stage;
	protected Base_AI npc;
	protected Animator anim;
	protected Transform target;
	protected State nextState;
	protected NavMeshAgent agent;

	public State(Base_AI _npc, Animator _anim, Transform _target, NavMeshAgent _agent)
	{
		npc = _npc;
		anim = _anim;
		target = _target;
		agent = _agent;
		stage = EVENT.ENTER;
	}

	public virtual void Enter() { stage = EVENT.UPDATE; }
	public virtual void Update() { stage = EVENT.UPDATE; }
	public virtual void Exit() { stage = EVENT.EXIT; }

	public State Process()
	{
		if (stage == EVENT.ENTER) Enter();
		if (stage == EVENT.UPDATE) Update();
		if (stage == EVENT.EXIT)
		{
			Exit();
			return nextState;
		}
		return this;
	}

	public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
		Debug.Log("HAS SIDO GOLPEADO");
        if (!turretTransform.gameObject.activeSelf) //The turret that shot you is already dead
        {
			nextState = new Move(npc, anim, npc.Goal, agent); //Aim for the nexus
		}
        else //The turret that shot you is alive
        {
			nextState = new Move(npc, anim, turretTransform, agent); //Aim for the turret
		}
		stage = EVENT.EXIT;
	}
}
