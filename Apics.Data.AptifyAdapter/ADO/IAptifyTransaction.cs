using System;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{
    public interface IAptifyTransaction : IDisposable
    {
        string TransactionName { get; }
    }
}