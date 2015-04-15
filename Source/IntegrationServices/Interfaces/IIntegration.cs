using Core;

namespace IntegrationServices.Interfaces
{

    //TODO: This really should probably go into contract or core or something like that.
    public interface IIntegration : IDependency
    {
        void Execute(string[] args);
        string IntegrationName { get; }
        string Directions { get; }
    }
}
