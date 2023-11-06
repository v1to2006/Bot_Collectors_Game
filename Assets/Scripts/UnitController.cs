using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Vector3 _resourcePosition;
    private BaseController _baseController;
    private float _movementSpeed = 5f;
    private bool _collected = false;

    public void CollectResource(Vector3 position, BaseController baseController)
    {
        _resourcePosition = position;
        _baseController = baseController;
    }

    private void Update()
    {
        if (!_collected)
        {
            MoveTowardsResource();
        }
    }

    private void MoveTowardsResource()
    {
        Vector3 direction = (_resourcePosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _resourcePosition);

        transform.Translate(direction * _movementSpeed * Time.deltaTime);

        if (distance < 0.1f)
        {
            CollectResource();
        }
    }

    private void CollectResource()
    {
        _collected = true;
        _baseController.ResourceCollected();
        Destroy(gameObject);
    }
}
