using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    private const float FullCircleDegrees = 360f;

    [SerializeField] private Bot _prefab;
    [SerializeField, Min(0.1f)] private float _spawnRadius = 2f;
    [SerializeField, Min(1)] private int _initialBotCount = 3;

    public List<Bot> Spawn()
    {
        List<Bot> bots = new List<Bot>();

        float angleStep = FullCircleDegrees / _initialBotCount;

        for (int i = 0; i < _initialBotCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, angleStep * i, 0f);

            Vector3 offset = rotation * Vector3.forward * _spawnRadius;
            Vector3 position = transform.position + offset;

            Bot bot = Instantiate(_prefab, position, rotation);
            bots.Add(bot);
        }

        return bots;
    }
}