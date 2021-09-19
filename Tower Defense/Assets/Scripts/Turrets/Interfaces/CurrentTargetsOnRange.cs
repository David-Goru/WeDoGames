using System.Collections.Generic;
using UnityEngine;

public abstract class CurrentTargetsOnRange : EffectComponent, IRangeViewable
{
    public abstract List<Transform> CurrentTargets { get; }
    protected bool areTargetsInRange;
    public bool AreTargetsInRange { get => areTargetsInRange; }

    protected bool isRangeActive;
    public bool IsRangeActive { get => isRangeActive; }

    protected TurretStats turretStats;
    [SerializeField] protected Pool areaPool = null;
    Transform areaObject;

    public virtual void ShowRange()
    {
        isRangeActive = true;
        if(areaPool != null)
        {
            areaObject = ObjectPooler.GetInstance().SpawnObject(areaPool.tag, transform.position).transform;
            areaObject.localScale = Vector3.one * turretStats.GetStatValue(StatType.ATTACKRANGE) * 2;
        }
    }

    public virtual void HideRange()
    {
        isRangeActive = false;
        if (areaObject != null)
        {
            ObjectPooler.GetInstance().ReturnToThePool(areaObject);
            areaObject = null;
        }
    }
}

