using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
	[SerializeField] private Unit _unitPrefab;

	private SpawnPoint[] _unitSpawnPoints;
	private int _unitSpawnPointIndex = 0;

	private void Awake()
	{
		_unitSpawnPoints = new SpawnPoint[transform.childCount];

		for (int i = 0; i < _unitSpawnPoints.Length; i++)
		{
			_unitSpawnPoints[i] = transform.GetChild(i).GetComponent<SpawnPoint>();
		}
	}

	public Unit CreateUnit()
	{
		return Instantiate(_unitPrefab,
			_unitSpawnPoints[_unitSpawnPointIndex++ % _unitSpawnPoints.Length].transform.position,
			Quaternion.identity);
	}

}
