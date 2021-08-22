using UnityEngine;

public class DrawRayToEnemy : EffectComponent
{
    [SerializeField] Transform rayOrigin = null;
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] LineRenderer line = null;
    [SerializeField] Transform[] particlesTransform = null;

    public override void InitializeComponent()
    {
    }

    public override void UpdateComponent()
    {
        drawRay();
    }

    void drawRay()
    {
        if (targetDetection.CurrentTargets.Count <= 0) line.enabled = false;
        else
        {
            line.enabled = true;
            line.SetPosition(0, rayOrigin.position);
            line.SetPosition(1, targetDetection.CurrentTargets[0].position);
            Quaternion lookRotation = Quaternion.LookRotation((line.GetPosition(1) - line.GetPosition(0)).normalized);
            foreach (Transform transform in particlesTransform)
            {
                transform.rotation = lookRotation;
            }
        }
    }
}