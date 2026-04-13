namespace SmartEMR.Domain.Entities;

public class Member : BaseEntity
{
    public int? MEM_Idx { get; set; }
    public int? MEM_AdminUser { get; set; }
    public string? MEM_Name { get; set; }
    public string? MEM_MediNo { get; set; }
    public string? MEM_BizNum { get; set; }
    public string? MEM_BizType { get; set; }
    public string? MEM_Address1 { get; set; }
    public string? MEM_Address2 { get; set; }
    public string? MEM_Address3 { get; set; }
    public string? MEM_Tel1 { get; set; } 
    public string? MEM_Tel2 { get; set; }
    public string? MEM_Tel3 { get; set; }
    public string? MEM_StartDate { get; set; }
    public string? MEM_EndDate { get; set; }
    public int? MEM_OperationStatus { get; set; }
    public string? MEM_Date { get; set; }
    public string? MEM_YYMMDD { get; set; }
    public bool? MEM_IsValid { get; set; }
}
