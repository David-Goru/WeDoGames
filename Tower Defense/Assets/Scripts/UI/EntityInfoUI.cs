using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class EntityInfoUI : MonoBehaviour
{
    public static EntityInfoUI Instance;

    void Start()
    {
        Instance = this;
        enabled = false;
    }

    void Update()
    {
        // If enabled and click something, disable
    }
}