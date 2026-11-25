using UnityEngine;

public class BotSpawner
{
    private const float FullCircleDegrees = 360f;

    private BotSpawnerConfig _botSpawnerConfig;

    public BotSpawner(BotSpawnerConfig botSpawnerConfig)
    {
        _botSpawnerConfig = botSpawnerConfig;
    }

    public Bot Spawn(Vector3 basePosition)
    {
        float angle = Random.Range(0f, FullCircleDegrees);

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 offset = rotation * Vector3.forward * _botSpawnerConfig.SpawnRadius;
        Vector3 position = basePosition + offset;

        return Object.Instantiate(_botSpawnerConfig.Prefab, position, rotation);
    }
}