using SmartEMR.Application.Common.Processor;

namespace SmartEMR.Application.Common;

public class ProcessorProvider
{
    public ReceptionBoardProcessor ReceptionBoardProcessor = new();
    public ReceptionProcessor ReceptionProcessor = new();
}
