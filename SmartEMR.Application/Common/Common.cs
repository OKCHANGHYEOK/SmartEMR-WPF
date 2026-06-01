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

    public async Task<IQueryable<ChartCommonCode>> GetChartCommonCode(string CCCG_Cd = "", string CCC_Cd = "", bool isAll = false)
    {
        List<ChartCommonCode> arrCCC = new();

        if (isAll)
        {
            arrCCC.Add(new ChartCommonCode { CCC_Name = "전체", CCC_Cd = "ALL"});
        }

        var retCCC = await SmartMVVM.DataStore.GetItems<ChartCommonCode>(eAPI.ChartCommonCode_GetChartCommonCode, new ChartCommonCode { CCCG_Cd = CCCG_Cd, CCC_Cd = CCC_Cd});
        if (retCCC == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            Debug.WriteLine($"일치하는 코드값이 존재하지 않습니다. CCCG_Cd = {CCCG_Cd}, CCC_Cd = {CCC_Cd}");
            return arrCCC.AsQueryable();
        }

        arrCCC.AddRange(retCCC);

        return arrCCC.AsQueryable();
    }

    public IQueryable<int> GetBirth(eBirthType birthType)
    {
        List<int> arrBirth = new();

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
            sValue = 0;
            eValue = 12;
        }
        else if (birthType == eBirthType.Day)
        {
            sValue = 0;
            eValue = 31;
        }

        for (int i = sValue; i <= eValue; i++)
        {
            arrBirth.Add(i);
        }

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
