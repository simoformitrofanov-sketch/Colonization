using UnityEngine;

[CreateAssetMenu(fileName = "BaseSpawnerConfig", menuName = "Configs/Base Spawner")]
public class BaseSpawnerConfig : ScriptableObject
{
    [SerializeField] private Base _prefab;

    public Base Prefab => _prefab;
}
