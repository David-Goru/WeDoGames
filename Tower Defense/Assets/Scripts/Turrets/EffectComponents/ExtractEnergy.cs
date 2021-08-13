using UnityEngine;

public class ExtractEnergy : EffectComponent
{
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource audioSource = null;

    TurretStats turretStats;

    float timer = 0f;

    public override void InitializeComponent()
    {
        GetDependencies();
    }

    public override void UpdateComponent()
    {
        if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            extract();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void GetDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    void extract()
    {
        
    }

    /*void doAnimation()
    {
        if (anim != null) anim.SetTrigger("Shoot");
    }

    void playSound()
    {
        if (audioSource != null) audioSource.Play();
    }*/
}
