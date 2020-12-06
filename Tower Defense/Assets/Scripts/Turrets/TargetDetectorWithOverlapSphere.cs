using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class TargetDetectorWithOverlapSphere : MonoBehaviour, ITargetsDetector
{
    Collider[] collidersCache = new Collider[64];

    float range;
    public float Range { get { return range; } set { range = value; } }

    LayerMask targetLayer;
    public LayerMask TargetLayer { get { return targetLayer; } set { targetLayer = value; } }


    List<Transform> targets = new List<Transform>();

    public List<Transform> GetTargets()
    {
        targets.Clear();
        int nTargets = Physics.OverlapSphereNonAlloc(this.transform.position, range, collidersCache, targetLayer);
        for(int i = 0; i < nTargets; i++)
        {
            targets.Add(collidersCache[i].transform);
        }
        return targets;
    }
}