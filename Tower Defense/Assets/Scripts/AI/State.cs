﻿using UnityEngine;

public class State
{
	public enum STATE
	{
		MOVE, ATTACK, STUN, FEAR, KNOCKBACK
	};

	public enum EVENT
	{
		ENTER, UPDATE, EXIT
	};

    private Transform target;
	protected STATE name;
	protected EVENT stage;
	protected BaseAI npc;
	protected Animator anim;
    protected State nextState;

	public Transform Target { get => target; set => target = value; }

    public State(BaseAI _npc, Animator _anim, Transform _target)
	{
		npc = _npc;
		anim = _anim;
		target = _target;
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

	public void OnTurretHit(Transform turretTransform)
	{
		if (!turretTransform.gameObject.activeSelf && !npc.IsStunned && !npc.IsFeared && !npc.IsKnockbacked)
		{
			nextState = new Move(npc, anim, npc.Goal);
			stage = EVENT.EXIT;
		}
		else if (npc.CurrentTurret == null && !npc.IsStunned && !npc.IsFeared && !npc.IsKnockbacked)
		{
			npc.CurrentTurret = turretTransform.transform;
			nextState = new Move(npc, anim, turretTransform);
			stage = EVENT.EXIT;
		}
	}
}