using TMPro;
using UnityEngine;

public class BaseInfoView : MonoBehaviour
{
    [SerializeField] private BaseSelector _selector;
    [SerializeField] private TMP_Text _text;

    private Base _selectedBase;

    private void OnEnable()
    {
        _selector.SelectionChanged += OnSelectionChanged;

        OnSelectionChanged(_selector.SelectedBase);
    }

    private void OnDisable()
    {
        _selector.SelectionChanged -= OnSelectionChanged;

        if (_selectedBase != null)
        {
            _selectedBase.Changed -= Refresh;
        }

        _selectedBase = null;
    }

    private void OnSelectionChanged(Base selectedBase)
    {
        if (_selectedBase != null)
        {
            _selectedBase.Changed -= Refresh;
        }

        _selectedBase = selectedBase;

        if (_selectedBase != null)
        {
            _selectedBase.Changed += Refresh;
        }

        Refresh();
    }

    private void Refresh()
    {
        if (_selectedBase == null)
        {
            _text.text = "Выберите базу";
            return;
        }

        string botStatus = "автосборка";
        string colonizationStatus = "установить точку сбора - ЛКМ";

        if (_selectedBase.IsBuilding)
        {
            botStatus = "приостановлено";
            colonizationStatus = "строитель в пути";
        }
        else if (_selectedBase.HasFlag)
        {
            botStatus = "приостановлено";
            colonizationStatus = "флаг установлен";

            if (_selectedBase.ResourceCount >= _selectedBase.ConstructionCost)
            {
                colonizationStatus = "ожидание свободного бота";
            }
        }
        else if (_selectedBase.CanPlaceFlag == false)
        {
            colonizationStatus = "нужны минимум 2 бота";
        }

        _text.text =
            $"Выбранная база:\n" +
            $"Материалы - {_selectedBase.ResourceCount}  " +
            $"Боты - {_selectedBase.BotCount}\n\n" +
            $"Развитие:\n" +
            $"Создание бота - {_selectedBase.BotCreationCost} ({botStatus})\n" +
            $"Колонизация - {_selectedBase.ConstructionCost} ({colonizationStatus})";
    }
}