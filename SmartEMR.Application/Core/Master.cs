using System.Collections.ObjectModel;

namespace SmartEMR.Application.Core;

public class Master
{
    private readonly Dictionary<string, List<string>> _arrMasterRequest = new();

    public IReadOnlyDictionary<string, ReadOnlyCollection<string>> arrMasterRequest =>
        _arrMasterRequest.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly());

    public Master()
    {
        Intialize();
    }

    public void Intialize()
    {
        _arrMasterRequest.Clear();

        // Patient
        AddMasterRequest("PAT_IsForegin", "y");
    }

    private void AddMasterRequest(string name, string value)
    {

    }
}
