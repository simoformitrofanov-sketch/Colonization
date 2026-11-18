using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotMover : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float _speed = 3f;
    [SerializeField, Min(0.1f)] private float _rotationSpeed = 180f;

    private readonly WaitForFixedUpdate _physicsWait = new WaitForFixedUpdate();

    private Rigidbody _rigidbody;
    private Coroutine _movementRoutine;

    public event Action TargetReached;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        Stop();
    }

    public void Move(Vector3 targetPosition)
    {
        Stop();

        targetPosition.y = _rigidbody.position.y;

        _movementRoutine = StartCoroutine(MoveToTarget(targetPosition));
    }

    public void Stop()
    {
        if (_movementRoutine == null)
        {
            return;
        }

        StopCoroutine(_movementRoutine);
        _movementRoutine = null;
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (enabled)
        {
            yield return _physicsWait;

            Vector3 direction = targetPosition - _rigidbody.position;

            if (direction == Vector3.zero)
            {
                break;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion nextRotation = Quaternion.RotateTowards(
                _rigidbody.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(nextRotation);

            Vector3 nextPosition = Vector3.MoveTowards(
                _rigidbody.position,
                targetPosition,
                _speed * Time.fixedDeltaTime);

            _rigidbody.MovePosition(nextPosition);
        }

        _movementRoutine = null;
        TargetReached?.Invoke();
    }
}