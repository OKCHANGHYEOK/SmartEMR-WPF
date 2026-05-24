using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using System.Windows.Input;

namespace SmartEMR.Application.Xpf;

public class FloatPanel : CustomControl
{
    public ICommand CloseCommand { get; }

    public FloatPanel()
    {
        CloseCommand = new RelayCommand(ExecuteClose);
    }

    private void ExecuteClose()
    {
        SmartUI.CloseFloatPanel(this);
    }
}
 