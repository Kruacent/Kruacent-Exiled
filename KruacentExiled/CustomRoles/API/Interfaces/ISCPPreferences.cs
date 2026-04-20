using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.API.Interfaces
{

    /// <summary>
    /// SCP custom role implementing this interface will allow them to have a preference slider like the vanilla scps
    /// </summary>
    public interface ISCPPreferences
    {

        string SCPId { get; }
        bool IsSupport { get; }

        void Set(Player player);

        int GetPreferences(Player player);
    }
}
