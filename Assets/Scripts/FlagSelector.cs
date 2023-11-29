using UnityEngine;

public class FlagSelector : MonoBehaviour
{
    public bool FlagSelected { get; private set; } = false;

    public void SetFlagStatus(bool value)
    {
        FlagSelected = value;
    }
}
