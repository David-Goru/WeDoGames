using UnityEngine;

public class Stun : State
{
	float stunDuration;

	public Stun(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.STUN;
	}

	public override void Enter()
	{
		npc.IsFeared = false; //Just in case fear got interrupted
		anim.SetBool("stunned", true);
		base.Enter();

		stunDuration = npc.StunDuration;
	}

	public override void Update()
	{
		stunDuration -= Time.deltaTime;
		if (stunDuration <= 0)
        {
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.SetBool("stunned", false);
		anim.SetFloat("animSpeed", 1.0f);
		npc.IsStunned = false;
		base.Exit();
	}
}