using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/Pool", order = 1)]
public class Pool : ScriptableObject
{
    public GameObject prefab;
    public int size;
    public string tag;
}