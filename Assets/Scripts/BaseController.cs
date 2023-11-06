using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    private List<UnitController> _activeUnits = new List<UnitController>();

    private int _availableResources = 0;
    private int _unitsCount = 3;

    void Start()
    {
        for (int i = 0; i < _unitsCount; i++)
        {
            InstantiateUnit();
        }
    }

    private void Update()
    {
        ScanForResources();
    }

    private void ScanForResources()
    {
        Resource[] resources = FindObjectsOfType<Resource>();

        if (resources.Length > 0)
        {
            SendUnitToCollect(resources[0].transform.position);
        }
    }

    private void SendUnitToCollect(Vector3 resourcePosition)
    {
        if (_activeUnits.Count > 0)
        {
            UnitController unitController = _activeUnits[0];
            unitController.CollectResource(resourcePosition, this);
            _activeUnits.RemoveAt(0);
            _availableResources++;
        }
    }

    public void ResourceCollected()
    {
        _availableResources++;
    }

    private void InstantiateUnit()
    {
        Unit unit = Instantiate(_unitPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        UnitController unitController = unit.GetComponent<UnitController>();
        _activeUnits.Add(unitController);
    }
}
