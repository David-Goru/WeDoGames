using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSwitcher : MonoBehaviour
{
    IDayNightSwitchable[] dayNightSwitchables;

    void Awake()
    {
        dayNightSwitchables = transform.parent.GetComponentsInChildren<IDayNightSwitchable>();
    }

    void Start()
    {
        startDay();
    }

    void startDay()
    {
        foreach (var dayNightSystem in dayNightSwitchables) dayNightSystem.OnDayStart();
    }

    void startNight()
    {
        foreach (var dayNightSystem in dayNightSwitchables) dayNightSystem.OnNightStart();
    }
}