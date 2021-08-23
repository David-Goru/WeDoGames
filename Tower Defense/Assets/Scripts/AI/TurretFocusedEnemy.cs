using UnityEngine;

public class TurretFocusedEnemy : Base_AI
{
    public override void OnObjectSpawn()
    {
        goal = findGoal();
        base.OnObjectSpawn();
    }

    public override void EnemyUpdate()
    {
        base.EnemyUpdate();

        if (isTargetingNexus() && isAnyTurretAlive()) return; //Change currentState to Move to the recently spawned turret
    }

    public override void setNewGoal()
    {
        goal = findGoal();
    }

    Transform findGoal()
    {
        if (isAnyTurretAlive()) return getClosestTarget();

        return Nexus.GetTransform;
    }

    Transform getClosestTarget()
    {
        return transform; //Here is where the AI have to decide where is its turret target
    }

    bool isAnyTurretAlive()
    {
        return false;
    }

    bool isTargetingNexus()
    {
        return goal == Nexus.GetTransform;
    }
}