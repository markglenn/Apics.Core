using System;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{
    public interface IAptifyTransaction
    {
        string TransactionName { get; }
    }
}