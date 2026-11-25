using UnityEngine;

[CreateAssetMenu(fileName = "BotSpawnerConfig", menuName = "Configs/Bot Spawner")]
public class BotSpawnerConfig : ScriptableObject
{
    [SerializeField] private Bot _prefab;
    [SerializeField, Min(0.1f)] private float _spawnRadius = 2f;

    public Bot Prefab => _prefab;
    public float SpawnRadius => _spawnRadius;
}
