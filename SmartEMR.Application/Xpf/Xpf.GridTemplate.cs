namespace SmartEMR.Application.Xpf;

public abstract class GridTemplate : CustomControl
{
    protected GridTemplate()
    {
        Initialize();
    }

    public abstract void Initialize();
}