using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitController : MonoBehaviour
{
	[SerializeField] private Base _basePoint;

	private Rigidbody _rigidbody;
	private float _movementSpeed = 5f;

	private Transform _targetResource;
	private Transform _targetPoint;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void SetTargetPoint(Transform targetPoint)
	{
		_targetPoint = targetPoint;
	}

	private void MoveToTarget()
	{
		_rigidbody.Move(_targetPoint.position.normalized * _movementSpeed * Time.fixedDeltaTime, Quaternion.identity);

		_rigidbody.Move(_basePoint.transform.position.normalized * _movementSpeed * Time.fixedDeltaTime, Quaternion.identity);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.TryGetComponent<Resource>(out Resource resource))
		{

		}
	}
}
