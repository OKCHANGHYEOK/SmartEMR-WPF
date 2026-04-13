using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace SmartEMR.Application.Common;

public static class ImageHelper
{
    /// <summary>
    /// URI 경로를 받아 비트맵이미지를 생성하여 반환합니다.
    /// </summary>
    public static BitmapImage GlyphImage(string path)
    {
        try
        {
            var uri = new Uri($"pack://application:,,,/{path}", UriKind.Absolute);

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"해당 이미지 파일을 찾을 수 없습니다. {path}");
            return new BitmapImage();
        }
    }
}
