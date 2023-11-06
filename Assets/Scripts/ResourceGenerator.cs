using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _delayTime;

    private void Awake()
    {
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayTime);

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
        Vector3 randomPosition;

        float minGenerateRange = -20f;
        float maxGenerateRange = 20f;

        do
        {
            randomPosition = new Vector3(Random.Range(minGenerateRange, maxGenerateRange), 0.5f, Random.Range(minGenerateRange, maxGenerateRange));
        } while (randomPosition.x > rangeMin && randomPosition.x < rangeMax && randomPosition.z > rangeMin && randomPosition.z < rangeMax);

        return randomPosition;
    }
}
