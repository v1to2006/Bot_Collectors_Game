using UnityEngine;

public class BaseController : MonoBehaviour
{
	[SerializeField] private Transform[] _unitSpawnPoints;
	[SerializeField] private Unit _unitPrefab;

	private int _resourcesCount = 0;

	private void Awake()
	{
		SpawnUnits();
	}

	private void SpawnUnits()
	{
		int requiredSpawnPointsCount = 3;

		for (int i = 0; i < _unitSpawnPoints.Length; i++)
		{
			if (_unitSpawnPoints.Length >= requiredSpawnPointsCount && _unitSpawnPoints != null)
			{
				Instantiate(_unitPrefab, _unitSpawnPoints[i].position, Quaternion.identity);
			}
			else
			{
				Debug.Log("Not enough spawnpoints or null");
			}
		}
	}

	private void UnitReturned()
	{
		_resourcesCount++;
	}
}
