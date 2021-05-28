using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class TargetDetectorWithOverlapSphere : MonoBehaviour, ITargetsDetector
{
    Collider[] collidersCache = new Collider[64];

    List<Transform> targets = new List<Transform>();

    public List<Transform> GetTargets(float range, LayerMask targetsMask)
    {
        targets.Clear();
        int nTargets = Physics.OverlapSphereNonAlloc(this.transform.position, range, collidersCache, targetsMask);
        for(int i = 0; i < nTargets; i++)
        {
            targets.Add(collidersCache[i].transform);
        }
        return targets;
    }
}