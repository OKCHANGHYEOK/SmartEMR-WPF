using SmartEMR.Application.Common.Converter;
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
