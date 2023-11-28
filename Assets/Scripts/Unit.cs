using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class Unit : MonoBehaviour
{
	private Base _mainBase;
	private Resource _targetResource;
	private Flag _targetFlag;
	private UnitController _unitController;

	private void Awake()
	{
		_unitController = GetComponent<UnitController>();
	}

	public void StartDelivery(Base mainBase, Resource resource)
	{
		_mainBase = mainBase;
		_targetResource = resource;

		GoForResource();
	}

	public void StartBaseBuilding(Base mainBase, Flag flag)
	{
		_mainBase = mainBase;
		_targetFlag = flag;

		GoToFlag();
	}

	private void GoToFlag()
	{
		StartCoroutine(_unitController.MoveToTarget(_targetFlag.transform, () =>
		{
			BuildBase();
		}));
	}

	private void BuildBase()
	{
		Base buildedBase = Instantiate(_mainBase, _targetFlag.transform.position, Quaternion.identity);
		Destroy(_targetFlag.gameObject);
		buildedBase.AddUnit(this);
	}

	private void GoForResource()
	{
		StartCoroutine(_unitController.MoveToTarget(_targetResource.transform, () =>
		{
			_unitController.PickUp(_targetResource);
			ReturnToBase();
		}));
	}

	private void ReturnToBase()
	{
		StartCoroutine(_unitController.MoveToTarget(_mainBase.transform, () =>
		{
			_mainBase.CollectResource(this, _targetResource);
		}));

	}
}