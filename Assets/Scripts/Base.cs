using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private SpawnPoint[] _unitSpawnPoints;

	private Queue<Unit> _activeUnits;
	private List<Resource> _scannedResources;

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

		StartCoroutine(ScanForResources());
	}

	public void CollectResource(Unit unit, Resource deliveredResource)
	{
		Destroy(deliveredResource.gameObject);

		_resourcesCount++;

		_activeUnits.Enqueue(unit);

		Debug.Log($"Resources collected: {_resourcesCount}");
	}

	private IEnumerator ScanForResources()
	{
		float delayTime = 1f;
		WaitForSeconds scanDelay = new WaitForSeconds(delayTime);

		while (true)
		{
			_scannedResources = new List<Resource>(FindObjectsOfType<Resource>());

			if (_activeUnits.Count > 0 && _scannedResources.Count > 0)
			{
				foreach (Resource resource in _scannedResources)
				{
					if (resource.IsTarget == false)
					{
						SendUnit(resource);
						break;
					}
				}
			}

			yield return scanDelay;
		}
	}

	private void SendUnit(Resource resource)
	{
		resource.SetIsTarget(true);
		_activeUnits.Dequeue().StartDelivery(resource, this);
	}

	private void SpawnUnit()
	{
		Unit unit = Instantiate(_unitPrefab, _unitSpawnPoints[_unitSpawnPointIndex++].transform.position, Quaternion.identity);
		_activeUnits.Enqueue(unit);
	}
}
