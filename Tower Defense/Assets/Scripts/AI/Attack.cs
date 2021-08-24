using System.Collections;
using UnityEngine;

public class Attack : State
{
	float attackTimer;
	Quaternion npcRotation;
	float rotationSpeed = 200f;
	float timeBetweenAttacks;
	float attackDelay;

	Vector3 shootOffset = new Vector3(0f, 0.95f, 0f);

	public Attack(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.ATTACK;
		//Modify agent properties like speed, etc.
	}

	public override void Enter()
	{
		anim.SetTrigger("ATTACK");
		base.Enter();

		npcRotation = Quaternion.LookRotation(new Vector3(Target.position.x, npc.transform.position.y, Target.position.z) - npc.transform.position);
		timeBetweenAttacks = 1 / npc.AttackSpeed;
		attackDelay = getClipLength(anim, "Attack") / 2f;

		npc.StartCoroutine(dealDamage());
	}

	public override void Update()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer >= timeBetweenAttacks)
        {
			resetTimer();
			npc.StartCoroutine(dealDamage());

			anim.SetTrigger("ATTACK");
		}

		if (npc.transform.rotation != npcRotation) npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, npcRotation, rotationSpeed * Time.deltaTime);

		if (!Target.gameObject.activeSelf)
        {
			npc.setNewGoal();
			nextState = new Move(npc, anim, npc.Goal);
			stage = EVENT.EXIT;
		}

		timeBetweenAttacks = 1 / npc.AttackSpeed;
	}

	public override void Exit()
	{
		anim.ResetTrigger("ATTACK");
		anim.SetFloat("animSpeed", npc.Info.DefaultSpeed);
		base.Exit();
		attackTimer = 0f;
	}

	void resetTimer()
    {
		attackTimer = 0f;
    }

	void attackTarget()
    {
		if (npc.CompareTag("Fairy")) spawnProjectile();
		else meleeAttack();
    }

	IEnumerator dealDamage()
    {
		yield return new WaitForSeconds(attackDelay);
		attackTarget();
	}

	void spawnProjectile()
    {
		GameObject obj = ObjectPooler.GetInstance().SpawnObject("Fairy Projectile", 
			npc.transform.position + Vector3.ClampMagnitude(npc.transform.forward, 0.4f) + Vector3.ClampMagnitude(npc.transform.right, -0.2f) + shootOffset);
		obj.GetComponent<FairyProjectile>().SetInfo(Target, npc.Info.Damage);
    }

	void meleeAttack()
    {
		if (Target.CompareTag("Turret")) npc.CurrentTurretDamage.OnEnemyHit(npc.Info.Damage);
		else if (Target.CompareTag("Nexus")) Nexus.Instance.GetHit(npc.Info.Damage);
	}

	float getClipLength(Animator anim, string clipName)
	{
		AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

		foreach (AnimationClip clip in animationClips)
		{
			if (clip.name == clipName)
				return clip.length;
		}
		return 0f;
	}
}