using System.Collections.Generic;

public class ResourceRegistry
{
    private readonly List<Resource> _freeResources = new List<Resource>();
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

        _freeResources.Add(resource);
    }

    public bool TryReserve(out Resource resource)
    {
        resource = null;

        while (_freeResources.Count > 0)
        {
            int lastIndex = _freeResources.Count - 1;

            Resource candidate = _freeResources[lastIndex];
            _freeResources.RemoveAt(lastIndex);

            if (candidate != null)
            {
                _reservedResources.Add(candidate);
                resource = candidate;

                return true;
            }
        }

        return false;
    }

    public void Remove(Resource resource)
    {
        _freeResources.Remove(resource);
        _reservedResources.Remove(resource);
    }
}