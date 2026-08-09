using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public interface IDisplayDataMapper<T> where T : BaseEntity
{
    void Map(IEnumerable<T> items);
}
