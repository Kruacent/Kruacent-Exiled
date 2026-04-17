using Exiled.API.Features;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features._914Upgrades.RoleChanging
{
    public class ClassD914RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.ClassD;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.Scientist,50f)}
        };

    }
    public class Scientist914RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scientist;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.ClassD,50f)}
        };

    }
    public class Guard914RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.FacilityGuard;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new RoleOutput(RoleTypeId.Scientist,100f)}
        };

        protected override void SetRole(Player player, RoleTypeId newRole)
        {
            player.ChangeRole(newRole, Exiled.API.Enums.SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
        }

    }
    public class MTF914RC : Multiple914PlayerRoleChangeBase
    {
        public override HashSet<RoleTypeId> InputRole => new HashSet<RoleTypeId>(){ RoleTypeId.NtfCaptain, RoleTypeId.NtfPrivate, RoleTypeId.NtfSergeant, RoleTypeId.NtfSpecialist};

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.ChaosRifleman,50f)}
        };

    }

    public class Chaos914RC : Multiple914PlayerRoleChangeBase
    {
        public override HashSet<RoleTypeId> InputRole => new HashSet<RoleTypeId>() { RoleTypeId.ChaosRifleman, RoleTypeId.ChaosRepressor, RoleTypeId.ChaosMarauder, RoleTypeId.ChaosConscript };

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new RoleOutput(RoleTypeId.NtfPrivate,50f)}
        };

    }
}
