using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _minSpawnRange;
    [SerializeField] private float _maxSpawnRange;

    private Coroutine _spawnResourcesCoroutine;

    private void Awake()
    {
        _spawnResourcesCoroutine = StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_spawnDelay);

        while (true)
        {
            Instantiate(_resourcePrefab,
                GenerateRandomPosition(),
                Quaternion.identity,
                transform);

            yield return delay;
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        return new Vector3(Random.Range(_minSpawnRange, _maxSpawnRange), 0f, Random.Range(_minSpawnRange, _maxSpawnRange));
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawnResourcesCoroutine);
    }
}