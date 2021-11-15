using UnityEngine;

public class DayNightSwitcher : MonoBehaviour, ISwitchablesInstaller, ISwitchDetectable
{
    private IDayNightSwitchable[] dayNightSwitchables;

    public void SetSwitchableObjects(IDayNightSwitchable[] switchables)
    {
        dayNightSwitchables = switchables;
        startDay();
    }

    public void OnDayDetection()
    {
        startDay();
        print("Day Started");
    }

    public void OnNightDetection()
    {
        startNight();
        print("Night Started");
    }

    private void startDay()
    {
        foreach (var switchableObject in dayNightSwitchables) switchableObject.OnDayStart();
    }

    private void startNight()
    {
        foreach (var switchableObject in dayNightSwitchables) switchableObject.OnNightStart();
    }
}