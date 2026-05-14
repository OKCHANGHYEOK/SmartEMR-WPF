namespace SmartEMR.Domain.Entities;

public class Patient : BaseEntity
{
    public int PAT_Idx { get; set; }
    public int MEM_Idx { get; set; }
    public int MUR_Idx { get; set; }
    public string? PAT_BloodType { get; set; }
    public string? PAT_SourceType { get; set; }
    public string? PAT_Name { get; set; }
    public string? PAT_ChartNo { get; set; }
    public string? PAT_Sex { get; set; }
    public int? PAT_Age { get; set; }
    public string? vPAT_Info { get; set; }
    public string? PAT_BirthYear { get; set; }
    public string? PAT_BirthMonth { get; set; }
    public string? PAT_BirthDay { get; set; }
    public string? PAT_BirthDate { get; set; }
    public string? PAT_RegisterNum1 { get; set; }
    public string? PAT_RegisterNum2 { get; set; }
    public string? PAT_Hpp1 { get; set; }
    public string? PAT_Hpp2 { get; set; }
    public string? PAT_Hpp3 { get; set; }
    public string? PAT_PhoneNum { get; set; }
    public string? PAT_Address1 { get; set; }
    public string? PAT_Address2 { get; set; }
    public string? PAT_Address3 { get; set; }
    public string? PAT_Email { get; set; }
    public string? PAT_FirstVisitDate { get; set; }
    public string? PAT_LastVisitDate { get; set; }
    public string? PAT_IsSolar { get; set; }
    public string? PAT_IsAgreePersonalInfo { get; set; }
    public string? PAT_IsForeign { get; set; }
    public string? PAT_IsSMS { get; set; }
    public string? PAT_IsEmail { get; set; }
    public byte[]? PAT_ImageSource { get; set; }
    public string? PAT_Date { get; set; }
    public string? PAT_YYMMDD { get; set; }
    public bool? PAT_IsValid { get; set; }
    public string? PAT_Log { get; set; }
}
