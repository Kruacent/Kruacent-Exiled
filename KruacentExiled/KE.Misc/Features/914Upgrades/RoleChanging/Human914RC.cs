using Exiled.API.Features;
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
            { Scp914KnobSetting.OneToOne,new(RoleTypeId.Scientist,50f)}
        };

    }
    public class Scientist914RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.Scientist;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new(RoleTypeId.ClassD,50f)}
        };

    }
    public class Guard914RC : Base914PlayerRoleChange
    {
        public override RoleTypeId InputRole => RoleTypeId.FacilityGuard;

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.Rough,new(RoleTypeId.Scientist,100f)}
        };

        protected override void SetRole(Player player, RoleTypeId newRole)
        {
            player.Role.Set(newRole, RoleSpawnFlags.AssignInventory);
            Timing.CallDelayed(.5f, () =>
            {
                _upgradingPlayer.Remove(player);
            });
        }

    }
    public class MTF914RC : Multiple914PlayerRoleChangeBase
    {
        public override HashSet<RoleTypeId> InputRole => [RoleTypeId.NtfCaptain,RoleTypeId.NtfPrivate,RoleTypeId.NtfSergeant,RoleTypeId.NtfSpecialist];

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new(RoleTypeId.ChaosRifleman,50f)}
        };

    }

    public class Chaos914RC : Multiple914PlayerRoleChangeBase
    {
        public override HashSet<RoleTypeId> InputRole => [RoleTypeId.ChaosRifleman, RoleTypeId.ChaosRepressor, RoleTypeId.ChaosMarauder, RoleTypeId.ChaosConscript];

        public override IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; } = new Dictionary<Scp914KnobSetting, RoleOutput>()
        {
            { Scp914KnobSetting.OneToOne,new(RoleTypeId.ChaosRifleman,50f)}
        };

    }
}
