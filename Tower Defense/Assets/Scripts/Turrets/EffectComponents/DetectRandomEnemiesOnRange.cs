using System.Collections.Generic;
using UnityEngine;

public class DetectRandomEnemiesOnRange : CurrentTargetsOnRange
{
    [SerializeField] LayerMask targetLayer = 0;
    ITargetsDetector targetsDetector;
    TurretStats turretStats;
    List<Transform> currentTargets = new List<Transform>();

    public override List<Transform> CurrentTargets { get { return currentTargets; } }


    public override void InitializeComponent()
    {
        getDependencies();
        currentTargets.Clear();
    }


    public override void UpdateComponent()
    {
    }

    private void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        targetsDetector = GetComponent<ITargetsDetector>();
    }

    public void RandomizeTargets()
    {
        clearTargets();
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        int objectives = (int)turretStats.GetStatValue(StatType.OBJECTIVESTOHIT);

        List<Transform> targets = targetsDetector.GetTargets(range, targetLayer);
        for (int i = 0; i < objectives; i++)
        {
            if (targets.Count <= 0) return;
            SelectRandomTarget(targets);
        }
    }

    void SelectRandomTarget(List<Transform> targets)
    {
        int randomIndex = Random.Range(0, targets.Count);
        currentTargets.Add(targets[randomIndex]);
        targets.RemoveAt(randomIndex);
    }

    void clearTargets()
    {
        currentTargets.Clear();
    }

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (turretStats != null) debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////

}
