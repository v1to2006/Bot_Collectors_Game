using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _radius;

    public Queue<Resource> ScanResources()
    {
        Queue<Resource> scannedResources = new Queue<Resource>();

        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Resource>(out Resource resource))
            {
                if (!scannedResources.Contains(resource))
                {
                    scannedResources.Enqueue(resource);
                }
            }
        }

        return scannedResources;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }
}