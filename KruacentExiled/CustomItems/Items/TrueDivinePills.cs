using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.Extensions;
using PlayerRoles;
using ProjectMER.Features.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerHandle = Exiled.Events.Handlers.Player;

namespace KruacentExiled.CustomItems.Items
{
    public class TrueDivinePills : KECustomItem, ILumosItem, IRevivingCustomItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "True Divine Pills",
                    [TranslationKeyDesc] = "Guaranteed to respawn everybody, drop to change the mode",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "True Divine Pills",
                    [TranslationKeyDesc] = "Fait réappaître tout le monde, lâcher pour changer le mode",
                },
            };
        }

        public override ItemType ItemType => ItemType.SCP500;

        /// <inheritdoc/>
        public override string Name { get; set; } = "TrueDivinePills";

        /// <inheritdoc/>

        /// <inheritdoc/>
        public override float Weight { get; set; } = 0.65f;
        public override ItemRank Rank { get; } = ItemRank.Exotic;
        public Color Color { get; set; } = Color.yellow;
        private Dictionary<ushort, bool> TpOnPlayer = new Dictionary<ushort, bool>();

        /// <inheritdoc/>
        public override SpawnProperties SpawnProperties { get; set; } = new KESpawnProperties()
        {

        };

        /// <inheritdoc/>
        protected override void SubscribeEvents()
        {
            PlayerHandle.UsingItem += OnUsingItem;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            PlayerHandle.UsingItem -= OnUsingItem;
            base.UnsubscribeEvents();
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;

            if (ev.IsThrown)
            {
                return;
            }
            Player player = ev.Player;

            TpOnPlayer.TryGetValue(ev.Item.Serial, out bool spawnToPlayer);
            TpOnPlayer[ev.Item.Serial] = !spawnToPlayer;

            if (!spawnToPlayer)
                KECustomItem.ItemEffectHint(player, "Players will spawn to you");
            else
                KECustomItem.ItemEffectHint(player, "Players won't spawn to you");
            ev.IsAllowed = false;
        }



        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;
            Player player = ev.Player;

            List<Player> spectators = Player.Enumerable.Where(p => p.IsDead && !p.IsOverwatchEnabled).ToList();

            if (spectators.Count == 0)
            {
                KECustomItem.ItemEffectHint(player, "No one to respawn");
                ev.IsAllowed = false;
                return;
            }


            foreach (Player spectator in spectators)
            {
                bool revived = Revive(player, spectator);
                if (revived && TpOnPlayer.TryGetValue(ev.Item.Serial, out bool tp) && tp)
                {
                    spectator.Teleport(player);
                }


            }

        }

        public bool Revive(Player reviver, Player deadPlayer)
        {
            switch (reviver.Role.Side)
            {
                case Side.ChaosInsurgency:
                    deadPlayer.ChangeRole(RoleTypeId.ChaosRifleman, SpawnReason.ForceClass, RoleSpawnFlags.All);
                    return true;
                case Side.Mtf:
                    deadPlayer.ChangeRole(RoleTypeId.NtfPrivate, SpawnReason.ForceClass, RoleSpawnFlags.All);
                    return true;
            }
            return false;

        }
    }
}