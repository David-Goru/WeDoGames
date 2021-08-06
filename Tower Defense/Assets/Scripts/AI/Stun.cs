using UnityEngine;

// <summary>
// FSM state Stun. Enemies on this state won't be able to move or attack
// </summary>
public class Stun : State
{
	float stunDuration;

	public Stun(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.STUN;
	}

	public override void Enter()
	{
		anim.SetTrigger("stunned");
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
		anim.ResetTrigger("stunned");
		npc.IsStunned = false;
		base.Exit();
	}
}