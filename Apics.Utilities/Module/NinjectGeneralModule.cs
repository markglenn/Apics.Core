using System;
using System.Linq;
using Ninject;
using Ninject.Modules;

namespace Apics.Utilities.Module
{
    public class NinjectGeneralModule : NinjectModule
    {
        public override void Load( )
        {
            // Bind the kernel
            Bind<IKernel>( ).ToConstant( Kernel );
        }
    }
}