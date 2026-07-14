namespace SmartEMR.Application.ViewModels;

public class PatientHistoryViewModel : PatientViewModel
{
    public async Task UpdateHistoryBySelection(string targetHistoryType)
    {
        switch (targetHistoryType)
        {
            case "RES":
                break;

            case "RCP":
                break;

            case "CST":
                break;

            case "CORD":
                break;

            case "PAY":
                break;
        }
    }
}
