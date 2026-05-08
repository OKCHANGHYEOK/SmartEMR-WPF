using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartEMR.Application.Common;

public static class Module
{
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

    public static DrawingImage? GlyphSvgToImage(string filePath)
    {
        try
        {
            if (!System.IO.File.Exists(filePath))
            {
                Debug.WriteLine($"[에러] 실제 폴더에 파일이 없습니다.");
                return null;
            }

            var settings = new WpfDrawingSettings { IncludeRuntime = true, TextAsGeometry = true };
            var reader = new FileSvgReader(settings);
            var drawingGroup = reader.Read(filePath);

            return new DrawingImage(drawingGroup);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SVG 로드 실패: {ex.Message}");
            return null;
        }
    }
}
