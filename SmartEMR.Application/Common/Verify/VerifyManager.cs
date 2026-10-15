using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Threading;

namespace SmartEMR.Application.Common.Verify;

public partial class VerifyManager : ObservableObject
{
    private DispatcherTimer? _verifyTimer;

    [ObservableProperty]
    private bool isRequestVerifyCode = false;

    [ObservableProperty]
    private TimeSpan verifylimitTime = TimeSpan.FromMinutes(10);
    [ObservableProperty]
    private string verifyLimitTimeText = "10:00";

    [ObservableProperty]
    private string identityVerificationCode = "";
    [ObservableProperty]
    private bool isIdentityVerified = false;

    public void UpdateIsIdentityVerified()
    {
        if (IsIdentityVerified)
        {
            IsIdentityVerified = false;
        }
    }

    public void StartVerifyTimer()
    {
        _verifyTimer?.Stop();

        VerifylimitTime = TimeSpan.FromMinutes(10);

        UpdateVerifyLimitTimeText();

        _verifyTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _verifyTimer.Tick += VerifyTimer_Tick;
        _verifyTimer.Start();
    }

    public void StopVerifyTimer()
    {
        _verifyTimer?.Stop();

        VerifylimitTime = TimeSpan.Zero;

        UpdateVerifyLimitTimeText();
    }

    private void UpdateVerifyLimitTimeText()
    {
        VerifyLimitTimeText = $"{(int)VerifylimitTime.TotalMinutes:D2}:{VerifylimitTime.Seconds:D2}";
    }

    private void VerifyTimer_Tick(object? sender, EventArgs e)
    {
        if (_verifyTimer is null) return;

        if (VerifylimitTime <= TimeSpan.Zero)
        {
            StopVerifyTimer();
            return;
        }

        VerifylimitTime -= TimeSpan.FromSeconds(1);

        UpdateVerifyLimitTimeText();
    }

    public void Clear()
    {
        IsRequestVerifyCode = false;
        IsIdentityVerified = false;

        IdentityVerificationCode = "";
        
        StopVerifyTimer();
    }
}
