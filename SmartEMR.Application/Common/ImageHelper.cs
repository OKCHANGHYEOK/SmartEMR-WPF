using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Media;
using SharpVectors.Renderers.Wpf;
using SharpVectors.Converters;

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

    public static DrawingImage? GlyphSvgToImage(string filePath)
    {
        try
        {
            //// 1. 실행 파일(.exe)이 있는 폴더 위치를 가져옵니다.
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            //// 2. 경로 조합 (슬래시 방향 등을 OS에 맞게 알아서 처리해줍니다)
            //// 속성창에 보이는 구조대로라면 Images 폴더 안의 Svg 폴더입니다.
            //string filePath = System.IO.Path.Combine(baseDir, "Images", "Svg", svgFileName);

            //// 디버깅용: 출력창에 찍힌 이 경로를 복사해서 탐색기에 붙여넣어보세요.
            //Debug.WriteLine($"실제 찾는 경로: {filePath}");

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
