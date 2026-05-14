using System.Diagnostics;
using System.IO;
using System.Text;

namespace SmartEMR.Application.Core;

public static class Logger
{
    private static readonly string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

    public static void WriteLog(Exception e)
    {
        try
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            string filePath = Path.Combine(dir, fileName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($" {e.Message}");
            sb.AppendLine($" {e.GetType().FullName}");
            sb.AppendLine($" {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("===============================================================");
            sb.AppendLine(e.StackTrace);
            sb.AppendLine(); 
            sb.AppendLine();

            File.AppendAllText(filePath, sb.ToString());

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
