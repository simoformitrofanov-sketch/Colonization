using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    private const float FullCircleDegrees = 360f;

    [SerializeField] private Bot _prefab;
    [SerializeField] private BotDispatcher _initialBaseDispatcher;
    [SerializeField, Min(0.1f)] private float _spawnRadius = 2f;
    [SerializeField, Min(1)] private int _initialBotCount = 3;

    private void Start()
    {
        Vector3 basePosition = _initialBaseDispatcher.transform.position;

        for (int i = 0; i < _initialBotCount; i++)
        {
            Bot bot = Spawn(basePosition);
            _initialBaseDispatcher.Add(bot);
        }
    }

    public Bot Spawn(Vector3 basePosition)
    {
        float angle = Random.Range(0f, FullCircleDegrees);

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 offset = rotation * Vector3.forward * _spawnRadius;
        Vector3 position = basePosition + offset;

        return Instantiate(_prefab, position, rotation);
    }
}