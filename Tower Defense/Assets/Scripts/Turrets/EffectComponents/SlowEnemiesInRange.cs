using UnityEngine;

public class SlowEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    TurretStats turretStats;

    float timer = 0f;

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
        timer = 0f;
    }

    public override void UpdateComponent()
    {
        if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            slowEnemies();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void slowEnemies()
    {
        if (ReferenceEquals(targetDetection, null)) return;

        float secondsSlowed = turretStats.GetStatValue(StatType.EFFECTDURATION);
        float slowReduction = turretStats.GetStatValue(StatType.SLOWREDUCTION);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ISlowable enemyISlowable = target.GetComponent<ISlowable>();
            if (enemyISlowable != null)
            {
                enemyISlowable.Slow(secondsSlowed, slowReduction);
            }
        }
        timer = 0f;
    }
}
