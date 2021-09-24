using UnityEngine;
using UnityEngine.Audio;

public class Nexus : Entity
{
    [SerializeField] int StartingHP = 0;
    [SerializeField] GameObject endScreenUI = null;
    [SerializeField] AudioMixer soundsMixer = null;

    public bool IsFullHealth { get => currentHP == maxHP; }
    public bool IsAlive { get => currentHP > 0; }

    public static Nexus Instance;
    public static Transform GetTransform { get => Instance.transform; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        title = name;
        maxHP = StartingHP;
        currentHP = maxHP;
    }

    void stopSounds()
    {
        soundsMixer.SetFloat("SoundsVolume", -80);
    }

    public void GetHit(float damage)
    {
        currentHP -= Mathf.RoundToInt(damage);

        if (currentHP <= 0)
        {
            currentHP = 0;
            stopSounds();
            endScreenUI.SetActive(true);
            Time.timeScale = 0;
        }

        UI.UpdateNexusLifeText(currentHP);
    }
}