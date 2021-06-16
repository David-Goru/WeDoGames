using UnityEngine;

// <summary>
// FSM state Knockback. Enemies on this state will be pushed in the opposite direction they were hit.
// </summary>
public class Knockback : State
{
	//float stunDuration;

	public Knockback(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.KNOCKBACK;
	}

	public override void Enter()
	{
		anim.SetTrigger("stunned");
		base.Enter();

		npc.CurrentTurret = null;
		//stunDuration = npc.StunDuration;
	}

	public override void Update()
	{
		/*stunDuration -= Time.deltaTime;
		if (stunDuration <= 0)
        {
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}*/
	}

	public override void Exit()
	{
		anim.ResetTrigger("stunned");
		npc.IsKnockbacked = false;
		base.Exit();
	}
}