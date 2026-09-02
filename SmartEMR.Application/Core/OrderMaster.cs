namespace SmartEMR.Application.Core;

public class OrderMaster
{
    public static string[] ORDER_ASSESSMENTS = { ORDER_CLINIC_ASM_FIR, ORDER_CLINIC_ASM_REP, ORDER_HOSPITAL_ASM_FIR, ORDER_HOSPITAL_ASM_REP };

    public const string ORDER_CLINIC_ASM_FIR = "AA154";                 //  초진진찰료 - 의원
    public const string ORDER_CLINIC_ASM_REP = "AA254";                 //  재진진찰료 - 의원
    public const string ORDER_HOSPITAL_ASM_FIR = "AA155";               //  초진진찰료 - 병원
    public const string ORDER_HOSPITAL_ASM_REP = "AA255";               //  재진진찰료 - 병원
    public const string ORDER_EXM_BLOOD = "BZ001";                      //  일반혈액검사
    public const string ORDER_EXM_URINE = "CZ322";                      //  뇨(소변)검사
    public const string ORDER_MED_SCIM_INJECTION = "KK010";             //  피하 또는 근육내주사
    public const string ORDER_MED_IV_INJECTION = "KK020";               //  정맥내일시주사
    public const string ORDER_MED_IV_INFUSION_UNDER_100 = "KK051";      //  정맥내점적주사 100ml 미만
    public const string ORDER_MED_IV_INFUSION_100_TO_500 = "KK052";     //  정맥내점적주사 100ml ~ 500ml
    public const string ORDER_MED_IV_INFUSION_501_TO_1000 = "KK053";    //  정맥내점적주사 501ml ~ 1000ml
    public const string ORDER_MED_IV_SIDE_INJECTION = "KK054";          //  수액제주입로를통한주사
    public const string ORDER_MED_INTRA_ARTICULAR_INJECTION = "KK090";  //  관절강내주사
    public const string ORDER_TRT_SIMPLE = "M0111";                     //  단순처치
    public const string ORDER_TRT_INFECTED = "M0121";                   //  염증성처치
    public const string ORDER_TRT_SDTS = "M0137";                       //  흡입배농 및 배액처치
    public const string ORDER_TRT_BURN = "N0111";                       //  화상처치
    public const string ORDER_TRT_SKIN = "N0181";                       // 피부과처치
}
