using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    [CustomRole(RoleTypeId.None)]
    internal class Maladroit : GlobalCustomRole
    {
        private static Dictionary<Player, CoroutineHandle> _coroutines = new();

        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu es <color=#FFFF00>maladroit</color>\nFait attention à tes items !";
        public override uint Id { get; set; } = 1057;
        public override string PublicName { get; set; } = "Maladroit";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;

        protected override void RoleAdded(Player player)
        {
            _coroutines.Add(player, Timing.RunCoroutine(ThrowingItem(player)));
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines(_coroutines[player]);
            _coroutines.Remove(player);
        }

        private IEnumerator<float> ThrowingItem(Player p)
        {            
            Dictionary<int, Action> ActionDictionnary = new()
            {
                { 50, () => p.DropHeldItem() },
                { 80, () => { /* Nothing */  } },
                { 95, () => DropItemFromInventory(p, 1) },
                { 100, () => DropItemFromInventory(p, 2) },
            };


            while (true)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(120f, 200f));
                int proba = UnityEngine.Random.Range(0, 101);
                
                foreach (var seuil in ActionDictionnary.Keys.OrderBy(p => p))
                {
                    if(proba < seuil)
                    {
                        ActionDictionnary[seuil]();
                        break;
                    }
                }
            }
        }

        private void DropItemFromInventory(Player p, int number)
        {
            for(int i = 0; i <= number; i++)
            {
                p.DropItem(p.Items.GetRandomValue());
            }
        }
    }
}