using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.Xpf;

public class FloatPanel : CustomControl
{
    public ICommand CloseCommand { get; }

    static FloatPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatPanel), new FrameworkPropertyMetadata(typeof(FloatPanel)));
    }

    public FloatPanel() : base()
    {
        CloseCommand = new RelayCommand(ExecuteClose);
    }

    private void ExecuteClose()
    {
        SmartUI.CloseFloatPanel(this);
    }
}
 