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

    private bool _flagSelected = false;

    private int _startUnitsCount = 3;
    private int _unitSpawnPointIndex = 0;
    private int _resourcesCount = 0;
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
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit) && !_flagSelected)
        {
            _flag = Instantiate(_flagPrefab, hit.point, Quaternion.identity);
            StartCoroutine(_mouseFollower.FollowMouse(_flag.transform));
            _flagSelected = true;
        }
    }

    public void CollectResource(Unit unit, Resource deliveredResource)
    {
        Destroy(deliveredResource.gameObject);

        _resourcesCount++;

        _activeUnits.Enqueue(unit);

        if (ShouldSendUnitToBuildBase())
        {
            SendUnitToBuildBase();
        }
        else if (CanAffordNewUnit())
        {
            BuyNewUnit();
        }
    }

    private bool CanAffordNewUnit()
    {
        return _resourcesCount >= _unitPrice;
    }

    private bool ShouldSendUnitToBuildBase()
    {
        return _resourcesCount >= _baseBuildPrice;
    }

    private void BuyNewUnit()
    {
        CreateUnit();

        _resourcesCount -= _unitPrice;
    }

    private void SendUnitToBuildBase()
    {

    }

    private void SetFlag()
    {

    }

    private IEnumerator SendUnitsForResources()
    {
        float delayTime = 0.1f;
        WaitForSeconds delay = new WaitForSeconds(delayTime);

        while (true)
        {
            SendUnitToCollectResource();

            yield return delay;
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