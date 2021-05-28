using System.Collections.Generic;
using UnityEngine;

public interface ITargetsDetector
{
	List<Transform> GetTargets(float range, LayerMask targetsLayer);
}