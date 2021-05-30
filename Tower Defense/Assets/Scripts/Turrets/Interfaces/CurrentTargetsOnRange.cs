using System.Collections.Generic;
using UnityEngine;

public abstract class CurrentTargetsOnRange : EffectComponent
{
    public abstract List<Transform> CurrentTargets { get; }
}

