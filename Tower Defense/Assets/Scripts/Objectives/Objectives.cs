using UnityEngine;

[CreateAssetMenu(fileName = "Objectives Order", menuName = "Objectives/Objectives Order", order = 0)]
public class Objectives : ScriptableObject
{
    [SerializeField, Tooltip("One array of objectives per wave")] ObjectivesPerWave[] objectivesOrder;

    public ObjectivesPerWave[] ObjectivesOrder { get => objectivesOrder; }
}

[System.Serializable]
public class ObjectivesPerWave
{
    [SerializeField] Objective[] objectives;

    public Objective[] Objectives { get => objectives; set => objectives = value; }
}