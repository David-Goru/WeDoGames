﻿using UnityEngine;

public class Nexus : Entity
{
    [SerializeField] int StartingHP = 0;
    [SerializeField] GameObject endScreenUI = null;

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

    public void GetHit(float damage)
    {
        currentHP -= Mathf.RoundToInt(damage);

        if (currentHP <= 0)
        {
            currentHP = 0;
            endScreenUI.SetActive(true);
            Time.timeScale = 0;
        }

        UI.UpdateNexusLifeText(currentHP);
    }
}