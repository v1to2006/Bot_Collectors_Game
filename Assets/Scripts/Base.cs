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
    [SerializeField] private FlagSelector _flagSelector;
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private SpawnPoint[] _unitSpawnPoints;

    private MouseFollow _mouseFollow;
    private Queue<Unit> _activeUnits;
    private Coroutine _mouseFollowCoroutine;

    private Flag _flag;
    private Flag _selectedFlag;

    private int _resourcesCount = 0;
    private int _startUnitsCount = 3;
    private int _unitSpawnPointIndex = 0;
    private int _unitPrice = 3;
    private int _baseBuildPrice = 5;

    private void Awake()
    {
        _activeUnits = new Queue<Unit>();
        _mouseFollow = GetComponent<MouseFollow>();

        _ground.GroundClicked += TryPlaceFlag;

        for (int i = 0; i < _startUnitsCount; i++)
        {
            CreateUnit();
        }

        StartCoroutine(SendUnitsForResources());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_selectedFlag != null && _flagSelector.FlagSelected)
        {
            StopCoroutine(_mouseFollowCoroutine);
            Destroy(_selectedFlag.gameObject);
            _flagSelector.SetFlagStatus(false);
        }
        else if (_flagSelector.FlagSelected == false)
        {
            _selectedFlag = Instantiate(_flagPrefab);
            _mouseFollowCoroutine = StartCoroutine(_mouseFollow.FollowMouse(_selectedFlag.transform));
            _flagSelector.SetFlagStatus(true);
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

    private void TryPlaceFlag()
    {
        if (_selectedFlag != null && _flagSelector.FlagSelected)
        {
            StopCoroutine(_mouseFollowCoroutine);

            if (_flag != null)
                Destroy(_flag.gameObject);

            _flag = Instantiate(_flagPrefab, _ground.HitPosition, Quaternion.identity);
            Destroy(_selectedFlag.gameObject);

            _flagSelector.SetFlagStatus(false);
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
