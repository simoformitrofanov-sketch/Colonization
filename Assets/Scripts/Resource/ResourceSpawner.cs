using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Collider _ground;
    [SerializeField, Min(0.1f)] private float _spawnInterval = 2f;
    [SerializeField, Min(1)] private int _maxResourceCount = 20;
    [SerializeField, Min(0f)] private float _edgeOffset = 0.5f;

    private readonly List<Resource> _resources = new List<Resource>();

    private Coroutine _spawnRoutine;

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

    private IEnumerator SpawnResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            RemoveDeliveredResources();

            if (_resources.Count < _maxResourceCount)
            {
                Spawn();
            }

            yield return delay;
        }
    }

    private void RemoveDeliveredResources()
    {
        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            if (_resources[i] == null)
            {
                _resources.RemoveAt(i);
            }
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

        Resource resource = Instantiate(
            _prefab,
            position,
            Quaternion.identity);

        _resources.Add(resource);
    }
}