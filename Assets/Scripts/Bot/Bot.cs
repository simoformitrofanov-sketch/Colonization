using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;

    private BotMover _mover;
    private Coroutine _taskRoutine;

    public event Action<Bot> ConstructionSiteReached;

    public bool IsFree => _taskRoutine == null;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }

    private void OnDisable()
    {
        if (_taskRoutine != null)
        {
            StopCoroutine(_taskRoutine);
            _taskRoutine = null;
        }
    }

    public void Collect(Resource resource, CollectionPoint collectionPoint)
    {
        if (IsFree == false || resource == null)
        {
            return;
        }

        _taskRoutine = StartCoroutine(CollectResource(resource, collectionPoint));
    }

    public void MoveToConstructionSite(Vector3 position)
    {
        if (IsFree == false)
        {
            return;
        }

        _taskRoutine = StartCoroutine(ReachConstructionSite(position));
    }

    private IEnumerator CollectResource(Resource resource, CollectionPoint collectionPoint)
    {
        yield return _mover.MoveTo(resource.transform.position);

        resource.AttachTo(_carryPoint);

        yield return _mover.MoveTo(collectionPoint.transform.position);

        _taskRoutine = null;

        collectionPoint.Collect(this, resource);
    }

    private IEnumerator ReachConstructionSite(Vector3 position)
    {
        yield return _mover.MoveTo(position);

        _taskRoutine = null;

        ConstructionSiteReached?.Invoke(this);
    }
}