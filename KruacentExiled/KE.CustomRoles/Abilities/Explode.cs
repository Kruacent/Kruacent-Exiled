using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Explode : KEAbilities
    {
        public override string Name { get; } = "Explode";
        public override string PublicName { get; } = "Explode";

        public override string Description { get; } = "Tu as une ceinture d'explosif autour de toi, fait attention n'appuie pas sur le bouton";

        public override int Id => 2007;

        public override float Cooldown { get; } = 4*60f;

        protected override void AbilityUsed(Player player)
        {
            ExplosiveGrenade grenade = ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE));
            grenade.FuseTime = 0.2f;
            grenade.SpawnActive(player.Position);
            Log.Debug("Grenade spawned");
        }
    }
}
