using UnityEngine;

public class FlagSelector : MonoBehaviour
{
	public bool FlagSelected { get; private set; }

	private void Awake()
	{
		FlagSelected = false;
	}


}
