using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{

    public class OverseerProtocolDocument : KECustomKeycard, ILumosItem
    {
        public const string TranslationAdvantage = "OverseerProtocolDocumentAdvantage";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Secret Protocol",
                    [TranslationKeyDesc] = "You need to escape with this item to get an advantage",
                    [TranslationAdvantage] = "<color=red>Your team gained +1 wave</color>",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Protocole Secret",
                    [TranslationKeyDesc] = "Tu as besoin de t'echaper avec cet objet pour gagner un avantage",
                    [TranslationAdvantage] = "<color=red>Tu viens de faire gagner +1 spawn à ton équipe</color>",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.KeycardCustomTaskForce;
        public override string Name { get; set; } = "OverseerProtocolDocument";
        public override float Weight { get; set; } = 0.65f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.red;
        public override string KeycardLabel { get; set; } = "ProtocolDocument";
        public override Color32? KeycardLabelColor { get; set; } = Color.red;
        public override Color32? KeycardPermissionsColor { get; set; } = Color.red;
        public override Color32? TintColor { get; set; } = new Color32(255, 255, 255, 255);
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = Exiled.API.Enums.RoomType.Hcz079,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 100
                }
            },

        };

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            base.UnsubscribeEvents();
        }


        private void OnEscaping(EscapingEventArgs ev)
        {
            Item document = null;

            foreach(Item item in ev.Player.Items)
            {
                if (Check(item))
                {
                    document = item;
                    KELog.Debug("player has escaped with o5 document");
                    break;
                }
            }

            if (document == null)
            {
                KELog.Debug("p didn't escape with the 05 doc");
                return;
            }

            OverseerProtocol(ev.Player);

            document.Destroy();
        }

        /// <summary>
        /// Active le protocole Overseer pour le joueur spécifié si le protocole est activé.
        /// </summary>
        /// <param name="player">Joueur concerné par le protocole Overseer.</param> Player who has stolen the 05 Protocol
        /// <param name="protocolActive">Indique si le protocole Overseer doit être activé.</param> If the protocol is active
        private void OverseerProtocol(Player player)
        {
            KELog.Debug("document are stolen method OverSeerProtocl");

            player.Role.Team.GetFaction().TryGetSpawnableFaction(out SpawnableFaction faction);
            
            Respawn.TryGetTokens(faction, out int token);
            KELog.Debug($"Faction {faction} has {token} token before modyfing");
            
            Respawn.ModifyTokens(player.Role.Team.GetFaction(), token + 1);

            Respawn.TryGetTokens(faction, out int tokenAfter);
            KELog.Debug($"Faction {faction} has {tokenAfter} token after modyfing");

            KECustomItem.ItemEffectHint(player, TranslationAdvantage);
        }
    }
}
