using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    [CustomRole(RoleTypeId.None)]
    internal class Alzheimer : GlobalCustomRole
    {
        private static Dictionary<Player, CoroutineHandle> _coroutines = new();
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Name { get; set; } = "Alzheimer";
        public override string Description { get; set; } = "Tu es <color=#0f0f0f>Vieux</color>";
        public override uint Id { get; set; } = 1056;
        public override string CustomInfo { get; set; } = "Vieux";
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        protected override void RoleAdded(Player player)
        {
            _coroutines.Add(player, Timing.RunCoroutine(Teleport(player)));
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines(_coroutines[player]);
            _coroutines.Remove(player);
        }


        private IEnumerator<float> Teleport(Player p)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(300f, 600f));
                p.EnableEffect(EffectType.Flashed,5);
                p.EnableEffect(EffectType.Invisible,5);
                p.Teleport(Room.Random(p.Zone));
            }
        }


    }
}
