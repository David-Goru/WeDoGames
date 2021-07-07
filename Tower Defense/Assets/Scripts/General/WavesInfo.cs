using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesInfo", menuName = "ScriptableObjects/WavesInfo", order = 0)]
public class WavesInfo : ScriptableObject
{
    [SerializeField] List<EnemyList> enemyWaves;

    public List<EnemyList> EnemyWaves { get => enemyWaves; set => enemyWaves = value; }
}