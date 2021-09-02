using System;
using UnityEngine;

public class DrawRayToEnemy : EffectComponent
{
    [SerializeField] Transform rayOrigin = null;
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] LineRenderer line = null;
    [SerializeField] Transform[] particlesTransform = null;

    Transform currentenemy = null;
    Collider enemyCol;

    public override void InitializeComponent()
    {
        if(targetDetection.CurrentTargets.Count > 0) currentenemy = targetDetection.CurrentTargets[0];
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
            checkIfTargetChange();
            line.enabled = true;
            line.SetPosition(0, rayOrigin.position);
            line.SetPosition(1, enemyCol.bounds.center);
            Quaternion lookRotation = Quaternion.LookRotation((line.GetPosition(1) - line.GetPosition(0)).normalized);
            foreach (Transform transform in particlesTransform)
            {
                transform.rotation = lookRotation;
            }
        }
    }

    void checkIfTargetChange()
    {
        if(currentenemy != targetDetection.CurrentTargets[0])
        {
            currentenemy = targetDetection.CurrentTargets[0];
            enemyCol = currentenemy.GetComponent<Collider>();
        }
    }
}