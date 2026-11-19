using System;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class BaseSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _ground;
    [SerializeField] private LayerMask _selectionMask;

    private InputReader _inputReader;
    private bool _isPlacingFlag;

    public event Action<Base> SelectionChanged;

    public Base SelectedBase { get; private set; }

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        _inputReader.Clicked -= OnClicked;
        _isPlacingFlag = false;
    }

    private void OnClicked(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            _selectionMask,
            QueryTriggerInteraction.Collide) == false)
        {
            return;
        }

        if (hit.collider.TryGetComponent(out Base selectedBase))
        {
            SelectedBase = selectedBase;
            _isPlacingFlag = true;

            SelectionChanged?.Invoke(SelectedBase);
            return;
        }

        if (SelectedBase == null ||
            _isPlacingFlag == false ||
            hit.collider != _ground)
        {
            return;
        }

        SelectedBase.PlaceFlag(hit.point);
        _isPlacingFlag = false;
    }
}