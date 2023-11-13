using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private ResourceScanner _resourceScanner;
	[SerializeField] private SpawnPoint[] _unitSpawnPoints;

	private Queue<Unit> _activeUnits;

	private int _startUnitsCount = 3;
	private int _unitSpawnPointIndex = 0;
	private int _resourcesCount = 0;

	private void Awake()
	{
		_activeUnits = new Queue<Unit>();

		for (int i = 0; i < _startUnitsCount; i++)
		{
			SpawnUnit();
		}

		StartCoroutine(SendUnits());
	}

	public void CollectResource(Unit unit, Resource deliveredResource)
	{
		Destroy(deliveredResource.gameObject);

		_resourcesCount++;

		_activeUnits.Enqueue(unit);

		Debug.Log($"Resources collected: {_resourcesCount}");
	}

	private IEnumerator SendUnits()
	{
		float delayTime = 0.1f;
		WaitForSeconds delay = new WaitForSeconds(delayTime);

		while (true)
		{
			SendUnit();

			yield return delay;
		}
	}

	private void SendUnit()
	{
		if (_activeUnits.Count > 0 && _resourceScanner.GetResourcesCount() > 0)
		{
			Resource resource = _resourceScanner.GetResource();
			_activeUnits.Dequeue().StartDelivery(resource, this);
		}
	}

	private void SpawnUnit()
	{
		Unit unit = Instantiate(_unitPrefab, _unitSpawnPoints[_unitSpawnPointIndex++].transform.position, Quaternion.identity);
		_activeUnits.Enqueue(unit);
	}
}