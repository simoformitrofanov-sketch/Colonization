using TMPro;
using UnityEngine;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _base.ResourceCountChanged += Refresh;
        Refresh(_base.ResourceCount);
    }

    private void OnDisable()
    {
        _base.ResourceCountChanged -= Refresh;
    }

    private void Refresh(int count)
    {
        _text.text = $"Ресурсы: {count}";
    }
}