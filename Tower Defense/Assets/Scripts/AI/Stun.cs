using UnityEngine;

public class Stun : State
{
	float stunDuration;
	GameObject stunVFX;

	public Stun(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.STUN;
	}

	public override void Enter()
	{
		anim.SetTrigger("STUN");
		stunVFX = npc.ObjectPool.SpawnObject("StunVFX", npc.ParticlesSpawnPos.position);
		base.Enter();

		stunDuration = npc.StunDuration;
	}

	public override void Update()
	{
		stunDuration -= Time.deltaTime;
		if (stunDuration <= 0)
        {
			if (!Target.gameObject.activeSelf)
			{
				npc.setNewGoal();
				nextState = new Move(npc, anim, npc.Goal);
			}
			else if (npc.PathReached) nextState = new Attack(npc, anim, Target);
			else nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("STUN");
		npc.ObjectPool.ReturnToThePool(stunVFX.transform);
		npc.IsStunned = false;
		base.Exit();
	}
}