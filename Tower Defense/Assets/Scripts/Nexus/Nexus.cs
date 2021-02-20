using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] float health;

    public static Nexus Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public float Health
    {
        get
        {
            return health;
        }
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