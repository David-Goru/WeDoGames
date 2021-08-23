using UnityEngine;

public class TurretFocusedEnemy : BaseAI
{
    public override void OnObjectSpawn()
    {
        goal = findGoal();
        base.OnObjectSpawn();
    }

    public override void EnemyUpdate()
    {
        base.EnemyUpdate();

        if (isTargetingNexus() && isAnyTurretAlive() && !isEnemyUnderCC())
        {
            setNewGoal();
            currentState = new Move(this, anim, goal);
        }
    }

    public override void setNewGoal()
    {
        goal = findGoal();
    }

    Transform findGoal()
    {
        if (isAnyTurretAlive()) return getRandomTarget();

        return Nexus.GetTransform;
    }

    Transform getRandomTarget()
    {
        return Master.Instance.ActiveTurrets[Random.Range(0, Master.Instance.ActiveTurrets.Count)].transform;
    }

    bool isAnyTurretAlive()
    {
        return Master.Instance.ActiveTurrets.Count > 0;
    }

    bool isTargetingNexus()
    {
        return goal == Nexus.GetTransform;
    }

    bool isEnemyUnderCC()
    {
        return IsKnockbacked || IsStunned || IsFeared;
    }
}