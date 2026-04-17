using System.Windows;

namespace SmartEMR.Application.Services;

public class DialogService
{
    public static bool ShowConfirm(string title, string msg)
    {
        // 상속 구조와 상관없이 현재 활성화된 윈도우 위에 띄웁니다.
        var msgBox = MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);

        return msgBox == MessageBoxResult.OK;
    }
}
