using System.Diagnostics;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Processes;

namespace Backend.Application.Factories;

public class ProcessFactory
{
    private readonly IProcessManager _processManager;
    
    public ProcessFactory(IProcessManager processManager)
    {
        _processManager = processManager;
    }
    
    public BaseProcess Create(ProcessStartInfo processStartInfo, LogLevelMode debug = LogLevelMode.Off)
    {
        var process = new BaseProcess(processStartInfo, _processManager, debug);
        return process;
    }
}