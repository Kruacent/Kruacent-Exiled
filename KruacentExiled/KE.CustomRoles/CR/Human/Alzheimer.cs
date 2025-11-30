using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Alzheimer : GlobalCustomRole, IColor, IHealable
    {
        private static Dictionary<Player, CoroutineHandle> _coroutines = new();
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "POV Mishima";
        public override string PublicName { get; set; } = "Vieux";
        public override string InternalName => GetType().Name;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public Color32 Color => new Color32(112,112,112,0);

        public HashSet<ItemType> HealItem => [ItemType.SCP500];

        protected override void RoleAdded(Player player)
        {
            _coroutines.Add(player, Timing.RunCoroutine(Teleport(player)));
        }




        protected override void RoleRemoved(Player player)
        {
            if (!_coroutines.ContainsKey(player)) return;



            Timing.KillCoroutines(_coroutines[player]);
            _coroutines.Remove(player);
        }

        private IEnumerator<float> Teleport(Player p)
        {
            while (p.IsAlive)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(300f, 600f));
                p.EnableEffect(EffectType.Flashed,1,5);
                p.EnableEffect(EffectType.Invisible,1,6);
                p.Teleport(Utils.Extensions.RoomExtensions.RandomSafeRoom(p.Zone));
            }
        }


    }
}
