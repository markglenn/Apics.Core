using System;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{
    internal interface IAptifyTransaction : IDisposable
    {
        string TransactionName { get; }
    }
}