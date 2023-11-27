using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MouseFollow))]
public class Base : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private Flag _flagPrefab;
	[SerializeField] private ResourceScanner _resourceScanner;
	[SerializeField] private SpawnPoint[] _unitSpawnPoints;

	private MouseFollow _mouseFollower;
	private Queue<Unit> _activeUnits;
	private Flag _flag;
	private Coroutine _mouseFollowCoroutine;

	private bool _flagSelected = false;

	private int _resourcesCount = 0;
	private int _startUnitsCount = 3;
	private int _unitSpawnPointIndex = 0;
	private int _unitPrice = 3;
	private int _baseBuildPrice = 5;

	private void Awake()
	{
		_activeUnits = new Queue<Unit>();
		_mouseFollower = GetComponent<MouseFollow>();

		for (int i = 0; i < _startUnitsCount; i++)
		{
			CreateUnit();
		}

		StartCoroutine(SendUnitsForResources());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_flagSelected)
		{
			_flag = Instantiate(_flagPrefab);
			_mouseFollowCoroutine = StartCoroutine(_mouseFollower.FollowMouse(_flag.transform));
			_flagSelected = true;
		}
		else
		{
			StopCoroutine(_mouseFollowCoroutine);
			Destroy(_flag.gameObject);
			_flag = null;
			_flagSelected = false;
		}
	}

	public void CollectResource(Unit unit, Resource deliveredResource)
	{
		Destroy(deliveredResource.gameObject);

		_resourcesCount++;

		_activeUnits.Enqueue(unit);

		TryBuyNewUnit();
	}

	private void TryBuyNewUnit()
	{
		if (_resourcesCount >= _unitPrice)
		{
			CreateUnit();

			_resourcesCount -= _unitPrice;
		}
	}

	private void SendUnitBuildBase()
	{

	}

	private IEnumerator SendUnitsForResources()
	{
		while (true)
		{
			SendUnitToCollectResource();

			yield return null;
		}
	}

	private void SendUnitToCollectResource()
	{
		if (_activeUnits.Count > 0 && _resourceScanner.GetResourcesCount() > 0)
		{
			Resource resource = _resourceScanner.GetResource();
			_activeUnits.Dequeue().StartDelivery(resource, this);
		}
	}

	private void CreateUnit()
	{
		Unit unit = Instantiate(_unitPrefab, _unitSpawnPoints[_unitSpawnPointIndex++ % _unitSpawnPoints.Length].transform.position, Quaternion.identity);
		_activeUnits.Enqueue(unit);
	}
}