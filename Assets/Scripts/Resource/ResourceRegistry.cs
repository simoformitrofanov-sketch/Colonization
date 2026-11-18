using System.Collections.Generic;

public class ResourceRegistry
{
    private readonly Queue<Resource> _freeResources = new Queue<Resource>();
    private readonly List<Resource> _reservedResources = new List<Resource>();

    public void Register(Resource resource)
    {
        if (resource == null)
        {
            return;
        }

        if (_freeResources.Contains(resource) || _reservedResources.Contains(resource))
        {
            return;
        }

        _freeResources.Enqueue(resource);
    }

    public bool TryReserve(out Resource resource)
    {
        while (_freeResources.Count > 0)
        {
            resource = _freeResources.Dequeue();

            if (resource != null)
            {
                _reservedResources.Add(resource);
                return true;
            }
        }

        resource = null;
        return false;
    }

    public void Remove(Resource resource)
    {
        _reservedResources.Remove(resource);
    }
}