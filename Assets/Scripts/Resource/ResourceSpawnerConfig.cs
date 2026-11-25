using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSpawnerConfig", menuName = "Configs/Resource Spawner")]
public class ResourceSpawnerConfig : ScriptableObject
{
    [SerializeField] private Resource _prefab;
    [SerializeField, Min(0.1f)] private float _spawnInterval = 2f;
    [SerializeField, Min(0f)] private float _edgeOffset = 0.5f;
    [SerializeField, Min(1)] private int _maxCount = 20;

    public Resource Prefab => _prefab;
    public float SpawnInterval => _spawnInterval;
    public float EdgeOffset => _edgeOffset;
    public int MaxCount => _maxCount;
}
