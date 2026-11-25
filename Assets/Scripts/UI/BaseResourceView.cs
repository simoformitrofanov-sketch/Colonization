using TMPro;
using UnityEngine;

public class BaseResourceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Base _base;

    private void OnEnable()
    {
        if (_base != null)
        {
            _base.Changed += Refresh;

            Refresh();
        }
    }

    private void OnDisable()
    {
        if (_base != null)
        {
            _base.Changed -= Refresh;
        }
    }

    public void Initialize(Base baseInstance)
    {
        _base = baseInstance;

        if (isActiveAndEnabled)
        {
            _base.Changed += Refresh;

            Refresh();
        }
    }

    private void Refresh()
    {
        _text.text = _base.ResourceCount.ToString();
    }
}