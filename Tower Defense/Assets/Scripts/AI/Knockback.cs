using UnityEngine;

// <summary>
// FSM state Knockback. Enemies on this state will be pushed in the opposite direction they were hit.
// </summary>
public class Knockback : State
{
	RaycastHit hit;
	float pushDistance;
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
	}

	public override void Update()
	{
		//Por ahora doy por hecho que se empuja en la dirección opuesta para debug (cambiar a la dirección del proyectil)
        if (Vector3.Distance(npc.transform.position, -npc.transform.forward * pushDistance + npc.transform.position) < 0.1f)
        {
			npc.transform.position = -npc.transform.forward * pushDistance + npc.transform.position;
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		//-npc.transform... debería ser el lugar al que vas a ser empujado, por ahora lo dejo así para debug
		else if (Physics.Raycast(npc.transform.position, -npc.transform.forward, out hit, maxViewRange))
		{
			if (hit.transform.CompareTag("Turret"))
			{
				nextState = new Move(npc, anim, Target);
				stage = EVENT.EXIT;
			}
		}
		//De nuevo, cambiar la dirección a la del proyectil en el futuro
		else
		{
			npc.transform.position = Vector3.Lerp(npc.transform.position, -npc.transform.forward * pushDistance + npc.transform.position, lerpSpeed * Time.deltaTime);
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("stunned");
		npc.IsKnockbacked = false;
		base.Exit();
	}
}