using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Interfaces
{

    /// <summary>
    /// SCP custom role implementing this interface will allow them to have a preference slider like the vanilla scps
    /// </summary>
    public interface ISCPPreferences
    {

        public abstract bool IsSupport { get; }
    }
}
