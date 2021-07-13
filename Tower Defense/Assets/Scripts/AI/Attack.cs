using UnityEngine;

// <summary>
// FSM state Attack. In this state the AI will go to the target position and attack it.
// </summary>
public class Attack : State
{
	float attackTimer;
	Quaternion npcRotation;
	float rotationSpeed = 200f;

	public Attack(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.ATTACK;
		//Modify agent properties like speed, etc.
	}

	public override void Enter()
	{
		anim.SetTrigger("attacking");
		anim.SetFloat("animSpeed", 0);
		base.Enter();

		npcRotation = Quaternion.LookRotation(new Vector3(Target.position.x, npc.transform.position.y, Target.position.z) - npc.transform.position);

		attackTarget();
	}

	public override void Update()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer >= npc.Info.AttackSpeed)
        {
			resetTimer();
			attackTarget();
			anim.SetFloat("animSpeed", 0);
		}
        else
        {
			anim.SetFloat("animSpeed", attackTimer/npc.Info.AttackSpeed);
        }

		if (npc.transform.rotation != npcRotation) npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, npcRotation, rotationSpeed * Time.deltaTime);

		if (!Target.gameObject.activeSelf && npc.Goal.gameObject.activeSelf)
        {
			nextState = new Move(npc, anim, npc.Goal);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("attacking");
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
        if (Target.gameObject.CompareTag("Turret")) npc.CurrentTurretDamage.OnEnemyHit(npc.Info.Damage);
		else if (Target.gameObject.CompareTag("Nexus")) Nexus.Instance.GetHit(npc.Info.Damage);
    }
}