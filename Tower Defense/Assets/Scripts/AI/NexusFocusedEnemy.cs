using UnityEngine;

public class NexusFocusedEnemy : Base_AI
{
    public override void OnObjectSpawn()
    {
        goal = Nexus.GetTransform;
        base.OnObjectSpawn();
    }
}