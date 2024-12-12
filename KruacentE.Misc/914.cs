using Exiled.API.Enums;
using Exiled.Events.EventArgs.Scp914;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace KruacentExiled.KruacentE.Misc
{
    internal class _914
    {
        internal Dictionary<int, RoleTypeId> roleScp = new Dictionary<int, RoleTypeId>()
        {
            { -1, RoleTypeId.Scp049 },
            { -2, RoleTypeId.Scp939 },
            { -3, RoleTypeId.Scp096 },

            { 1, RoleTypeId.Scp106 },
            { 2, RoleTypeId.Scp173 },
            { 3, RoleTypeId.Scp3114},

        };
        internal void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Teleport(ev.Player, ev.KnobSetting);

            ChangingRole(ev.Player, ev.KnobSetting);
        }


        private void Teleport(Player p, Scp914KnobSetting knob)
        {
            if (knob == Scp914KnobSetting.Fine && UnityEngine.Random.value < .01f)
                p.Teleport(Room.Random(ZoneType.Entrance));
            if (knob == Scp914KnobSetting.Coarse && UnityEngine.Random.value < .25f)
                p.Teleport(Room.Random(ZoneType.LightContainment));
        }

        private void ChangingRole(Player p, Scp914KnobSetting knob)
        {
            if (p.IsScp)
            {
                HandleScpChangingRole(p, knob);
            }
            else if (p.IsHuman)
            {
                HandleHumanChangingRole(p, knob);
            }
        }

        private void HandleHumanChangingRole(Player p, Scp914KnobSetting knob)
        {
            if (p.IsHuman)
            {
                switch (p.Role.Type)
                {
                    case RoleTypeId.Scientist:
                        if (UnityEngine.Random.value < .5f)
                            p.Role.Set(RoleTypeId.ClassD);
                        break;
                    case RoleTypeId.ClassD:
                        if (UnityEngine.Random.value < .5f)
                            p.Role.Set(RoleTypeId.Scientist);
                        break;
                    case RoleTypeId.FacilityGuard:
                        if (knob == Scp914KnobSetting.Rough)
                            p.Role.Set(RoleTypeId.FacilityGuard);
                        break;
                }
            }
        }

        private void HandleScpChangingRole(Player p, Scp914KnobSetting knob)
        {

            if (p.IsScp)
            {
                var invertRoleScp = roleScp.ToDictionary(k => k.Value, v => v.Key);
                if (UnityEngine.Random.value < .5f)
                {
                    // get the id of the scp
                    if (invertRoleScp.TryGetValue(p.Role.Type, out int key))
                    {
                        RoleTypeId newRole;
                        switch (knob)
                        {
                            //going up in the graph
                            case Scp914KnobSetting.Rough:
                                if (roleScp.TryGetValue(key - 1, out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                            //going horizontaly in the graph
                            case Scp914KnobSetting.OneToOne:
                                if (roleScp.TryGetValue(key * (-1), out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                            //going down in the graph
                            case Scp914KnobSetting.VeryFine:
                                if (roleScp.TryGetValue(key + 1, out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                        }
                    }

                }
            }
        }

    }
}
