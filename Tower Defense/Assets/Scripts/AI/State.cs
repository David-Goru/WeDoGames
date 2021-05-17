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

	public Transform Target;
	public STATE Name;
	protected EVENT stage;
	protected Base_AI npc;
	protected Animator anim;
    protected State nextState;

    public State(Base_AI _npc, Animator _anim, Transform _target)
	{
		npc = _npc;
		anim = _anim;
		Target = _target;
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
		if (!turretTransform.gameObject.activeSelf && !npc.isStunned && !npc.isFeared) //The turret that shot you is already dead and you're not stunned AND you're not feared
        {
			nextState = new Move(npc, anim, npc.Goal); //Aim for the nexus
			stage = EVENT.EXIT;
		}
        else if(!npc.isStunned && !npc.isFeared) //The turret that shot you is alive and you're not stunned AND you're not feared
        {
			nextState = new Move(npc, anim, turretTransform); //Aim for the turret
			stage = EVENT.EXIT;
		}
	}
}
