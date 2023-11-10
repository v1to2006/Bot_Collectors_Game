using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;

	private Base _mainBase;
	private Resource _targetResource;
	private float _reachDistance = 1.5f;

	public void StartDelivery(Resource resource, Base mainBase)
	{
		_mainBase = mainBase;
		_targetResource = resource;

		GoForResource();
	}

	private void PickUp(Resource resource)
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

	private void GoForResource()
	{
		StartCoroutine(MoveToTarget(_targetResource.transform, () =>
		{
			PickUp(_targetResource);
			ReturnToBase();
		}));
	}

	private void ReturnToBase()
	{
		StartCoroutine(MoveToTarget(_mainBase.transform, () =>
		{
			_mainBase.CollectResource(this, _targetResource);
		}));

	}

	private IEnumerator MoveToTarget(Transform target, Action complete)
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
}