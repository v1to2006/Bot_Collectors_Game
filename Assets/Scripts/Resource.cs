using UnityEngine;

public class Resource : MonoBehaviour
{
	public bool IsTarget { get; private set; } = false;

	public void SetIsTarget(bool value)
	{
		IsTarget = value;
	}
}
