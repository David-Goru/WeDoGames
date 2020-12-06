using System.Collections.Generic;
using UnityEngine;

public interface ITargetsDetector
{
	LayerMask TargetLayer { get; set; }
	float Range { get; set; }

	List<Transform> GetTargets();
}