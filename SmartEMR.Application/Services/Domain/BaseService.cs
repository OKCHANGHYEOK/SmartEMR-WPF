
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class BaseService
{
    protected readonly IDataStore _dataStore;
    
    public BaseService(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }
}
