using UnityEngine;

[RequireComponent(typeof(UnitAction))]
public class Unit : MonoBehaviour
{
    private Base _mainBase;
    private Resource _targetResource;
    private Flag _targetFlag;
    private UnitAction _unitController;

    private Coroutine _goToFlagCoroutine;
    private Coroutine _goForResourcesCoroutine;
    private Coroutine _returnToBaseCoroutine;

    private void Awake()
    {
        _unitController = GetComponent<UnitAction>();
    }

    public void StartDelivery(Base mainBase, Resource resource)
    {
        _mainBase = mainBase;
        _targetResource = resource;

        GoForResource();
    }

    public void StartBaseBuilding(Base mainBase, Flag flag)
    {
        _mainBase = mainBase;
        _targetFlag = flag;

        GoToFlag();
    }

    private void GoToFlag()
    {
        _goToFlagCoroutine = StartCoroutine(_unitController.MoveToTarget(_targetFlag.transform, () =>
        {
            BuildBase();
            StopCoroutine(_goToFlagCoroutine);
        }));
    }

    private void BuildBase()
    {
        Base buildedBase = Instantiate(_mainBase, _targetFlag.transform.position, Quaternion.identity);
        Destroy(_targetFlag.gameObject);
        buildedBase.AddUnit(this);
    }

    private void GoForResource()
    {
        _goForResourcesCoroutine = StartCoroutine(_unitController.MoveToTarget(_targetResource.transform, () =>
        {
            _unitController.PickUp(_targetResource);
            ReturnToBase();
            StopCoroutine(_goForResourcesCoroutine);
        }));
    }

    private void ReturnToBase()
    {
        _returnToBaseCoroutine = StartCoroutine(_unitController.MoveToTarget(_mainBase.transform, () =>
        {
            _mainBase.CollectResource(this, _targetResource);
            StopCoroutine(_returnToBaseCoroutine);
        }));
    }
}