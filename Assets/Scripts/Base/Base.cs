using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotDispatcher))]
[RequireComponent(typeof(ResourceScanner))]
[RequireComponent(typeof(ResourceStorage))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private readonly ResourceRegistry _registry = new ResourceRegistry();

    private BotDispatcher _dispatcher;
    private ResourceScanner _scanner;
    private ResourceStorage _storage;

    private void Awake()
    {
        _dispatcher = GetComponent<BotDispatcher>();
        _scanner = GetComponent<ResourceScanner>();
        _storage = GetComponent<ResourceStorage>();
    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += OnResourcesFound;
        _dispatcher.ResourceDelivered += OnResourceDelivered;
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= OnResourcesFound;
        _dispatcher.ResourceDelivered -= OnResourceDelivered;
    }

    private void OnResourcesFound(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            _registry.Register(resource);
        }

        _dispatcher.AssignTasks(_registry, transform.position);
    }

    private void OnResourceDelivered(Resource resource)
    {
        _registry.Remove(resource);
        _resourceSpawner.Return(resource);

        _storage.Add();

        _dispatcher.AssignTasks(_registry, transform.position);
    }
}