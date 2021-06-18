using UnityEngine;

// <summary>
// FSM state Knockback. Enemies on this state will be pushed in the opposite direction they were hit.
// </summary>
public class Knockback : State
{
	RaycastHit hit;
	float pushDistance;
	Vector3 pushDirection;
	float maxViewRange = 2f;
	float lerpSpeed = 5f;

	public Knockback(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.KNOCKBACK;
	}

	public override void Enter()
	{
		anim.SetTrigger("stunned");
		base.Enter();

		npc.CurrentTurret = null;
		pushDistance = npc.PushDistance;
		pushDirection = npc.PushDirection;
	}

	public override void Update()
	{
        if (Vector3.Distance(npc.transform.position, pushDirection * pushDistance + npc.transform.position) < 0.1f)
        {
			npc.transform.position = -npc.transform.forward * pushDistance + npc.transform.position;
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		else if (Physics.Raycast(npc.transform.position, pushDirection, out hit, maxViewRange))
		{
			if (hit.transform.CompareTag("Turret"))
			{
				nextState = new Move(npc, anim, Target);
				stage = EVENT.EXIT;
			}
		}
		else
		{
			npc.transform.position = Vector3.Lerp(npc.transform.position, pushDirection * pushDistance + npc.transform.position, lerpSpeed * Time.deltaTime);
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("stunned");
		npc.IsKnockbacked = false;
		base.Exit();
	}
}