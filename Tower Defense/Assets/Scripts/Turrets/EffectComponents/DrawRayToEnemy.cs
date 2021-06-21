using UnityEngine;

public class DrawRayToEnemy : EffectComponent
{
    [SerializeField] Transform rayOrigin = null;
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] LineRenderer line = null;

    public override void InitializeComponent()
    {
    }

    public override void UpdateComponent()
    {
        if(targetDetection.CurrentTargets.Count <= 0)
        {
            line.enabled = false;
        }
        else
        {
            line.enabled = true;
            line.SetPosition(0, rayOrigin.position);
            line.SetPosition(1, targetDetection.CurrentTargets[0].position);
        }
    }
}