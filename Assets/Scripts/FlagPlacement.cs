using UnityEngine;

[RequireComponent(typeof(MouseFollow))]
public class FlagPlacement : MonoBehaviour
{
	private Flag _flag;

	[SerializeField] private Flag _flagTemplate;
	[SerializeField] private FlagStatus _flagStatus;
	[SerializeField] private GroundPointerClick _groundPointerClick;
	[SerializeField] private BasePointerClick _basePointerClick;

	private Flag _selectedFlag;
	private MouseFollow _mouseFollow;
	private Coroutine _mouseFollowCoroutine;

	public Flag Flag
	{
		get { return _flag; }
	}

	private void Awake()
	{
		_mouseFollow = GetComponent<MouseFollow>();

		_groundPointerClick.GroundClicked += TryPlaceFlag;
		_basePointerClick.BaseClicked += TrySelectFlag;
	}

	public void ResetFlag()
	{
		_flag = null;
	}

	private void TrySelectFlag()
	{
		if (_selectedFlag != null && _flagStatus.FlagSelected)
		{
			DeleteFlag();
		}
		else if (_flagStatus.FlagSelected == false)
		{
			SelectFlag();
		}
	}

	private void TryPlaceFlag()
	{
		if (_selectedFlag != null && _flagStatus.FlagSelected)
		{
			StopCoroutine(_mouseFollowCoroutine);
			if (_flag != null)
				Destroy(_flag.gameObject);

			_flag = Instantiate(_flagTemplate, _groundPointerClick.ClickPosition, Quaternion.identity);
			Destroy(_selectedFlag.gameObject);

			_flagStatus.DeleteFlag();
		}
	}

	private void DeleteFlag()
	{
		StopCoroutine(_mouseFollowCoroutine);
		Destroy(_selectedFlag.gameObject);
		_flagStatus.DeleteFlag();
	}

	private void SelectFlag()
	{
		_selectedFlag = Instantiate(_flagTemplate);
		_mouseFollowCoroutine = StartCoroutine(_mouseFollow.FollowMouse(_selectedFlag.transform));
		_flagStatus.SelectFlag();
	}
}
