namespace SmartEMR.Application.Common
{
    public interface IDisposable 
    {
        bool disposed { get; set; }

        void Dispose(bool disposedValue); 
    }
}
