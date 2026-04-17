using SmartEMR.Application.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public interface IViewLayout
{
    IReadOnlyList<BindGrid> BindGrids { get; }

    void OnBindGrid_BindClick(object sender, BindClickEventArgs e); 
}
