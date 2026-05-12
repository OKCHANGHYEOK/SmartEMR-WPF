using SmartEMR.Application.ViewBase;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartEMR.Application.Common;

public enum eSmartEMRLocation 
{ 
    CALENDAR = 0,
    DESK = 1,
    EXAM = 2,
    PAYMENT = 3,
    CRM = 4,
    CONFIG = 5
}

public class Common
{
    public BrushConverter BrushConverter { get; } = new BrushConverter();

    public void DisposeControl(object? element)
    {
        if (element == null) return;

        if (element is IDisposable disposable)
        {
            disposable.Dispose(true);
        }

        if (element is DependencyObject obj)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            
            for (int i =0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);  

                DisposeControl(child);
            }
        }
    }
}
