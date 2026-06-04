using DevExpress.Xpf.Core;
using System.Windows;

namespace SmartEMR.Application.Xpf;

public class CheckEdit : DevExpress.Xpf.Editors.CheckEdit
{
    public CheckEdit()
    {
        this.MinWidth = 24;
        this.MinHeight = 18;
        this.HorizontalAlignment = HorizontalAlignment.Center;
        this.VerticalAlignment = VerticalAlignment.Center;
        this.VerticalContentAlignment = VerticalAlignment.Center;

        DevExpress.Xpf.Core.ThemeManager.SetThemeName(this, Theme.Office2019ColorfulName);
    }
}
