using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private Collider _ground;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField, Min(0.1f)] private float _scanHeight = 2f;
    [SerializeField, Min(0.1f)] private float _scanInterval = 1f;

    private Coroutine _scanRoutine;

    public event Action<List<Resource>> ResourcesFound;

    private void OnDisable()
    {
        if (_scanRoutine != null)
        {
            StopCoroutine(_scanRoutine);
            _scanRoutine = null;
        }
    }

    public void Initialize(Collider ground)
    {
        _ground = ground;
    }

    public void StartScanning()
    {
        _scanRoutine = StartCoroutine(SearchResources());
    }

    private IEnumerator SearchResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanInterval);

        while (enabled)
        {
            yield return delay;

            ResourcesFound?.Invoke(Scan());
        }
    }

    private List<Resource> Scan()
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

        List<Resource> resources = new List<Resource>();

        foreach (Collider foundCollider in colliders)
        {
            if (foundCollider.TryGetComponent(out Resource resource))
            {
                resources.Add(resource);
            }
        }

        return resources;
    }
}