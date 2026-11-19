using System;
using System.Collections.Generic;
using UnityEngine;

public class BotDispatcher : MonoBehaviour
{
    private readonly List<Bot> _bots = new List<Bot>();

    public event Action<int> CountChanged;

    public int Count => _bots.Count;

    public void Add(Bot bot)
    {
        _bots.Add(bot);

        CountChanged?.Invoke(Count);
    }

    public bool TryTakeFreeBot(out Bot bot)
    {
        bot = null;

        if (Count <= 1)
        {
            return false;
        }

        for (int i = 0; i < Count; i++)
        {
            if (_bots[i].IsFree == false)
            {
                continue;
            }

            bot = _bots[i];
            _bots.RemoveAt(i);

            CountChanged?.Invoke(Count);

            return true;
        }

        return false;
    }

    public void AssignTasks(
        ResourceRegistry registry, 
        ResourceReservations reservations, 
        CollectionPoint collectionPoint)
    {
        foreach (Bot bot in _bots)
        {
            if (bot.IsFree == false)
            {
                continue;
            }

            if (registry.TryReserve(reservations, out Resource resource) == false)
            {
                return;
            }

            bot.Collect(resource, collectionPoint);
        }
    }
}