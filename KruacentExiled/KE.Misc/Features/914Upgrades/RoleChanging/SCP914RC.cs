using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features._914Upgrades.RoleChanging
{
    public class Scp096RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp096;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp3114,50f)},
            { Scp914KnobSetting.VeryFine,new RoleOutput(RoleTypeId.Scp939,50f)}
        };

    }
    public class Scp3114RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp3114;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp096,50f)},
            { Scp914KnobSetting.VeryFine,new RoleOutput(RoleTypeId.Scp173,50f)}
        };

    }
    public class Scp939RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp939;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new RoleOutput(RoleTypeId.Scp096,50f)},
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp173,50f)},
            { Scp914KnobSetting.VeryFine,new RoleOutput(RoleTypeId.Scp049,50f)},
        };

    }

    public class Scp173RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp173;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new RoleOutput(RoleTypeId.Scp096,50f)},
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp939,50f)},
            { Scp914KnobSetting.VeryFine,new RoleOutput(RoleTypeId.Scp106,50f)},
        };

    }

    public class Scp106RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp106;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new RoleOutput(RoleTypeId.Scp173,50f)},
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp049,50f)},
        };

    }
    public class Scp049RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scp049;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new RoleOutput(RoleTypeId.Scp939,50f)},
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scp106,50f)},
        };

    }
}
