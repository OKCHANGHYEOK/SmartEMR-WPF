namespace SmartEMR.Domain.Entities;

public class Chart : BaseEntity
{
    public int? CHT_Idx { get; set; }
    public int? MEM_Idx { get; set; }
    public int? PAT_Idx { get; set; }
    public int? MUR_Idx_DOC { get; set; }
    public int? MUR_Idx_STF { get; set; }
    public string? PAT_ChartNo { get; set; }
    public string? PAT_Name { get; set; }
    public string? PAT_Sex { get; set; }
    public int? PAT_Age { get; set; }
    public string? vPAT_Info { get; set; }
    public int? CHT_VisitType { get; set; }
    public string? CHT_CHTType { get; set; }
    public string? vCHT_CHTType { get; set; }
    public string? CHT_Status { get; set; }
    public string? CHT_Order { get; set; }
    public string? CHT_Route { get; set; }
    public string? CHT_Subject { get; set; }
    public string? CHT_SubjectName { get; set; }
    public string? CHT_InsuranceType { get; set; }
    public string? CHT_MainSymptom { get; set; }
    public string? CHT_Diagnosis { get; set; }
    public string? CHT_StartDate { get; set; }  
    public string? CHT_EndDate { get; set; }
    public double? CHT_TotalPrice { get; set; }
    public string? CHT_CHTTime { get; set; }
    public string? CHT_Date { get; set; }
    public string? CHT_YYMMDD { get; set; }
    public bool? CHT_IsValid { get; set; }
}
