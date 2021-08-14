using UnityEngine;

[CreateAssetMenu(fileName = "Objectives Order", menuName = "Objectives/Objectives Order", order = 0)]
public class Objectives : ScriptableObject
{
    [SerializeField] Objective[] levelObjectives = null;

    public Objective[] LevelObjectives { get => levelObjectives; }
}