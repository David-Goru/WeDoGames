using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesInfo", menuName = "ScriptableObjects/WavesInfo", order = 0)]
public class WavesInfo : ScriptableObject
{
    [SerializeField] float planningTime = 0;
    [SerializeField] List<EnemyList> enemyWaves;

    public float PlanningTime { get => planningTime; set => planningTime = value; }
    public List<EnemyList> EnemyWaves { get => enemyWaves; set => enemyWaves = value; }
}