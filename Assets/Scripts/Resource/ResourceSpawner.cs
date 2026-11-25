using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    private ResourceSpawnerConfig _resourceSpawnerConfig;
    private ObjectPool<Resource> _resourcesPool;
    private Coroutine _spawnRoutine;
    private Collider _ground;

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

    public void Initialize(ResourceSpawnerConfig resourceSpawnerConfig, Collider ground)
    {
        _resourceSpawnerConfig = resourceSpawnerConfig;
        _ground = ground;
        _resourcesPool = new ObjectPool<Resource>(
            createFunc: CreateResource,
            actionOnGet: null,
            actionOnRelease: OnReturned,
            actionOnDestroy: DestroyResource,
            maxSize: _resourceSpawnerConfig.MaxCount);
    }

    public void StartSpawning()
    {
        _spawnRoutine = StartCoroutine(SpawnResources());
    }

    private Resource CreateResource()
    {
        Resource resource = Instantiate(_resourceSpawnerConfig.Prefab, transform);

        resource.gameObject.SetActive(false);
        resource.Delivered += Return;
        
        return resource;
    }

    private void Spawn()
    {
        Bounds bounds = _ground.bounds;

        float x = Random.Range(
            bounds.min.x + _resourceSpawnerConfig.EdgeOffset,
            bounds.max.x - _resourceSpawnerConfig.EdgeOffset);

        float z = Random.Range(
            bounds.min.z + _resourceSpawnerConfig.EdgeOffset,
            bounds.max.z - _resourceSpawnerConfig.EdgeOffset);

        Vector3 position = new Vector3(x, bounds.max.y, z);

        Resource resource = _resourcesPool.Get();

        resource.transform.SetPositionAndRotation(
            position,
            Quaternion.identity);

        resource.ResetState();
        resource.gameObject.SetActive(true);
    }
   
    private IEnumerator SpawnResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_resourceSpawnerConfig.SpawnInterval);

        while (enabled)
        {
            if (_resourcesPool.CountActive < _resourceSpawnerConfig.MaxCount)
            {
                Spawn();
            }

            yield return delay;
        }
    }

    private void Return(Resource resource)
    {
        _resourcesPool.Release(resource);
    }
   
    private void OnReturned(Resource resource)
    {
        resource.gameObject.SetActive(false);
        resource.transform.SetParent(transform, true);
    }

    private void DestroyResource(Resource resource)
    {
        resource.Delivered -= Return;
        Destroy(resource.gameObject);
    }
}