using System.ComponentModel;

namespace SmartEMR.Application.Enums;

public enum eSmartEMRLocation
{
    RES = 0,
    DSK = 1,
    CST = 2,
    PAY = 3,
    USR = 4,
    CNF = 5
}

public enum ePatientHistoryType
{
    RES,
    RCP,
    CST,
    CSTO,
    PAY
}

public enum eBirthType
{
    Year,
    Month,
    Day
}

public enum SaveMode
{
    SAVE,
    DELETE
}

public enum FromViewType
{
    VIEW,
    POPUP
}

public enum ConsultationStatus
{
    RDY,
    PND,
    ING,
    END
}

public enum CopaymentType
{
    General,
    Senior
}

public enum DuplicateResultCode
{
    EmptyInput,
    UnValidInput,
    ErrorOccured,
    HasDuplicate,
    NotDuplicate
}

public enum MemberUserRole
{
    [Description("ADM")]
    ADM,
    [Description("USR")]
    USR
}

public enum MemberUserDepartment
{
    [Description("MIS")]
    MIS,
    [Description("ADM")]
    ADM,
    [Description("MED")]
    MED
}