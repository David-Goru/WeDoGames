using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterInfo", menuName = "ScriptableObjects/MasterInfo", order = 0)]
public class MasterInfo : ScriptableObject
{
    [SerializeField] float balance;
    [SerializeField] TurretInfo[] buildingsSet;

    public float Balance { get => balance; set => balance = value; }
    public TurretInfo[] GetBuildingsSet() { return buildingsSet; }
}