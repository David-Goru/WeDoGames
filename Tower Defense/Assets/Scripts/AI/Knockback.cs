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
		anim.ResetTrigger("MOVE");
		anim.SetTrigger("STUN");
		base.Enter();

		pushDistance = npc.PushDistance;
		pushDirection = npc.PushDirection;
		originPos = npc.transform.position;
	}

	public override void Update()
	{
        if (Vector3.Distance(npc.transform.position, pushDirection * pushDistance + originPos) < 0.1f)
        {
			decideTargetToMove();
			stage = EVENT.EXIT;
		}
		else if (Physics.Raycast(npc.transform.position, pushDirection, out hit, maxViewRange, obstacleLayerMask))
		{
			decideTargetToMove();
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
		anim.ResetTrigger("STUN");
		anim.SetFloat("animSpeed", 1.0f);
		npc.IsKnockbacked = false;
		base.Exit();
	}

	void decideTargetToMove()
	{
		if (!Target.gameObject.activeSelf)
		{
			npc.setNewGoal();
			nextState = new Move(npc, anim, npc.Goal);
		}
		else nextState = new Move(npc, anim, Target);
	}
}