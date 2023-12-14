using UnityEngine;

[RequireComponent(typeof(MouseFollow))]
public class FlagPlacement : MonoBehaviour
{
    private Flag _flag;

    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private FlagStatus _flagSelector;
    [SerializeField] private GroundPointerClick _groundPointerClick;
    [SerializeField] private BasePointerClick _basePointerClick;

    private Flag _selectedFlag;
    private MouseFollow _mouseFollow;
    private Coroutine _mouseFollowCoroutine;

    private void Awake()
    {
        _mouseFollow = GetComponent<MouseFollow>();

        _groundPointerClick.GroundClicked += TryPlaceFlag;
        _basePointerClick.BaseClicked += TrySelectFlag;
    }

    public Flag Flag() => _flag;

    public void ResetFlag()
    {
        _flag = null;
    }

    private void TrySelectFlag()
    {
        if (_selectedFlag != null && _flagSelector.FlagSelected)
        {
            DeleteFlag();
        }
        else if (_flagSelector.FlagSelected == false)
        {
            SelectFlag();
        }
    }

    private void TryPlaceFlag()
    {
        if (_selectedFlag != null && _flagSelector.FlagSelected)
        {
            StopCoroutine(_mouseFollowCoroutine);
            if (_flag != null)
                Destroy(_flag.gameObject);

            _flag = Instantiate(_flagPrefab, _groundPointerClick.ClickPosition, Quaternion.identity);
            Destroy(_selectedFlag.gameObject);

            _flagSelector.UnSelectFlag();
        }
    }

    private void DeleteFlag()
    {
        StopCoroutine(_mouseFollowCoroutine);
        Destroy(_selectedFlag.gameObject);
        _flagSelector.UnSelectFlag();
    }

    private void SelectFlag()
    {
        _selectedFlag = Instantiate(_flagPrefab);
        _mouseFollowCoroutine = StartCoroutine(_mouseFollow.FollowMouse(_selectedFlag.transform));
        _flagSelector.SelectFlag();
    }
}
