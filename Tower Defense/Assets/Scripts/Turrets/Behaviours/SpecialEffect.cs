using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialEffect : MonoBehaviour, ITurretBehaviour
{
    List<EffectComponent> components = null;
    
    void Awake()
    {
        components = GetComponents<EffectComponent>().ToList();
    }

    public void InitializeBehaviour()
    {
        foreach (EffectComponent component in components) component.InitializeComponent();
    }

    public void UpdateBehaviour()
    {
        foreach (EffectComponent component in components) component.UpdateComponent();
    }
}
