using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.Commands;
using Exiled.Events.EventArgs.Player;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomRoles.CR.MTF.Medic;
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
        private const string PlayerWillSpawnToYou = "PlayerWillSpawnToYou";
        private const string PlayerWontSpawnToYou = "PlayerWontSpawnToYou";
        private const string NoOneToRevive = "NoOneToRevive";
        private const string SummonedPlayer = "SummonedPlayer";

        private int _numberOfPlayerSpawned; 

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "True Divine Pills",
                    [TranslationKeyDesc] = "Guaranteed to respawn everybody, drop to change the mode",
                    [PlayerWillSpawnToYou] = "Players will spawn to you",
                    [PlayerWontSpawnToYou] = "Players won't spawn to you",
                    [NoOneToRevive] = "No one to respawn",
                    [SummonedPlayer] = $"You summoned %NumberOfPlayer% player",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "True Divine Pills",
                    [TranslationKeyDesc] = "Fait réappaître tout le monde, lâcher pour changer le mode",
                    [PlayerWillSpawnToYou] = "Les joueurs vont apparaître sur toi",
                    [PlayerWontSpawnToYou] = "Les joueurs ne vont pas apparaître sur toi",
                    [NoOneToRevive] = "Pas de joueur à faire réapparaître",
                    [SummonedPlayer] = $"Tu as fait réapparaître %NumberOfPlayer%",
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
                KECustomItem.ItemEffectHint(player, GetTranslation(player, PlayerWillSpawnToYou));
            else
                KECustomItem.ItemEffectHint(player, GetTranslation(player, PlayerWontSpawnToYou));
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
                KECustomItem.ItemEffectHint(player, GetTranslation(player, NoOneToRevive));
                ev.IsAllowed = false;
                return;
            }

            ev.Item.Destroy();
            int i = 0;

            foreach (Player spectator in spectators)
            {
                bool revived = Revive(player, spectator);
                if (revived)
                {
                    if(TpOnPlayer.TryGetValue(ev.Item.Serial, out bool tp) && tp)
                    {
                        spectator.Teleport(player);
                    }

                    i++;
                }
            }

            string msgNumberOfPlayer = GetTranslation(player, SummonedPlayer);
            KECustomItem.ItemEffectHint(player, msgNumberOfPlayer.Replace("%NumberOfPlayer%", i.ToString()));

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