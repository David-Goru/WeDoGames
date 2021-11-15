using System;
using UnityEngine;

public class SwitchDetector : MonoBehaviour, ISwitchDetectableInstaller, IDetectorsInstaller
{
    private ISwitchDetectable switchDetectable;
    private IDayDetector[] dayDetectors;
    private INightDetector[] nightDetectors;
 
    private Action dayAction;
    private Action nightAction;

    public void SetSwitchDetectable(ISwitchDetectable switchDetectable)
    {
        this.switchDetectable = switchDetectable;
    }

    public void SetDayDetectors(IDayDetector[] dayDetectors)
    {
        this.dayDetectors = dayDetectors;
        if(dayAction == null) dayAction = onDayDetected;
        foreach (var dayDetector in dayDetectors) dayDetector.SetDayDetectionEvent(dayAction);
    }

    public void SetNightDetectors(INightDetector[] nightDetectors)
    {
        this.nightDetectors = nightDetectors;
        if (nightAction == null) nightAction = onNightDetected;
        foreach (var nightDetector in nightDetectors) nightDetector.SetNightDetectionEvent(nightAction);
    }

    private void onNightDetected()
    {
        switchDetectable.OnNightDetection();
    }

    private void onDayDetected()
    {
        switchDetectable.OnDayDetection();
    }
}
