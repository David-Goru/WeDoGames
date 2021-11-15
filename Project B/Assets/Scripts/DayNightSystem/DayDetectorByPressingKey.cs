using System;
using UnityEngine;

public class DayDetectorByPressingKey : MonoBehaviour, IDayDetector, IDayNightSwitchable
{
    [SerializeField] private KeyCode key = KeyCode.Q;

    private Action dayEvent;

    private bool isActive;

    private void Update()
    {
        if (isActive && Input.GetKeyDown(key)) dayEvent.Invoke();
    }

    public void OnDayStart()
    {
        isActive = false;
    }

    public void OnNightStart()
    {
        isActive = true;
    }

    public void SetDayDetectionEvent(Action dayEvent)
    {
        this.dayEvent = dayEvent;
    }
}
