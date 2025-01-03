﻿using Exiled.API.Enums;
using Exiled.Events.EventArgs.Scp914;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Extensions;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KE.Misc
{
    /// <summary>
    /// Everything 914 related
    /// </summary>
    internal class _914
    {

        internal Dictionary<int, RoleTypeId> roleScp = new Dictionary<int, RoleTypeId>()
        {
            { -1, RoleTypeId.Scp049 },
            { -2, RoleTypeId.Scp939 },
            { -3, RoleTypeId.Scp096 },

            { 3, RoleTypeId.Scp106 },
            { 2, RoleTypeId.Scp173 },
            { 1, RoleTypeId.Scp3114},

        };
        internal void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Log.Debug("upgrading player");
            Teleport(ev.Player, ev.KnobSetting);

            ChangingRole(ev.Player, ev.KnobSetting);
        }


        /// <summary>
        /// Teleport the player in a random place specified by the knob
        /// Coarse -> 1/4 chance to tp in Lcz -> 1/10 switch place with the scp
        /// Fine -> 1/100 chance to tp in Entrance or Hcz
        /// </summary>
        /// <param name="p"> the player being teleported</param>
        /// <param name="knob"> the knob setting of 914</param>
        private void Teleport(Player p, Scp914KnobSetting knob)
        {
            if (knob == Scp914KnobSetting.Fine && UnityEngine.Random.value < .01f)
            {
                if (UnityEngine.Random.value < .5f)
                    p.Teleport(Room.Random(ZoneType.Entrance));
                else
                    p.Teleport(Room.Random(ZoneType.HeavyContainment));
            }

            if (knob == Scp914KnobSetting.Coarse && UnityEngine.Random.value < .25f)
            {
                if (UnityEngine.Random.value < .10f && Player.List.Any(player => player.IsScp))
                {
                    Player playerScp = Player.List.ToList().Where(pl => pl.IsScp).GetRandomValue();
                    var pos = p.Position;
                    p.Teleport(playerScp.Position);
                    playerScp.Teleport(pos);
                    
                }
                else
                    p.Teleport(Room.Random(ZoneType.LightContainment));
            }
        }
        /// <summary>
        /// Changing the role of a player
        /// </summary>
        /// <param name="p"> the player to change the role</param>
        /// <param name="knob"> the knob setting of knob</param>
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
        /// <summary>
        /// Handle the change of role if the player is human
        /// </summary>
        /// <param name="p"> the human player </param>
        /// <param name="knob"> the knob setting of 914</param>
        private void HandleHumanChangingRole(Player p, Scp914KnobSetting knob)
        {
            if (p.IsHuman)
            {
                switch (p.Role.Type)
                {
                    case RoleTypeId.Scientist:
                        if (UnityEngine.Random.value < .5f && knob == Scp914KnobSetting.OneToOne)
                            p.Role.Set(RoleTypeId.ClassD);
                        break;
                    case RoleTypeId.ClassD:
                        if (UnityEngine.Random.value < .5f && knob == Scp914KnobSetting.OneToOne)
                            p.Role.Set(RoleTypeId.Scientist);
                        break;
                    case RoleTypeId.FacilityGuard:
                        if (knob == Scp914KnobSetting.Rough)
                            p.Role.Set(RoleTypeId.FacilityGuard);
                        break;
                }
            }
        }

        /// <summary>
        /// Handle the change of role if the player is a scp
        /// </summary>
        /// <param name="p"> the scp player (not a zombie)</param>
        /// <param name="knob"> the knob setting of 914</param>
        private void HandleScpChangingRole(Player p, Scp914KnobSetting knob)
        {

            if (p.IsScp && p.Role.Type != RoleTypeId.Scp0492)
            {
                var invertRoleScp = roleScp.ToDictionary(k => k.Value, v => v.Key);
                if (UnityEngine.Random.value < .5f)
                {
                    // get the id of the scp
                    if (invertRoleScp.TryGetValue(p.Role.Type, out int key))
                    {
                        switch (knob)
                        {
                            //going up in the graph
                            case Scp914KnobSetting.Rough:
                                TrySetRole(p, key - 1);
                                break;
                            //going horizontaly in the graph
                            case Scp914KnobSetting.OneToOne:
                                switch (Math.Abs(key))
                                {
                                    case 3:
                                        TrySetRole(p,key/(-3));
                                        break;
                                    case 2:
                                        TrySetRole(p,key* (-1));
                                        break;
                                    case 1:
                                        TrySetRole(p, key * (-3));
                                        break;

                                }
                                
                                break;
                            //going down in the graph
                            case Scp914KnobSetting.VeryFine:
                                TrySetRole(p, key + 1);
                                break;
                        }
                    }

                }
                else 
                {
                    Log.Debug("no luck");
                }
            }
        }



        private void TrySetRole(Player p ,int key)
        {
            RoleTypeId newRole;
            if (roleScp.TryGetValue(key, out newRole))
            {
                p.Role.Set(newRole);
            }
        }
    }
}
