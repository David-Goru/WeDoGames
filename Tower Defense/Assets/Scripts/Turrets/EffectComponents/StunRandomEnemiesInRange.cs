using UnityEngine;

public class StunRandomEnemiesInRange : EffectComponent
{
    [SerializeField] DetectRandomEnemiesOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    TurretStats turretStats;

    float timer = 0f;

    public override void InitializeComponent()
    {
        timer = 0f;
        GetDependencies();
    }

    public override void UpdateComponent()
    {
        if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            randomizeEnemies();
            stunEnemies();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }
    
    void randomizeEnemies()
    {
        targetDetection.RandomizeTargets();
    }

    private void stunEnemies()
    {
        float stunDuration = turretStats.GetStatValue(StatType.EFFECTDURATION);
        foreach (Transform target in targetDetection.CurrentTargets) stun(target, stunDuration);
    }

    private void stun(Transform target, float stunDuration)
    {
        target.GetComponent<IStunnable>().Stun(stunDuration);
    }

}