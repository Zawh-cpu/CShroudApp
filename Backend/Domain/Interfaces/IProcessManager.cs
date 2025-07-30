namespace Backend.Domain.Interfaces;

public interface IProcessManager
{
    void Register(IProcess process);
    void KillAll();
    
    Task KillAllAsync();
}