using Exiled.API.Features.Items;
using InventorySystem.Items.Usables.Scp244;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Item = Exiled.API.Features.Items.Item;
using Player = Exiled.API.Features.Player;
using Scp244Pickup = Exiled.API.Features.Pickups.Scp244Pickup;

namespace KE.CustomRoles.CR.SCP.SCP173
{
    public class Frozen : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Frozen SCP-173",
                    [TranslationKeyDesc] = "Instead of Tantrum you drop a SCP-244.\nKilling anyone with hypothermia gives Hume Shield",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP-173 Glacé",
                    [TranslationKeyDesc] = "Au lieu de chier tu poses un SCP-244.\nTué quelqu'un qui est en hypothermie donne du shield (dans le jeu hein)",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Frozen SCP-173",
                    [TranslationKeyDesc] = "Instead of Tantrum you drop a SCP-244.\nKilling anyone with hypothermia gives Hume Shield",
                },
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public override int MaxHealth { get; set; } = 4500;

        public override RoleTypeId Role => RoleTypeId.Scp173;

        public Color32 Color => new Color32(120, 205, 255, 0);


        protected override void SubscribeEvents()
        {

            LabApi.Events.Handlers.Scp173Events.CreatedTantrum += OnCreatedTantrum;
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;
            base.SubscribeEvents();
        }

       

        protected override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.Scp173Events.CreatedTantrum -= OnCreatedTantrum;
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
            base.UnsubscribeEvents();
        }


        private void OnDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Player == null || ev.Player.ReferenceHub == null) return;
            if (ev.Attacker == null || ev.Attacker.ReferenceHub == null) return;
            Player player = Player.Get(ev.Player);

            if (!Check(ev.Attacker)) return;


            if (ev.DamageHandler is ScpDamageHandler scpDamageHandler)
            {
                Player attacker = Player.Get(scpDamageHandler.Attacker.Hub);
                if (Check(attacker))
                {

                    Vector3 oldpos = ev.OldPosition;

                    bool isInAPrimed = _primed.Any(pickup => pickup.FogPercentForPoint(oldpos) > 0);

                    if (isInAPrimed)
                    {
                        attacker.HumeShield = Mathf.Min(attacker.MaxHumeShield, attacker.HumeShield + 400f);
                    }

                }

            }
        }

        private HashSet<Scp244DeployablePickup> _primed = new HashSet<Scp244DeployablePickup>();


        private void OnCreatedTantrum(Scp173CreatedTantrumEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            TantrumHazard tantrum = ev.Tantrum;
            float time = tantrum.LiveDuration;


            Vector3 position = tantrum.SyncedPosition + Vector3.up;
            tantrum.Destroy();

            Scp244 scp244 = (Scp244)Item.Create(ItemType.SCP244a);
            scp244.Primed = true;
            Scp244Pickup scp244Pickup = (Scp244Pickup)scp244.CreatePickup(position);


            _primed.Add(scp244Pickup.Base);

            Timing.CallDelayed(time, () =>
            {
                scp244Pickup.Destroy();
                _primed.Remove(scp244Pickup.Base);
            });


        }

        
    }
}
