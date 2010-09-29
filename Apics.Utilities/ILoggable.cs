using System;
using System.Linq;
using log4net;
using Ninject;

namespace Apics.Utilities
{
    public interface ILoggable
    {
        [Inject]
        ILog Log { get; set; }
    }
}