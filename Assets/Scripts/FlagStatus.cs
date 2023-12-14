using UnityEngine;

public class FlagStatus : MonoBehaviour
{
    public bool FlagSelected { get; private set; } = false;

    public void SelectFlag()
    {
        FlagSelected = true;
    }

    public void DeleteFlag()
    {
        FlagSelected = false;
    }
}
