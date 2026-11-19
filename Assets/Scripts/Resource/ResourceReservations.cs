using System.Collections.Generic;
using UnityEngine;

public class ResourceReservations : MonoBehaviour
{
    private readonly HashSet<Resource> _reservedResources = new HashSet<Resource>();

    public bool TryReserve(Resource resource)
    {
        if (resource == null || 
            resource.gameObject.activeInHierarchy == false)
        {
            return false;
        }

        return _reservedResources.Add(resource);
    }

    public void Release(Resource resource)
    {
        _reservedResources.Remove(resource);
    }
}