using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotSpawner))]
public class BotDispatcher : MonoBehaviour
{
    private readonly List<Bot> _bots = new List<Bot>();

    private BotSpawner _spawner;

    public event Action<Resource> ResourceDelivered;

    private void Awake()
    {
        _spawner = GetComponent<BotSpawner>();

        _bots.AddRange(_spawner.Spawn());
    }

    private void OnEnable()
    {
        foreach (Bot bot in _bots)
        {
            bot.ResourceDelivered += OnResourceDelivered;
        }
    }

    private void OnDisable()
    {
        foreach (Bot bot in _bots)
        {
            bot.ResourceDelivered -= OnResourceDelivered;
        }
    }

    public void AssignTasks(
        ResourceRegistry registry,
        Vector3 returnPosition)
    {
        foreach (Bot bot in _bots)
        {
            if (bot.IsFree == false)
            {
                continue;
            }

            if (registry.TryReserve(out Resource resource) == false)
            {
                return;
            }

            bot.Collect(resource, returnPosition);
        }
    }

    private void OnResourceDelivered(Resource resource)
    {
        ResourceDelivered?.Invoke(resource);
    }
}