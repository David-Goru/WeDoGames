using UnityEngine;

public class WaitingAnimationProjectile : Projectile
{
    [SerializeField] string clipName;
    [SerializeField] string parameterName;
    [SerializeField] Animator anim;

    bool isAnimationEnded;
    float timer = 0f;
    float animationTime = 0f;

    void Awake()
    {
        animationTime = getClipLength(anim, clipName);
    }

    protected override void initializate()
    {
        isAnimationEnded = false;
        timer = 0f;
        anim.SetTrigger(parameterName);
    }

    protected override void updateProjectile()
    {
        if (!target.gameObject.activeSelf) disable();
        if (!isAnimationEnded)
        {
            if (timer >= animationTime) isAnimationEnded = true;
            else timer += Time.deltaTime;
        }
        else chaseEnemy();
    }

    protected void chaseEnemy()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform == target) OnEnemyCollision(other);
    }

    protected override void OnEnemyCollision(Collider other)
    {
        damageEnemy(other);
        disable();
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
