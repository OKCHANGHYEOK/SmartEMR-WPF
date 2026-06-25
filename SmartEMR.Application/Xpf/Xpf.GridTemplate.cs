namespace SmartEMR.Application.Xpf;

public abstract class GridTemplate : CustomControl
{
    public object? DataItem => DataContext;

    protected GridTemplate()
    {
        Initialize();
    }

    public abstract void Initialize();
}