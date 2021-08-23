using UnityEngine;

public class Knockback : State
{
	RaycastHit hit;
	float pushDistance;
	Vector3 pushDirection;
	float maxViewRange = 0.5f;
	float lerpSpeed = 5f;
	Vector3 originPos;

	LayerMask obstacleLayerMask = LayerMask.GetMask("Object");

	public Knockback(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.KNOCKBACK;
	}

	public override void Enter()
	{
		npc.IsStunned = false; //Just in case stun got interrupted
		npc.IsFeared = false; //Just in case fear got interrupted
		anim.ResetTrigger("moving");
		anim.SetBool("stunned", true);
		base.Enter();

		pushDistance = npc.PushDistance;
		pushDirection = npc.PushDirection;
		originPos = npc.transform.position;
	}

	public override void Update()
	{
        if (Vector3.Distance(npc.transform.position, pushDirection * pushDistance + originPos) < 0.1f)
        {
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		else if (Physics.Raycast(npc.transform.position, pushDirection, out hit, maxViewRange, obstacleLayerMask))
		{
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		else
		{
			//pushDirection * pushDistance + npc.transform.position si NO quiero un movimiento que decelere al llegar al destino
			npc.transform.position = Vector3.Lerp(npc.transform.position, pushDirection * pushDistance + originPos, lerpSpeed * Time.deltaTime);
		}
	}

	public override void Exit()
	{
		anim.SetBool("stunned", false);
		anim.SetFloat("animSpeed", 1.0f);
		npc.IsKnockbacked = false;
		base.Exit();
	}
}