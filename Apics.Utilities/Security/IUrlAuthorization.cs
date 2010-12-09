using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apics.Utilities.Security
{
    public interface IUrlAuthorization
    {
        Uri GenerateLink( Uri url );
        bool ValidateUrl( Uri url );
    }
}
