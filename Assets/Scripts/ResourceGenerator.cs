using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private Resource _resourcePrefab;
	[SerializeField] private ResourceScanner _resourceScanner;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private float _minSpawnRange;
	[SerializeField] private float _maxSpawnRange;

	private void Awake()
	{
		StartCoroutine(SpawnResources());
	}

	private IEnumerator SpawnResources()
	{
		WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

		float minBaseXPosition = -5f;
		float maxBaseXPosition = 5f;

		while (true)
		{
			Vector3 randomPosition = GenerateRandomPositionExcludingRange(minBaseXPosition, maxBaseXPosition);
			_resourceScanner.AddResource(Instantiate(_resourcePrefab, randomPosition, Quaternion.identity, transform));

			yield return delay;
		}
	}

	private Vector3 GenerateRandomPositionExcludingRange(float rangeMin, float rangeMax)
	{
		Vector3 randomPosition = new Vector3(0, 0, 0);

		while (randomPosition.x > rangeMin && randomPosition.x < rangeMax && randomPosition.z > rangeMin && randomPosition.z < rangeMax)
		{
			randomPosition = new Vector3(Random.Range(_minSpawnRange, _maxSpawnRange), 0f, Random.Range(_minSpawnRange, _maxSpawnRange));
		}

		return randomPosition;
	}
}