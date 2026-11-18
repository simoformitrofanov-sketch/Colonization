using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _rotationSpeed = 180f;

    private Rigidbody _rigidbody;
    private Vector3 _targetPosition;

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _targetPosition.y = _rigidbody.position.y;

        IsMoving = true;
    }

    public void Move()
    {
        if (IsMoving == false)
        {
            return;
        }

        Vector3 direction = _targetPosition - _rigidbody.position;

        if (direction == Vector3.zero)
        {
            Stop();
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion nextRotation = Quaternion.RotateTowards(
            _rigidbody.rotation,
            targetRotation,
            _rotationSpeed * Time.fixedDeltaTime);

        _rigidbody.MoveRotation(nextRotation);

        Vector3 nextPosition = Vector3.MoveTowards(
            _rigidbody.position,
            _targetPosition,
            _speed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(nextPosition);
    }

    public void Stop()
    {
        IsMoving = false;
    }
}