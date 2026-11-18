using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Collider _ground;
    [SerializeField, Min(0.1f)] private float _spawnInterval = 2f;
    [SerializeField, Min(1)] private int _maxCount = 20;
    [SerializeField, Min(0f)] private float _edgeOffset = 0.5f;

    private ObjectPool<Resource> _resourcesPool;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        _resourcesPool = new ObjectPool<Resource>(
            createFunc: CreateResource,
            actionOnGet: null,
            actionOnRelease: OnReturned,
            actionOnDestroy: DestroyResource,
            maxSize: _maxCount);
    }

    private void OnEnable()
    {
        _spawnRoutine = StartCoroutine(SpawnResources());
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private void OnDestroy()
    {
        _resourcesPool.Clear();
    }

    public void Return(Resource resource)
    {
        _resourcesPool.Release(resource);
    }

    private IEnumerator SpawnResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            if (_resourcesPool.CountActive < _maxCount)
            {
                Spawn();
            }

            yield return delay;
        }
    }

    private void Spawn()
    {
        Bounds bounds = _ground.bounds;

        float x = Random.Range(
            bounds.min.x + _edgeOffset,
            bounds.max.x - _edgeOffset);

        float z = Random.Range(
            bounds.min.z + _edgeOffset,
            bounds.max.z - _edgeOffset);

        Vector3 position = new Vector3(x, bounds.max.y, z);

        Resource resource = _resourcesPool.Get();

        resource.transform.SetPositionAndRotation(
            position,
            Quaternion.identity);

        resource.ResetState();
        resource.gameObject.SetActive(true);
    }

    private Resource CreateResource()
    {
        Resource resource = Instantiate(_prefab, transform);

        resource.gameObject.SetActive(false);

        return resource;
    }

    private void OnReturned(Resource resource)
    {
        resource.gameObject.SetActive(false);
        resource.transform.SetParent(transform, true);
    }

    private void DestroyResource(Resource resource)
    {
        Destroy(resource.gameObject);
    }
}