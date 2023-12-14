using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;

    private Queue<Resource> _scannedResources;

    private void Awake()
    {
        _scannedResources = new Queue<Resource>();
    }

    public Resource GetResource()
    {
        return _scannedResources.Peek();
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
