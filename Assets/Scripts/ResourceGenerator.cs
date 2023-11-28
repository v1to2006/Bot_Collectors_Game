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

		while (true)
		{
			_resourceScanner.AddResource(Instantiate(_resourcePrefab,
				GenerateRandomPosition(),
				Quaternion.identity,
				transform));

			yield return delay;
		}
	}

	private Vector3 GenerateRandomPosition()
	{
		return new Vector3(Random.Range(_minSpawnRange, _maxSpawnRange), 0f, Random.Range(_minSpawnRange, _maxSpawnRange));
	}
}