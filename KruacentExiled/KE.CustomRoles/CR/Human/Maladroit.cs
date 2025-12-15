using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
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
    public class Maladroit : GlobalCustomRole, IColor
    {
        private static Dictionary<Player, CoroutineHandle> _coroutines = new();

        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Fait attention à \"tes\" items !";
        public override string PublicName { get; set; } = "Maladroit Vole";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public Color32 Color => new(211, 110, 112, 0);

        public override HashSet<string> Abilities { get; } = new()
        {
            "Thief"
        };
        protected override void RoleAdded(Player player)
        {
            _coroutines.Add(player, Timing.RunCoroutine(ThrowingItem(player)));
        }

        protected override void RoleRemoved(Player player)
        {
            if (!_coroutines.ContainsKey(player)) return;
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


            while (p.IsAlive)
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