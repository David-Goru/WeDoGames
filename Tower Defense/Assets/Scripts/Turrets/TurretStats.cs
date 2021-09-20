using UnityEngine;

public class TurretStats : Entity, IHealable
{
    [SerializeField] TurretInfo buildingInfo = null;
    [SerializeField] Transform healParticlesPos = null;
    [SerializeField] Pool healVFX = null;

    HealthSlider healthSlider;
    IRangeViewable rangeViewable;
    ObjectPooler objectPooler;

    public TurretInfo BuildingInfo { get => buildingInfo; set => buildingInfo = value; }

    void Awake()
    {
        healthSlider = GetComponentInChildren<HealthSlider>();
        objectPooler = ObjectPooler.GetInstance();
        rangeViewable = GetComponentInChildren<IRangeViewable>();
    }

    void Start()
    {
        Initialize();
    }

    public override void CloseUI()
    {
        hideRange();
    }

    public override void ShowUI()
    {
        showRange();
    }

    void hideRange()
    {
        if (rangeViewable != null) rangeViewable.HideRange();
    }

    void showRange()
    {
        if (rangeViewable != null) rangeViewable.ShowRange();
    }

    public void Initialize()
    {
        title = buildingInfo.name;
        currentHP = (int)GetStatValue(StatType.MAXHEALTH);
        maxHP = currentHP;
    }

    public float SearchStatValue(StatType statType)
    {
        for (int i = 0; i < buildingInfo.currentStats.Count; i++)
        {
            if (buildingInfo.currentStats[i].Type == statType) return buildingInfo.currentStats[i].Value;
        }
        Debug.LogError("There is no " + statType + "on " + buildingInfo.name);
        return 0f;
    }

    public float GetStatValue(StatType type)
    {
        return buildingInfo.GetStat(type);
    }

    public void SetHP(int hp)
    {
        currentHP = hp;
        healthSlider.SetFillAmount((float)currentHP / maxHP);
    }

    public void Heal(int healValue)
    {
        int hp = Mathf.Clamp(currentHP + healValue, 0, maxHP);
        SetHP(hp);
        objectPooler.SpawnObject(healVFX.tag, healParticlesPos.position);
    }
}