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
        switchablesInstaller = transform.parent.GetComponentInChildren<ISwitchablesInstaller>();
        detectorsInstaller = transform.parent.GetComponentInChildren<IDetectorsInstaller>();
        switchDetectableInstaller = transform.parent.GetComponentInChildren<ISwitchDetectableInstaller>();
        switchables = transform.parent.GetComponentsInChildren<IDayNightSwitchable>();
        dayDetectors = transform.parent.GetComponentsInChildren<IDayDetector>();
        nightDetectors = transform.parent.GetComponentsInChildren<INightDetector>();
        switchDetectable = transform.parent.GetComponentInChildren<ISwitchDetectable>();
    }

    private void setDependencies()
    {
        switchablesInstaller.SetSwitchableObjects(switchables);
        detectorsInstaller.SetDayDetectors(dayDetectors);
        detectorsInstaller.SetNightDetectors(nightDetectors);
        switchDetectableInstaller.SetSwitchDetectable(switchDetectable);
    }
}
