using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private BaseSpawnerConfig _baseSpawnerConfig;
    [SerializeField] private BotSpawnerConfig _botSpawnerConfig;
    [SerializeField] private ResourceSpawnerConfig _resourceSpawnerConfig;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private Collider _ground;
    [SerializeField] private int _initialBotCount = 3;

    private BotSpawner _botSpawner;
    private BaseSpawner _baseSpawner;

    private void Start()
    {
        HashSet<Resource> sharedReservations = new HashSet<Resource>();

        _resourceSpawner.Initialize(_resourceSpawnerConfig, _ground);

        _botSpawner = new BotSpawner(_botSpawnerConfig);

        _baseSpawner = new BaseSpawner(
            sharedReservations,
            _baseSpawnerConfig,
            _botSpawner,
            _ground);

        Base newBase = _baseSpawner.Spawn(transform.position);

        for (int i = 0; i < _initialBotCount; i++)
        {
            Bot bot = _botSpawner.Spawn(newBase.transform.position);

            newBase.AddBot(bot);
        }

        newBase.StartWorking();
        _resourceSpawner.StartSpawning();
    }
}
