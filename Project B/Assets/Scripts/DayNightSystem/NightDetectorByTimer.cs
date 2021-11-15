using System;
using UnityEngine;

public class NightDetectorByTimer : MonoBehaviour, INightDetector, IDayNightSwitchable
{
    [SerializeField] private float dayTime = 60f;

    private Action nightEvent;
    private float timer = 0f;
    private bool isActive;

    private void Update()
    {
        if (isActive) 
        {
            if (timer >= dayTime)
            {
                nightEvent.Invoke();
                isActive = false;
            }
            else timer += Time.deltaTime;
        }
    }

    public void OnDayStart()
    {
        timer = 0f;
        isActive = true;
    }

    public void OnNightStart()
    {
        isActive = false;
    }

    public void SetNightDetectionEvent(Action nightEvent)
    {
        this.nightEvent = nightEvent;
    }
}
