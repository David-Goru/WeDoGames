using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLightSystem : MonoBehaviour, IDayNightSwitchable
{
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private Light myLight;

    public void OnDayStart()
    {
        myLight.color = dayColor;
    }

    public void OnNightStart()
    {
        myLight.color = nightColor;
    }
}
