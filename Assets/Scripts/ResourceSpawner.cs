using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
	[SerializeField] private Resource _resourcePrefab;

	private Transform[] _spawnPoints;

	private void Awake()
	{
		_spawnPoints = new Transform[transform.childCount];

		for (int i = 0; i < _spawnPoints.Length - 1; i++)
		{
			_spawnPoints[i] = transform.GetChild(i).GetComponent<Transform>();
		}

		StartCoroutine(SpawnResources());
	}

	private IEnumerator SpawnResources()
	{
		float delayTime = 10f;
		WaitForSeconds delay = new WaitForSeconds(delayTime);

		while (true)
		{
			Instantiate(_resourcePrefab,
				_spawnPoints[Random.Range(0, _spawnPoints.Length - 1)].transform.position,
				Quaternion.identity);

			yield return delay;
		}
	}
}