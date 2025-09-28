using KE.Items.API.Core.Upgrade;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.API.Interface
{
    public interface IUpgradableCustomItem
    {
        IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; }
    }
}
