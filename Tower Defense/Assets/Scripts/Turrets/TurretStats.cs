using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : Entity, IHealable
{
    [SerializeField] TurretInfo buildingInfo = null;
    [SerializeField] Transform healParticlesPos = null;
    [SerializeField] Pool healVFX = null;

    IRangeViewable rangeViewable;
    ObjectPooler objectPooler;

    public TurretInfo BuildingInfo { get => buildingInfo; set => buildingInfo = value; }

    void Awake()
    {
        objectPooler = ObjectPooler.GetInstance();
        rangeViewable = GetComponentInChildren<IRangeViewable>();
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (rangeViewable.IsRangeActive && Input.GetMouseButton(0) && mouseIsOutOfTurret())
        {
            rangeViewable.HideRange();
        }
    }

    bool mouseIsOutOfTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return !Physics.Raycast(ray, out hit) || hit.transform != transform;
    }

    public void Initialize()
    {
        title = buildingInfo.name;
        currentHP = (int)GetStatValue(StatType.MAXHEALTH);
        maxHP = currentHP;
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if(Master.Instance.DoingAction() || rangeViewable.IsRangeActive) return;

        rangeViewable.ShowRange();
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

    public void Heal(int healValue)
    {
        currentHP = Mathf.Clamp(currentHP + healValue, 0, maxHP);
        objectPooler.SpawnObject(healVFX.tag, healParticlesPos.position);
    }
}