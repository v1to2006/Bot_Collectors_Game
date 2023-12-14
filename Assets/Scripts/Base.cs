using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private FlagPlacement _flagSelection;

    private Queue<Unit> _activeUnits;

    private int _startUnitsCount = 3;
    private int _resourcesCount = 0;
    private int _unitPrice = 3;
    private int _baseBuildPrice = 5;

    private Coroutine _unitsSendCoroutine;

    private void Awake()
    {
        _activeUnits = new Queue<Unit>();
    }

    private void Start()
    {
        for (int i = 0; i < _startUnitsCount; i++)
        {
            _activeUnits.Enqueue(_unitSpawner.CreateUnit());
        }

        _unitsSendCoroutine = StartCoroutine(SendUnitsForResources());
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
        if (_flagSelection.Flag() == null && _resourcesCount >= _unitPrice)
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
        _activeUnits.Enqueue(_unitSpawner.CreateUnit());
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
        if (_activeUnits.Count > 0 && _resourceStorage.GetResourcesCount() > 0)
        {
            _activeUnits.Dequeue().StartDelivery(this, _resourceStorage.GetResource());
        }
    }

    private void SendUnitToFlag()
    {
        _activeUnits.Dequeue().StartBaseBuilding(this, _flagSelection.Flag());
        _resourcesCount -= _baseBuildPrice;
        _flagSelection.ResetFlag();
    }

    private void OnDestroy()
    {
        StopCoroutine(_unitsSendCoroutine);
    }
}