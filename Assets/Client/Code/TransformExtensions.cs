using UnityEngine;

public static class TransformExtensions
{
	public static bool IsNear(this Transform t, Vector3 target, float distance)
	{
		return (t.position - target).sqrMagnitude <= distance * distance;
	}
}