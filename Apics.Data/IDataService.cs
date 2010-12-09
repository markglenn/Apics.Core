using System;
namespace Apics.Data
{
    public interface IDataService : IDisposable
    {
        IDataStore DataStore { get; }
    }
}
