using System;
using System.Linq;

namespace Apics.Data.Dialect
{
    public interface IDialect
    {
        string Name { get; }
        string Dialect { get; }
        string DriverClass { get; }
    }
}
