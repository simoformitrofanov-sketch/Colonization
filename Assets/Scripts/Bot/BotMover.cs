using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotMover : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float _speed = 3f;
    [SerializeField, Min(0.1f)] private float _rotationSpeed = 180f;

    private readonly WaitForFixedUpdate _physicsWait = new WaitForFixedUpdate();

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        targetPosition.y = _rigidbody.position.y;

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
    }
}