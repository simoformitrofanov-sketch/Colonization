using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;

    private BotMover _mover;
    private Resource _resource;
    private CollectionPoint _collectionPoint;

    public event Action<Bot> ConstructionSiteReached;

    public bool IsFree { get; private set; } = true;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }

    private void OnDisable()
    {
        _mover.Arrived -= OnResourceReached;
        _mover.Arrived -= OnCollectionPointReached;
        _mover.Arrived -= OnConstructionSiteReached;
        _mover.StopMoving();

        _resource = null;
        _collectionPoint = null;
        IsFree = true;
    }

    public void Collect(Resource resource, CollectionPoint collectionPoint)
    {
        if (IsFree == false || resource == null)
        {
            return;
        }

        _resource = resource;
        _collectionPoint = collectionPoint;
        IsFree = false;

        _mover.Arrived += OnResourceReached;
        _mover.MoveTo(_resource.transform.position);
    }

    public void MoveToConstructionSite(Vector3 position)
    {
        if (IsFree == false)
        {
            return;
        }

        IsFree = false;

        _mover.Arrived += OnConstructionSiteReached;
        _mover.MoveTo(position);
    }

    public bool HasReached(Vector3 targetPosition)
    {
        return _mover.HasReached(targetPosition);
    }

    private void OnResourceReached()
    {
        _mover.Arrived -= OnResourceReached;

        _resource.AttachTo(_carryPoint);

        _mover.Arrived += OnCollectionPointReached;
        _mover.MoveTo(_collectionPoint.transform.position);
    }

    private void OnCollectionPointReached()
    {
        _mover.Arrived -= OnCollectionPointReached;

        Resource resource = _resource;
        CollectionPoint collectionPoint = _collectionPoint;

        _resource = null;
        _collectionPoint = null;
        IsFree = true;

        collectionPoint.Collect(this, resource);
    }

    private void OnConstructionSiteReached()
    {
        _mover.Arrived -= OnConstructionSiteReached;

        IsFree = true;

        ConstructionSiteReached?.Invoke(this);
    }
}