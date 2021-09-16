using UnityEngine;

public class Fear : State
{
	float fearDuration;
	GameObject fearVFX;

	public Fear(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.FEAR;
	}

	public override void Enter()
	{
		npc.IsFeared = true;
		npc.PathReached = false;

		fearVFX = npc.ObjectPool.SpawnObject("FearVFX", npc.ParticlesSpawnPos.position);
		fearVFX.transform.SetParent(npc.ParticlesSpawnPos);
		fearVFX.transform.localScale = new Vector3(1f, 1f, 1f);

		npc.Speed = npc.Info.DefaultSpeed / 2;

		anim.SetTrigger("FEAR");
		base.Enter();

		PathData fearPosition = new PathData(-npc.transform.forward * 25.0f);
		fearDuration = npc.FearDuration;
		PathRequestManager.RequestPath(npc.transform.position, fearPosition, npc.Info.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		fearDuration -= Time.deltaTime;
		if (fearDuration <= 0)
		{
			if (!Target.gameObject.activeSelf)
			{
				npc.setNewGoal();
				nextState = new Move(npc, anim, npc.Goal);
			}
			else nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		npc.PathReached = false;
		npc.Speed = npc.Info.DefaultSpeed;
		anim.ResetTrigger("FEAR");
		if(fearVFX != null) npc.ObjectPool.ReturnToThePool(fearVFX.transform);
		npc.IsFeared = false;

		base.Exit();
	}
}