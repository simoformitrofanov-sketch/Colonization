using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner
{
    private readonly HashSet<Resource> _sharedReservations;
    private BaseSpawnerConfig _baseSpawnerConfig;
    private BotSpawner _botSpawner;
    private Collider _ground;

    public BaseSpawner(
        HashSet<Resource> sharedReservations,
        BaseSpawnerConfig baseSpawnerConfig,
        BotSpawner botSpawner,
        Collider ground)
    {
        _sharedReservations = sharedReservations;
        _baseSpawnerConfig = baseSpawnerConfig;
        _botSpawner = botSpawner;
        _ground = ground;
    }

    public Base Spawn(Vector3 position)
    {
        Base newBase = Object.Instantiate(
            _baseSpawnerConfig.Prefab,
            position,
            Quaternion.identity);

        ResourceRegistry registry = new ResourceRegistry(_sharedReservations);

        newBase.Initialize(
            this,
            _botSpawner,
            registry,
            _ground);

        BaseResourceView resourceView = newBase.GetComponentInChildren<BaseResourceView>(true);

        resourceView.Initialize(newBase);

        return newBase;
    }
}