using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;

    private BotMover _mover;
    private Resource _resource;
    private Vector3 _returnPosition;

    public enum State
    {
        Idle,
        MovingToResource,
        ReturningToBase
    }

    public State CurrentState { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }

    private void FixedUpdate()
    {
        if (CurrentState == State.Idle)
        {
            return;
        }

        _mover.Move();

        if (CurrentState == State.MovingToResource)
        {
            if (_mover.IsMoving == false)
            {
                _resource.AttachTo(_carryPoint);
                _mover.SetTarget(_returnPosition);

                CurrentState = State.ReturningToBase;
            }
        }
    }

    public void Collect(Resource resource, Vector3 returnPosition)
    {
        if (CurrentState != State.Idle || resource == null)
        {
            return;
        }

        _resource = resource;
        _returnPosition = returnPosition;

        _mover.SetTarget(_resource.transform.position);

        CurrentState = State.MovingToResource;
    }

    public Resource Unload()
    {
        if (CurrentState != State.ReturningToBase)
        {
            return null;
        }

        _mover.Stop();

        Resource resource = _resource;
        _resource = null;

        CurrentState = State.Idle;

        return resource;
    }
}