using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Processor;

public interface IQueryResultListProcessor<T> where T : BaseEntity
{
    void Process(IEnumerable<T> items);
}
