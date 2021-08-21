using UnityEngine;

[RequireComponent(typeof(SpecialEffect))]
public abstract class EffectComponent : MonoBehaviour
{
    public abstract void InitializeComponent();

    public abstract void UpdateComponent();
}