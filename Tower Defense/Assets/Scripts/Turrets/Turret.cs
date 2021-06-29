using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is responsible of handling all the behaviours of the turret
/// </summary>

[SelectionBase]
[RequireComponent(typeof(TurretStats))]
public class Turret : MonoBehaviour, IPooledObject
{
    List<ITurretBehaviour> behaviours = new List<ITurretBehaviour>();
    TurretStats turretStats = null;

    private void Awake()
    {
        behaviours = GetComponentsInChildren<ITurretBehaviour>().ToList();
        turretStats = GetComponent<TurretStats>();
    }

    void Start()
    {
        InilitalizeBehaviours();
    }

    void Update()
    {
        foreach (var behaviour in behaviours) behaviour.UpdateBehaviour();
    }

    private void InilitalizeBehaviours()
    {
        foreach (var behaviour in behaviours) behaviour.InitializeBehaviour();
    }

    public void AddBehaviour(ITurretBehaviour behaviour)
    {
        behaviours.Add(behaviour);
        behaviour.InitializeBehaviour();
    }

    public void RemoveBehaviour(ITurretBehaviour behaviour)
    {
        behaviours.Remove(behaviour);
    }

    /// <summary>
    /// Called when turret spawns. It resets its values.
    /// </summary>
    public void OnObjectSpawn()
    {
        InitializeTurretStats();
        InilitalizeBehaviours();
    }

    private void InitializeTurretStats()
    {
        turretStats.InitializeStats();
    }
}