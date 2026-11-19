using System;
using UnityEngine;

public class CollectionPoint : MonoBehaviour
{
    public event Action<Bot, Resource> BotTaskFinished;

    public void Collect(Bot bot, Resource resource)
    {
        BotTaskFinished?.Invoke(bot, resource);
    }
}