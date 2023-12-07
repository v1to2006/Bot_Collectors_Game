using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField] private ResourceScanner _resourceScanner;
	[SerializeField] private UnitSpawner _unitSpawner;
	[SerializeField] private FlagManager _flagManager;

	private Queue<Unit> _activeUnits;

	private int _startUnitsCount = 3;
	private int _resourcesCount = 0;
	private int _unitPrice = 3;
	private int _baseBuildPrice = 5;

	private void Awake()
	{
		_activeUnits = new Queue<Unit>();
	}

	private void Start()
	{
		for (int i = 0; i < _startUnitsCount; i++)
		{
			_activeUnits.Enqueue(_unitSpawner.CreateUnit());
		}

		StartCoroutine(SendUnitsForResources());
	}

	public void CollectResource(Unit unit, Resource deliveredResource)
	{
		Destroy(deliveredResource.gameObject);
		_resourcesCount++;
		_activeUnits.Enqueue(unit);
		MakeNewStep();
	}

	public void AddUnit(Unit unit)
	{
		_activeUnits.Enqueue(unit);
	}

	private void MakeNewStep()
	{
		if (_flagManager.Flag == null && _resourcesCount >= _unitPrice)
		{
			BuyNewUnit();
		}
		else if (_resourcesCount >= _baseBuildPrice && _activeUnits.Count > 0)
		{
			SendUnitToFlag();
		}
	}

	private void BuyNewUnit()
	{
		_activeUnits.Enqueue(_unitSpawner.CreateUnit());
		_resourcesCount -= _unitPrice;
	}

	private IEnumerator SendUnitsForResources()
	{
		while (true)
		{
			SendUnitToCollectResource();
			yield return null;
		}
	}

	private void SendUnitToCollectResource()
	{
		if (_activeUnits.Count > 0 && _resourceScanner.GetResourcesCount() > 0)
		{
			_activeUnits.Dequeue().StartDelivery(this, _resourceScanner.GetResource());
		}
	}

	private void SendUnitToFlag()
	{
		_activeUnits.Dequeue().StartBaseBuilding(this, _flagManager.Flag);
		_resourcesCount -= _baseBuildPrice;
		_flagManager.ResetFlag();
	}
}