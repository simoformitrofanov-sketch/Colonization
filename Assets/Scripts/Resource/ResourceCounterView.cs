using TMPro;
using UnityEngine;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _storage.CountChanged += Refresh;
        Refresh(_storage.Count);
    }

    private void OnDisable()
    {
        _storage.CountChanged -= Refresh;
    }

    private void Refresh(int count)
    {
        _text.text = $"Ресурсы: {count}";
    }
}