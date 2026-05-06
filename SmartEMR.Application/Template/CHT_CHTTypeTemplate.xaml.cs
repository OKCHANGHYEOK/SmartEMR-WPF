using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.Template;

/// <summary>
/// CHT_CHTTypeTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class CHT_CHTTypeTemplate : GridTemplate
{
    public override void Initalize()
    {
    }

    private void OnMouseLeftButtonDown_CHTTypeTemplate(object sender, MouseButtonEventArgs e)
    {
        var element = sender as GridTemplate;

        if (element == null) return;

        MessageBox.Show("기능 구현중입니다.");
    }
}
