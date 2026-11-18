using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;

    private BotMover _mover;
    private Resource _resource;
    private Vector3 _returnPosition;

    public event Action<Resource> ResourceDelivered;

    public bool IsFree => _resource == null;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnResourceReached;
        _mover.TargetReached -= OnBaseReached;

        _mover.Stop();
    }

    public void Collect(Resource resource, Vector3 returnPosition)
    {
        if (IsFree == false || resource == null)
        {
            return;
        }

        _resource = resource;
        _returnPosition = returnPosition;

        _mover.TargetReached += OnResourceReached;
        _mover.Move(_resource.transform.position);
    }

    private void OnResourceReached()
    {
        _mover.TargetReached -= OnResourceReached;

        _resource.AttachTo(_carryPoint);

        _mover.TargetReached += OnBaseReached;
        _mover.Move(_returnPosition);
    }

    private void OnBaseReached()
    {
        _mover.TargetReached -= OnBaseReached;

        Resource resource = _resource;
        _resource = null;

        ResourceDelivered?.Invoke(resource);
    }
}