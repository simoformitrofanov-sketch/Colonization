using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class BotMover : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float _speed = 3f;
    [SerializeField, Min(0.1f)] private float _rotationSpeed = 180f;
    [SerializeField, Min(0.01f)] private float _arrivalDistance = 0.05f;

    private readonly WaitForFixedUpdate _physicsWait = new WaitForFixedUpdate();

    private Rigidbody _rigidbody;
    private Coroutine _moveRoutine;

    public event Action Arrived;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        StopMoving();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        StopMoving();

        _moveRoutine = StartCoroutine(MoveRoutine(targetPosition));
    }

    public void StopMoving()
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
    }

    public bool HasReached(Vector3 targetPosition)
    {
        targetPosition.y = _rigidbody.position.y;

        Vector3 offset = targetPosition - _rigidbody.position;

        return offset.sqrMagnitude <= _arrivalDistance * _arrivalDistance;
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        targetPosition.y = _rigidbody.position.y;

        while (enabled)
        {
            yield return _physicsWait;

            Vector3 offset = targetPosition - _rigidbody.position;

            if (HasReached(targetPosition))
            {
                break;
            }

            Quaternion targetRotation = Quaternion.LookRotation(offset);

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

        _moveRoutine = null;

        Arrived?.Invoke();
    }
}