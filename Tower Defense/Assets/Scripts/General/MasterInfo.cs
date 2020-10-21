using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterInfo", menuName = "ScriptableObjects/MasterInfo", order = 0)]
public class MasterInfo : ScriptableObject
{
    [SerializeField] string[] buildingsSet;

    public string[] GetBuildingsSet() { return buildingsSet; }
}