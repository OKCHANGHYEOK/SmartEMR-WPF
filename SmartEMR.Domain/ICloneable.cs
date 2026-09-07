using SmartEMR.Domain.Entities;

namespace SmartEMR.Domain;

internal interface ICloneable<T> where T : BaseEntity
{
    public T Clone();
}
