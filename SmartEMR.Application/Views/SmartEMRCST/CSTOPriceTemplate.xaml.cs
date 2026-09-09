using System.Globalization;
using System.Windows.Input;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
///CSTOPriceTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class CSTOPriceTemplate : GridTemplate
{
    public CSTOPriceTemplate() { }

    public override void Initialize()
    {
    }

    private async void OnEditValueChanged_TextEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        if (sender is not TextEdit element || !element.IsKeyboardFocusWithin) return;

        var dataItem = element.DataContext as ConsultationOrder;
        if (dataItem is not null)
        {
            await SmartUI.SendMessage("UpdateCSTOByPrice", dataItem);
        }

        SmartUI.BeginInvoke(() =>
        {
            element.Focus();
            element.CaretIndex = element.Text?.Length ?? 0;
        }, System.Windows.Threading.DispatcherPriority.Background);
    }
}

public class InsuranceTypeToBooleanConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string CSTO_InsuranceType) return false;

        return CSTO_InsuranceType == "INS";
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}