using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apics.Utilities.Security
{
    public interface IUrlAuthorization
    {
        string GenerateLink( string url );
        bool ValidateUrl( string url );
    }
}
