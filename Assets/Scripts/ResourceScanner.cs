using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
	private readonly Queue<Resource> _scannedResources = new Queue<Resource>();

	public Resource GetResource()
	{
		return _scannedResources.Dequeue();
	}

	public int GetResourcesCount()
	{
		return _scannedResources.Count;
	}

	public void AddResource(Resource resource)
	{
		_scannedResources.Enqueue(resource);
	}
}