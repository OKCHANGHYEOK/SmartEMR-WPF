using HandyControl.Controls;
using SmartEMR.Application.Common.Converter;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

public enum eBirthType
{
    Year,
    Month,
    Day
}

public enum OperationType
{
    CREATE,
    UPDATE,
    DELETE
}

public class Common
{
    public BrushConverter BrushConverter { get; } = new BrushConverter();

    private List<ChartCommonCode> _arrCCC = new();
    public IReadOnlyList<ChartCommonCode> arrCCC => _arrCCC.AsReadOnly();

    public async Task Initialize()
    {
        var retCCC = await SmartMVVM.DataStore.GetItems<ChartCommonCode>(eAPI.ChartCommonCode_GetChartCommonCode, new ChartCommonCode());
        if (retCCC == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("ChartCommonCode_GetChartCommonCode 조회에 실패했습니다.", NotificationType.Error);
            return;
        }

        _arrCCC = retCCC.ToList();
    }

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

    public IQueryable<ChartCommonCode> GetChartCommonCode(string CCCM_Cd = "", string CCCG_Cd = "", string CCC_Cd = "", bool isAll = false)
    {
        List<ChartCommonCode> retCCC = new();

        if (isAll)
        {
            retCCC.Add(new ChartCommonCode { CCC_Name = "전체", CCC_Cd = "ALL"});
        }

        IEnumerable<ChartCommonCode>? targetItems = null;

        if (!string.IsNullOrWhiteSpace(CCCM_Cd))
        {
            targetItems = arrCCC.Where(x => x.CCCM_Cd == CCCM_Cd);
        }

        if (!string.IsNullOrWhiteSpace(CCCG_Cd))
        {
            targetItems = arrCCC.Where(x => x.CCCG_Cd == CCCG_Cd);
        }

        if (!string.IsNullOrWhiteSpace(CCC_Cd))
        {
            targetItems = arrCCC.Where(x => x.CCC_Cd == CCC_Cd);
        }
        
        if (targetItems != null)
        {
            retCCC.AddRange(targetItems);
        }

        return retCCC.AsQueryable();
    }

    public IQueryable<object> GetBirth(eBirthType birthType)
    {
        List<object> arrBirth = new();

        var nowDT = DateTime.Now;

        int sValue = 0;
        int eValue = 0;

        if (birthType == eBirthType.Year)
        {

            sValue = nowDT.Year - 120;
            eValue = nowDT.Year;
        }
        else if (birthType == eBirthType.Month)
        {
            sValue = 1;
            eValue = 12;
        }
        else if (birthType == eBirthType.Day)
        {
            sValue = 1;
            eValue = 31;
        }

        for (int i = sValue; i <= eValue; i++)
        {
            arrBirth.Add(new
            {
                attrName = i,
                attrValue = i
            });
        }

        arrBirth.Reverse();

        return arrBirth.AsQueryable();
    }
}

public class PAT_ImageSourceToImageConverter : BaseConverter
{
    private static readonly BitmapImage DefaultImage = GlyphImage("Images/smartemr_patient_default_image.png");

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not byte[] bytes || bytes.Length == 0)
        {
            return DefaultImage;
        }

        try
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // 메모리 누수 방지 (중요!)
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze(); // UI 스레드 간 성능 최적화 및 크로스 스레드 예외 방지
                return image;
            }
        }
        catch
        {
            return DefaultImage;
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IntToBooleanConverter : BaseConverter
{
    public bool invert { get; set; } = false;

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return false;

        if (!Int32.TryParse(value.ToString(), out var intValue)) return false;

        var bFlag = intValue == 0 ? true: false;

        return invert ? !bFlag : bFlag;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class YNToBooleanConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var strValue = value?.ToString();
        if (string.IsNullOrWhiteSpace(strValue)) return default!;

        return strValue == "y" ? true : false;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var bFlag = (bool)value;

        return bFlag ? "y" : "n";
    }
}