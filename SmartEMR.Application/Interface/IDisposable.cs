namespace SmartEMR.Application.Interface;

public interface IDisposable 
{
    bool disposed { get; set; }

    void Dispose(bool disposedValue); 
}
