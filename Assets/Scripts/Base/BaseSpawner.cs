using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourceReservations _reservations;
    [SerializeField] private Collider _ground;

    public Base Spawn(Vector3 position, Bot founder)
    {
        Base newBase = Instantiate(
            _prefab,
            position,
            Quaternion.identity);

        newBase.Initialize(
            this,
            _botSpawner,
            _resourceSpawner,
            _reservations,
            _ground,
            founder);

        return newBase;
    }
}