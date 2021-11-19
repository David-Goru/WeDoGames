using UnityEngine;

public class DayNightInstaller : MonoBehaviour, ILoadable
{
    private ISwitchablesInstaller switchablesInstaller;
    private IDetectorsInstaller detectorsInstaller;
    private ISwitchDetectableInstaller switchDetectableInstaller;

    private IDayNightSwitchable[] switchables;
    private IDayDetector[] dayDetectors;
    private INightDetector[] nightDetectors;
    private ISwitchDetectable switchDetectable;

    public void Create()
    {
        getDependencies();
        setDependencies();
    }

    private void getDependencies()
    {
        Transform parent = transform.parent;
        switchablesInstaller = parent.GetComponentInChildren<ISwitchablesInstaller>();
        detectorsInstaller = parent.GetComponentInChildren<IDetectorsInstaller>();
        switchDetectableInstaller = parent.GetComponentInChildren<ISwitchDetectableInstaller>();
        switchables = parent.GetComponentsInChildren<IDayNightSwitchable>();
        dayDetectors = parent.GetComponentsInChildren<IDayDetector>();
        nightDetectors = parent.GetComponentsInChildren<INightDetector>();
        switchDetectable = parent.GetComponentInChildren<ISwitchDetectable>();
    }

    private void setDependencies()
    {
        switchablesInstaller.SetSwitchableObjects(switchables);
        detectorsInstaller.SetDayDetectors(dayDetectors);
        detectorsInstaller.SetNightDetectors(nightDetectors);
        switchDetectableInstaller.SetSwitchDetectable(switchDetectable);
    }
}