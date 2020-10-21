using System.Collections;
using System.Collections.Generic;
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
	protected GameObject npc;
	protected Animator anim;
	protected Transform target;
	protected State nextState;
	protected NavMeshAgent agent;

	private float visDist = 10.0f; //Distance of vision
	private float attackDist = 7.0f; //Distance from the target to start attacking

	public State(GameObject _npc, Animator _anim, Transform _target, NavMeshAgent _agent)
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
}

public class Move : State
{
	public Move(GameObject _npc, Animator _anim, Transform _target, NavMeshAgent _agent) : base(_npc, _anim, _target, _agent)
	{
		Name = STATE.MOVE;
	}

	public override void Enter()
	{
		//anim.SetTrigger("moving");
		base.Enter();
	}

	public override void Update()
	{
		//base.Update();
		//if (Turret starts attacking me)
		//{
		//	stage = EVENT.EXIT;
		//	change nextState to ATTACK
		//}
		agent.SetDestination(target.position);
	}

	public override void Exit()
	{
		//anim.ResetTrigger("moving");
		base.Exit();
	}
}

public class Attack : State
{

	public Attack(GameObject _npc, Animator _anim, Transform _target, NavMeshAgent _agent) : base(_npc, _anim, _target, _agent)
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
	}

	public override void Exit()
	{
		//anim.ResetTrigger("attacking");
		base.Exit();
	}
}

