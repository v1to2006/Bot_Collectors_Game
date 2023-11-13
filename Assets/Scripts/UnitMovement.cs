using System;
using System.Collections;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;

	private float _reachDistance = 1f;

	public IEnumerator MoveToTarget(Transform target, Action complete)
	{
		Vector3 targetFixedVector3 = new Vector3(target.position.x, transform.position.y, target.position.z);

		while (Vector3.Distance(transform.position, targetFixedVector3) > _reachDistance)
		{
			targetFixedVector3 = new Vector3(target.position.x, transform.position.y, target.position.z);

			transform.position = Vector3.MoveTowards(transform.position, targetFixedVector3, _movementSpeed * Time.deltaTime);
			transform.LookAt(targetFixedVector3);

			yield return null;
		}

		complete?.Invoke();
	}

	public void PickUp(Resource resource)
	{
		Vector3 pickedResourcePosition = new Vector3(0, 0, 1);

		BoxCollider resourceBoxCollider = resource.GetComponent<BoxCollider>();
		Rigidbody resourceRigidbody = resource.GetComponent<Rigidbody>();

		if (resourceBoxCollider != null && resourceRigidbody != null)
		{
			resourceBoxCollider.enabled = false;
			resourceRigidbody.isKinematic = true;
			resource.transform.localPosition = pickedResourcePosition;
			resource.transform.SetParent(transform, false);
		}
	}
}
