using System.Globalization;
using System.Windows;

namespace SmartEMR.Application.Common.Converter
{
    public class IntToVisibilityConverter : BaseConverter
    {
        public bool invert { get; set; } = false;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 1. 먼저 값이 int형이고 0보다 큰지 판별 (환자가 선택되었는지 여부)
            bool isPatientSelected = value is int intValue && intValue > 0;

            // 2. invert 속성이 true면 이 결과를 반전시킴
            bool shouldBeVisible = invert ? !isPatientSelected : isPatientSelected;

            // 3. 최종 Visibility 반환
            return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
