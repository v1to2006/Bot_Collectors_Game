using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MouseFollow))]
public class Base : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private Flag _flagPrefab;
	[SerializeField] private Ground _ground;
	[SerializeField] private ResourceScanner _resourceScanner;
	[SerializeField] private SpawnPoint[] _unitSpawnPoints;

	private MouseFollow _mouseFollow;
	private Queue<Unit> _activeUnits;
	private Flag _flag;
	private Coroutine _mouseFollowCoroutine;

	private bool _flagSelected;

	private int _resourcesCount = 0;
	private int _startUnitsCount = 3;
	private int _unitSpawnPointIndex = 0;
	private int _unitPrice = 3;
	private int _baseBuildPrice = 5;

	public void Awake()
	{
		_activeUnits = new Queue<Unit>();
		_mouseFollow = GetComponent<MouseFollow>();

		_ground.Clicked += TryPlaceFlag;

		for (int i = 0; i < _startUnitsCount; i++)
		{
			CreateUnit();
		}

		StartCoroutine(SendUnitsForResources());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_flag != null)
		{
			StopCoroutine(_mouseFollowCoroutine);
			Destroy(_flag.gameObject);
			_flagSelected = false;
		}
		else
		{
			_flag = Instantiate(_flagPrefab);
			_mouseFollowCoroutine = StartCoroutine(_mouseFollow.FollowMouse(_flag.transform));
			_flagSelected = true;
		}
	}

	public void TryPlaceFlag()
	{
		if (_flag != null && _flagSelected)
		{
			StopCoroutine(_mouseFollowCoroutine);
			_flag.transform.position = _ground.HitPosition;
			_flagSelected = false;
		}
	}

	public void CollectResource(Unit unit, Resource deliveredResource)
	{
		Destroy(deliveredResource.gameObject);

		_resourcesCount++;

		_activeUnits.Enqueue(unit);

		MakeNewStep();
	}

	public void AddUnit(Unit unit)
	{
		_activeUnits.Enqueue(unit);
	}

	private void MakeNewStep()
	{
		if (_flag == null && _resourcesCount >= _unitPrice)
		{
			BuyNewUnit();
		}
		else if (_resourcesCount >= _baseBuildPrice && _activeUnits.Count > 0)
		{
			SendUnitToFlag();
		}
	}

	private void BuyNewUnit()
	{
		CreateUnit();

		_resourcesCount -= _unitPrice;
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
			_activeUnits.Dequeue().StartDelivery(this, _resourceScanner.GetResource());
		}
	}

	private void SendUnitToFlag()
	{
		_activeUnits.Dequeue().StartBaseBuilding(this, _flag);

		_resourcesCount -= _baseBuildPrice;
		_flag = null;
	}

	private void CreateUnit()
	{
		Unit unit = Instantiate(_unitPrefab,
			_unitSpawnPoints[_unitSpawnPointIndex++ % _unitSpawnPoints.Length].transform.position,
			Quaternion.identity);

		_activeUnits.Enqueue(unit);
	}
}