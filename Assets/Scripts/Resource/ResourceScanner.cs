using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private Collider _ground;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField, Min(0.1f)] private float _scanHeight = 2f;

    public void Scan(ResourceRegistry registry)
    {
        Bounds bounds = _ground.bounds;

        Vector3 halfExtents = new Vector3(
            bounds.extents.x,
            _scanHeight / 2f,
            bounds.extents.z);

        Vector3 center = new Vector3(
            bounds.center.x,
            bounds.max.y + halfExtents.y,
            bounds.center.z);

        Collider[] colliders = Physics.OverlapBox(
            center,
            halfExtents,
            Quaternion.identity,
            _resourceMask,
            QueryTriggerInteraction.Ignore);

        foreach (Collider foundCollider in colliders)
        {
            if (foundCollider.TryGetComponent(out Resource resource))
            {
                registry.Register(resource);
            }
        }
    }
}