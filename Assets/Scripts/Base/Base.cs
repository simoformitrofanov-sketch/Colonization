using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotDispatcher))]
[RequireComponent(typeof(ResourceScanner))]
[RequireComponent(typeof(ResourceStorage))]
[RequireComponent(typeof(CollectionPoint))]
public class Base : MonoBehaviour
{
    private const int BotCost = 3;
    private const int BaseCost = 5;

    [SerializeField] private BaseFlag _flag;

    private ResourceRegistry _registry;
    private BaseSpawner _baseSpawner;
    private BotSpawner _botSpawner;
    private BotDispatcher _dispatcher;
    private ResourceScanner _scanner;
    private ResourceStorage _storage;
    private CollectionPoint _collectionPoint;
    private Bot _builder;

    public event Action Changed;

    public int ConstructionCost => BaseCost;
    public int BotCreationCost => BotCost;
    public int ResourceCount => _storage.Count;
    public int BotCount => _dispatcher.Count;
    public bool CanPlaceFlag => BotCount > 1 || IsBuilding;
    public bool IsBuilding => _builder != null;
    public bool HasFlag => _flag.IsPlaced;

    private void Awake()
    {
        _dispatcher = GetComponent<BotDispatcher>();
        _scanner = GetComponent<ResourceScanner>();
        _storage = GetComponent<ResourceStorage>();
        _collectionPoint = GetComponent<CollectionPoint>();
    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += OnResourcesFound;
        _collectionPoint.BotTaskFinished += OnBotTaskFinished;
        _storage.CountChanged += OnCountChanged;
        _dispatcher.CountChanged += OnCountChanged;

        if (_builder != null)
        {
            _builder.ConstructionSiteReached += OnConstructionSiteReached;
        }
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= OnResourcesFound;
        _collectionPoint.BotTaskFinished -= OnBotTaskFinished;
        _storage.CountChanged -= OnCountChanged;
        _dispatcher.CountChanged -= OnCountChanged;

        if (_builder != null)
        {
            _builder.ConstructionSiteReached -= OnConstructionSiteReached;
        }
    }

    public void Initialize(
        BaseSpawner baseSpawner,
        BotSpawner botSpawner,
        ResourceRegistry registry,
        Collider ground)
    {
        _baseSpawner = baseSpawner;
        _botSpawner = botSpawner;
        _registry = registry;
        _scanner.Initialize(ground);
    }

    public void StartWorking()
    {
        _scanner.StartScanning();
    }

    public void AddBot(Bot bot)
    {
        _dispatcher.Add(bot);
    }

    public void PlaceFlag(Vector3 position)
    {
        if (CanPlaceFlag == false)
        {
            return;
        }

        _flag.Place(position);

        ProcessTasks();

        Changed?.Invoke();
    }

    private void ProcessTasks()
    {
        if (_flag.IsPlaced)
        {
            TryStartConstruction();
        }
        else
        {
            TryBuyBot();
        }

        _dispatcher.AssignTasks(
            _registry,
            _collectionPoint);
    }

    private void TryBuyBot()
    {
        if (_flag.IsPlaced)
        {
            return;
        }

        if (_storage.TrySpend(BotCost) == false)
        {
            return;
        }

        Bot bot = _botSpawner.Spawn(transform.position);
        _dispatcher.Add(bot);
    }

    private void TryStartConstruction()
    {
        if (_builder != null || _storage.Count < BaseCost)
        {
            return;
        }

        if (_dispatcher.TryTakeBuilder(out Bot builder) == false)
        {
            return;
        }

        _storage.TrySpend(BaseCost);

        _builder = builder;
        _builder.ConstructionSiteReached += OnConstructionSiteReached;
        _builder.MoveToConstructionSite(_flag.Position);

        Changed?.Invoke();
    }

    private void OnResourcesFound(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            _registry.Register(resource);
        }

        ProcessTasks();
    }

    private void OnBotTaskFinished(Bot bot, Resource resource)
    {
        _registry.Remove(resource);
        resource.Deliver();

        _storage.Add();

        ProcessTasks();
    }

    private void OnConstructionSiteReached(Bot bot)
    {
        if (bot.HasReached(_flag.Position) == false)
        {
            bot.MoveToConstructionSite(_flag.Position);
            return;
        }

        bot.ConstructionSiteReached -= OnConstructionSiteReached;

        Base newBase = _baseSpawner.Spawn(_flag.Position);

        newBase.AddBot(bot);
        newBase.StartWorking();

        _builder = null;
        _flag.Hide();

        ProcessTasks();

        Changed?.Invoke();
    }

    private void OnCountChanged(int count) 
    {
        Changed?.Invoke();
    }
}