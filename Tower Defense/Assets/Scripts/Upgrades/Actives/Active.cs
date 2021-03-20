using UnityEngine;
using System;

/// <summary>
/// This is a class
/// </summary>
[CreateAssetMenu(fileName = "Active", menuName = "Upgrades/Active", order = 0)]
public class Active : Upgrade
{
    [SerializeField] ActiveAction activeAction;

    public ActiveAction ActiveAction { get => activeAction; }
}