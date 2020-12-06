using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Attack : State
{
	float attackTimer;
	Quaternion npcRotation;
	float rotationSpeed = 200f;

	public Attack(Base_AI _npc, Animator _anim, Transform _target, NavMeshAgent _agent) : base(_npc, _anim, _target, _agent)
	{
		Name = STATE.ATTACK;
		//Modify agent properties like speed, etc.
	}

	public override void Enter()
	{
		//anim.SetTrigger("attacking");
		base.Enter();

		//if (target == npc.Goal) nexus = target.GetComponent<Nexus>();

		npcRotation = Quaternion.LookRotation(new Vector3(target.position.x, npc.transform.position.y, target.position.z) - npc.transform.position);
		//npc.transform.LookAt(target);
		attackTarget();
	}

	public override void Update()
	{
		//base.Update();
		//Start damaging the turret. If turret is destroyed --> change state to move

		attackTimer += Time.deltaTime;
		if(attackTimer >= npc.AttackSpeed) //Attack depednding on npc attack speed
        {
			resetTimer();
			attackTarget();
        }

		if(npc.transform.rotation != npcRotation) //Rotate towards turret if you aren't already
        {
			npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, npcRotation, rotationSpeed * Time.deltaTime);
		}

		if (!target.gameObject.activeSelf)
		{
			//Debug.Log("El enemigo ha sido destruido");
            if (npc.Goal.gameObject.activeSelf)
            {
				nextState = new Move(npc, anim, npc.Goal, agent);
				stage = EVENT.EXIT;
			}
		}
	}

	public override void Exit()
	{
		//anim.ResetTrigger("attacking");
		base.Exit();
		attackTimer = 0f;
	}

	void resetTimer()
    {
		attackTimer = 0f;
    }

	void attackTarget()
    {
		//Debug.Log("GOLPE");
        if (target.gameObject.CompareTag("Turret"))
        {
			npc.currentTurretDamage.OnEnemyHit(npc.Damage);
		}
		else if (target.gameObject.CompareTag("Nexus"))
        {
			MasterHandler.Nexus.GetHit(npc.Damage);
        }
    }
}
