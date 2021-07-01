using UnityEngine;

// <summary>
// FSM state Knockback. Enemies on this state will be pushed in the opposite direction they were hit.
// </summary>
public class Knockback : State
{
	RaycastHit hit;
	float pushDistance;
	Vector3 pushDirection;
	float maxViewRange = 0.5f;
	float lerpSpeed = 5f;
	Vector3 originPos;

	public Knockback(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.KNOCKBACK;
	}

	public override void Enter()
	{
		anim.ResetTrigger("moving");
		anim.SetTrigger("stunned");
		base.Enter();

		npc.CurrentTurret = null;
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
			//Debug.Log("Push distance reached");
		}
		else if (Physics.Raycast(npc.transform.position, pushDirection, out hit, maxViewRange))
		{
			//Debug.Log("Raycast entered");
			if (hit.transform.CompareTag("Turret"))
			{
				nextState = new Move(npc, anim, Target);
				stage = EVENT.EXIT;
				//Debug.Log("Push hit reached");
			}
		}
		else
		{
			//pushDirection * pushDistance + npc.transform.position si NO quiero un movimiento que decelere al llegar al destino
			npc.transform.position = Vector3.Lerp(npc.transform.position, pushDirection * pushDistance + originPos, lerpSpeed * Time.deltaTime);
			//Debug.Log("Pushing...");
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("stunned");
		npc.IsKnockbacked = false;
		base.Exit();
	}
}