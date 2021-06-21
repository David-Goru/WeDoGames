﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour, ITurretDamage, ISlowable, IPoisonable
{
    MeshRenderer meshRenderer;
    float timer = 0f;
    bool hit = false;

    Color initialColor;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        initialColor = meshRenderer.material.color;
    }

    private void Update()
    {
        if (hit)
        {
            if(timer >= 0.1f)
            {
                hit = false;
                meshRenderer.material.color = initialColor;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        hit = true;
        timer = 0f;
        meshRenderer.material.color = Color.red;
    }

    public void Slow(float secondsSlowed, float slowReduction)
    {
        print("I have been slowed for " + secondsSlowed + " seconds with a reduction of " + slowReduction);
    }

    public void Poison(float secondsPoisoned, float poisonDamagePerSecond)
    {
        print("I have been poisoned for " + secondsPoisoned + " seconds with a damage of " + poisonDamagePerSecond + " per second");
    }
}
