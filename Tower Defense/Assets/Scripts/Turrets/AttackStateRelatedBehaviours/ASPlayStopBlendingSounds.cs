using UnityEngine;

public class ASPlayStopBlendingSounds : EffectComponent, ITurretAttackState
{
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip startingClip = null;
    [SerializeField] AudioClip loopingClip = null;
    [SerializeField] AudioClip endingClip = null;

    float timer = 0f;
    float audioLength = 0f;
    bool isStarting = true;

    public override void InitializeComponent()
    {
        isStarting = false;
    }

    public void OnAttackEnter()
    {
        setAudioSource(startingClip, false);
        audioLength = startingClip.length;
        isStarting = true;
        timer = 0f;
    }
    
    public override void UpdateComponent()
    {
        if (!isStarting) return;
        if (timer >= audioLength)
        {
            setAudioSource(loopingClip, true);
            isStarting = false;
        }
        else timer += Time.deltaTime;
    }

    public void OnAttackExit()
    {
        if (endingClip != null)
        {
            setAudioSource(endingClip, false);
        }
        else audioSource.Stop();
    }

    void setAudioSource(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}