using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Core;

public class PopupManager
{
    private readonly ObservableCollection<FloatPanel> _activePopups = new();
    public ObservableCollection<FloatPanel> Popups => _activePopups;

    public bool HasPopup => _activePopups.Any();

    public void Show(ViewLayout vl)
    {
        vl.IsPopupView = true;

        Add(new FloatPanel { Content = vl });
    }

    public void Close(FloatPanel floatPanel)
    {
        Remove(floatPanel);
    }

    private void Add(FloatPanel panel)
    {
        if (!_activePopups.Contains(panel))
        {
            _activePopups.Add(panel);
        }

        UpdatePopupState();
    }

    private void Remove(FloatPanel panel)
    {
        if (panel == null) return;

        _activePopups.Remove(panel);

        UpdatePopupState();
    }

    private void UpdatePopupState()
    {
        foreach(var popup in _activePopups)
        {
            popup.IsTopMostPopup = popup == _activePopups.LastOrDefault();
        }
    }
}

