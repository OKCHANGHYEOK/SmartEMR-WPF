using Microsoft.Win32;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using SmartEMR.Application.Core;
using System.Diagnostics;
using System.IO;
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

    public static BitmapImage? GenerateBitmapImage(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            return null;
        }

        try
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // 스트림 해제 후 이미지 유지를 위해 필수
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze(); // 크로스 스레드 및 성능 최적화

                return bitmap;
            }
        }
        catch (Exception ex)
        {
            SmartUI.SetNofification("이미지 업로드중 오류가 발생했습니다.", NotificationType.Error);
            return null;
        }
    }

    public static byte[]? SelectImage()
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "이미지선택",
            Filter = "이미지 파일 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|모든 파일 (*.*)|*.*"
        };

        if (fileDialog.ShowDialog() == true)
        {
            string selectedFilePath = fileDialog.FileName;

            try
            {
                byte[] imageBytes = File.ReadAllBytes(selectedFilePath);

                if (imageBytes.Length > 0)
                {
                    SmartUI.SetNofification("이미지가 선택되었습니다.", NotificationType.Success);
                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                SmartUI.SetNofification("이미지 업로드에 실패했습니다. 다시 시도해주세요.", NotificationType.Error);
                return null;
            }
        }

        return null;
    }
}
