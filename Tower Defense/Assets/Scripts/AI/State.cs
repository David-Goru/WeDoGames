using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Finite State Machine for AI enemies. Also detects turrets hitting the enemies.
// </summary>
public class State
{
	public enum STATE
	{
		MOVE, ATTACK, STUN, FEAR
	};

	public enum EVENT
	{
		ENTER, UPDATE, EXIT
	};

	public STATE Name;
	protected EVENT stage;
	protected Base_AI npc;
	protected Animator anim;
	protected BuildingRange target;
	protected State nextState;

	public State(Base_AI _npc, Animator _anim, BuildingRange _target)
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

	public void OnTurretHit(BuildingRange turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        if (!turretTransform.gameObject.activeSelf && !npc.isStunned) //The turret that shot you is already dead and you're not stunned
        {
			nextState = new Move(npc, anim, npc.Goal); //Aim for the nexus
		}
        else if(!npc.isStunned) //The turret that shot you is alive and you're not stunned
        {
			nextState = new Move(npc, anim, turretTransform); //Aim for the turret
		}
		stage = EVENT.EXIT;
	}
}
