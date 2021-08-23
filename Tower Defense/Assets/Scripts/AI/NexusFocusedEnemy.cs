public class NexusFocusedEnemy : BaseAI
{
    public override void OnObjectSpawn()
    {
        goal = Nexus.GetTransform;
        base.OnObjectSpawn();
    }
}