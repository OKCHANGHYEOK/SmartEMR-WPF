namespace SmartEMR.Domain.Entities;

public class EmailVerification : BaseEntity
{
    private int? m_EV_Idx;
    private string? m_EV_Purpose;
    private string? m_EV_Email;
    private string? m_EV_CodeOrigin;
    private string? m_EV_CodeHash;
    private string? m_EV_CreatedAt;
    private string? m_EV_ExpiredAt;
    private int? m_EV_AttemptCount;
    private bool? m_EV_IsVerified;
    private bool? m_EV_IsValid;

    #region "NotifyPropertyChanged"
    public int? EV_Idx
    {
        get => m_EV_Idx;
        set => SetProperty(ref m_EV_Idx, value);
    }

    public string? EV_Purpose
    {
        get => m_EV_Purpose;
        set => SetProperty(ref m_EV_Purpose, value);
    }

    public string? EV_Email
    {
        get => m_EV_Email;
        set => SetProperty(ref m_EV_Email, value);
    }

    public string? EV_CodeOrigin
    {
        get => m_EV_CodeOrigin;
        set => SetProperty(ref m_EV_CodeOrigin, value);
    }

    public string? EV_CodeHash
    {
        get => m_EV_CodeHash;
        set => SetProperty(ref m_EV_CodeHash, value);
    }

    public string? EV_CreatedAt
    {
        get => m_EV_CreatedAt;
        set => SetProperty(ref m_EV_CreatedAt, value);
    }

    public string? EV_ExpiredAt
    {
        get => m_EV_ExpiredAt;
        set => SetProperty(ref m_EV_ExpiredAt, value);
    }

    public int? EV_AttemptCount
    {
        get => m_EV_AttemptCount;
        set => SetProperty(ref m_EV_AttemptCount, value);
    }

    public bool? EV_IsVerified
    {
        get => m_EV_IsVerified;
        set => SetProperty(ref m_EV_IsVerified, value);
    }

    public bool? EV_IsValid
    {
        get => m_EV_IsValid;
        set => SetProperty(ref m_EV_IsValid, value);
    }

    #endregion
}
