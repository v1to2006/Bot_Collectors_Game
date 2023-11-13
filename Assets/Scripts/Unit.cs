using UnityEngine;

[RequireComponent(typeof(UnitMovement))]
public class Unit : MonoBehaviour
{
	private Base _mainBase;
	private Resource _targetResource;
	private UnitMovement _unitController;

	private void Awake()
	{
		_unitController = GetComponent<UnitMovement>();
	}

	public void StartDelivery(Resource resource, Base mainBase)
	{
		_mainBase = mainBase;
		_targetResource = resource;

		GoForResource();
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