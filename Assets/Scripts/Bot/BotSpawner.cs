using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _prefab;
    [SerializeField, Min(0.1f)] private float _spawnRadius = 2f;

    public List<Bot> Spawn(int count)
    {
        List<Bot> bots = new List<Bot>();

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
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