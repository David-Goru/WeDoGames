﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstantDamageToEnemiesInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;

    List<ITurretAttackState> attackStateBehaviours = new List<ITurretAttackState>();

    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    float timer = 0f;
    float damageInterval = 0f;
    float damage = 0f;
    int damagePerTick = 0;

    bool areTargetsInRange = false;

    public override void InitializeComponent()
    {
        getDependencies();
        initMembers();
    }

    public override void UpdateComponent()
    {
        checkIfAreTargetsInRangeHasChanged();
        checkIfDamageChanged();
        if (timer >= damageInterval)
        {
            doDamage();
            timer = 0f;
        }
        else timer += Time.deltaTime;
    }

    void checkIfAreTargetsInRangeHasChanged()
    {
        if (areTargetsInRange != targetDetection.AreTargetsInRange) 
        {
            areTargetsInRange = targetDetection.AreTargetsInRange;
            if (areTargetsInRange) callAttackStateBehaviours(true);
            else callAttackStateBehaviours(false);
        } 
    }

    void callAttackStateBehaviours(bool enter)
    {
        foreach (ITurretAttackState attackStateBehaviour in attackStateBehaviours)
        {
            if (enter) attackStateBehaviour.OnAttackEnter();
            else attackStateBehaviour.OnAttackExit();
        }
    }

    void getDependencies()
    {
        turretStats = GetComponentInParent<TurretStats>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
        attackStateBehaviours = GetComponents<ITurretAttackState>().ToList();
    }

    void initMembers()
    {
        timer = 0f;
        damage = turretStats.GetStatValue(StatType.DAMAGE);
        damageInterval = damage > 20 ? 1 / 20 : 1 / damage;
        damagePerTick = damage > 20 ? Mathf.RoundToInt(damage / 20) : 1;
        areTargetsInRange = false;
    }

    void checkIfDamageChanged()
    {
        float currentDamage = turretStats.GetStatValue(StatType.DAMAGE);
        if (damage != currentDamage)
        {
            damage = currentDamage;
            damageInterval = 1 / damage;
        }
    }

    void doDamage()
    {
        if (ReferenceEquals(targetDetection, null)) return;

        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ITurretDamage turretDamageable = target.GetComponent<ITurretDamage>();
            if (turretDamageable != null) turretDamageable.OnTurretHit(transform.parent, damagePerTick, enemyDamageHandler);
        }
    }
}
