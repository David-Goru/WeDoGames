using UnityEngine;

public class DamageEnemiesOnRangeWaitingAnimation : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    [SerializeField] ParticleSystem particles = null;
    [SerializeField] Animator anim = null;
    [SerializeField] string animationName = "";
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    bool areEnemiesInRange = false;
    float timer = 0f;

    float animationDuration = 0f;
    bool isAnimationPlaying = false;

    public override void InitializeComponent()
    {
        GetDependencies();
        timer = 0f;
        areEnemiesInRange = false;
        isAnimationPlaying = false;
        animationDuration = getClipLength(anim, animationName);
    }

    public override void UpdateComponent()
    {
        if (isAnimationPlaying)
        {
            if (isTimeToDamage()) doDamage();
            else timer += Time.deltaTime;
        }
        else if (areEnemiesInRange)
        {
            if (isTimeToStartAnimation())
            {
                startAnimation();
            }
            else timer += Time.deltaTime;
        }
        else if (targetDetection.AreTargetsInRange) 
        { 
            areEnemiesInRange = true;
            timer = 0f;
        }
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    bool isTimeToDamage()
    {
        return timer >= animationDuration;
    }

    bool isTimeToStartAnimation()
    {
        return timer >= turretStats.GetStatValue(StatType.ATTACKSPEED);
    }

    void startAnimation()
    {
        anim.SetTrigger("Shoot");
        isAnimationPlaying = true;
        timer = 0f;
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        playParticles();

        int damage = (int)turretStats.GetStatValue(StatType.DAMAGE);
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null) turretDamageable.OnTurretHit(transform, damage, enemyDamageHandler);
        }
        timer = 0f;
        areEnemiesInRange = false;
        isAnimationPlaying = false;
    }

    void playParticles()
    {
        if (particles != null) particles.Play();
    }

    float getClipLength(Animator anim, string clipName)
    {
        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0f;
    }
}
