using System;
using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	public static event Action<Resource> ResourceSpawned;

	[SerializeField] private Resource resourceTemplate;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private float _minSpawnRange;
	[SerializeField] private float _maxSpawnRange;

	private Coroutine _spawnResourcesCoroutine;

	private void Start()
	{
		_spawnResourcesCoroutine = StartCoroutine(SpawnResources());
	}

	private IEnumerator SpawnResources()
	{
		WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

		while (true)
		{
			Resource resource = Instantiate(resourceTemplate,
				GenerateRandomPosition(),
				Quaternion.identity,
				transform);

			ResourceSpawned?.Invoke(resource);

			yield return delay;
		}
	}

	private Vector3 GenerateRandomPosition()
	{
		return new Vector3(UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange), 0f, UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange));
	}

	private void OnDestroy()
	{
		if (_spawnResourcesCoroutine != null)
			StopCoroutine(_spawnResourcesCoroutine);
	}
}