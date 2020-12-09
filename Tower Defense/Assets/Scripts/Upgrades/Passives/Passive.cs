using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class Passive : ScriptableObject, IPerk
{
    [SerializeField] string title = "";
    //[SerializeField] Stat stat = null;

    public string Title { get => title; set => title = value; }
    //public float Stat { get => stat; set => stat = value; }
}