using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class SpecialEffect : MonoBehaviour, ITurretBehaviour
{
    [SerializeField] List<EffectComponent> components = null;
    public void InitializeBehaviour()
    {
        foreach (EffectComponent component in components)
        {
            component.InitializeComponent();
        }
    }

    public void UpdateBehaviour()
    {
        foreach (EffectComponent component in components)
        {
            component.UpdateComponent();
        }
    }
}
