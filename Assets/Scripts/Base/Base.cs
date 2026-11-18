using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotSpawner))]
[RequireComponent(typeof(ResourceScanner))]
public class Base : MonoBehaviour
{
    [SerializeField, Min(1)] private int _initialBotCount = 3;
    [SerializeField, Min(0.1f)] private float _scanInterval = 1f;

    private readonly ResourceRegistry _registry = new ResourceRegistry();
    private readonly List<Bot> _bots = new List<Bot>();

    private BotSpawner _spawner;
    private ResourceScanner _scanner;
    private Coroutine _resourceSearchRoutine;

    public event Action<int> ResourceCountChanged;

    public int ResourceCount { get; private set; }

    private void Awake()
    {
        _spawner = GetComponent<BotSpawner>();
        _scanner = GetComponent<ResourceScanner>();

        _bots.AddRange(_spawner.Spawn(_initialBotCount));
    }

    private void OnEnable()
    {
        _resourceSearchRoutine = StartCoroutine(SearchResourcesAndAssignTasks());
    }

    private void OnDisable()
    {
        if (_resourceSearchRoutine != null)
        {
            StopCoroutine(_resourceSearchRoutine);
            _resourceSearchRoutine = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Bot bot) == false)
        {
            return;
        }

        Resource resource = bot.Unload();

        if (resource == null)
        {
            return;
        }

        _registry.Remove(resource);
        Destroy(resource.gameObject);

        ResourceCount++;
        ResourceCountChanged?.Invoke(ResourceCount);
    }

    private IEnumerator SearchResourcesAndAssignTasks()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanInterval);

        while (enabled)
        {
            _scanner.Scan(_registry);
            AssignTasks();

            yield return delay;
        }
    }

    private void AssignTasks()
    {
        foreach (Bot bot in _bots)
        {
            if (bot.CurrentState != Bot.State.Idle)
            {
                continue;
            }

            if (_registry.TryReserve(out Resource resource) == false)
            {
                return;
            }

            bot.Collect(resource, transform.position);
        }
    }
}