using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float health;

    public float Health { get => health;}
    public float MaxHealth { get => maxHealth;}

    public static Nexus Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
    }

    public bool IsNexusAlive()
    {
        return health > 0;
    }

    public void GetHit(float damage)
    {
        health -= damage;
    }
}