using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
	private Queue<Resource> _scannedResources;

	private void Awake()
	{
		ResourceGenerator.ResourceSpawned += AddResource;

		_scannedResources = new Queue<Resource>();
	}

	public Resource GetResource()
	{
		return _scannedResources.Dequeue();
	}

	public int GetResourcesCount()
	{
		return _scannedResources.Count;
	}

	private void AddResource(Resource resource)
	{
		_scannedResources.Enqueue(resource);
	}
}
