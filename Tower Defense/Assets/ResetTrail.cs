using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrail : MonoBehaviour
{
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        trail.Clear();
    }
}
