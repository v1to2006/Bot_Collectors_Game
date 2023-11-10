using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private Resource _resourcePrefab;
	[SerializeField] private float _spawnDelay;

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
			Instantiate(_resourcePrefab, randomPosition, Quaternion.identity);

			yield return delay;
		}
	}

	private Vector3 GenerateRandomPositionExcludingRange(float rangeMin, float rangeMax)
	{
		Vector3 randomPosition = new Vector3(0, 0, 0);

		float minGenerateRange = -40f;
		float maxGenerateRange = 40f;

		while (randomPosition.x > rangeMin && randomPosition.x < rangeMax && randomPosition.z > rangeMin && randomPosition.z < rangeMax)
		{
			randomPosition = new Vector3(Random.Range(minGenerateRange, maxGenerateRange), 0f, Random.Range(minGenerateRange, maxGenerateRange));
		}

		return randomPosition;
	}
}