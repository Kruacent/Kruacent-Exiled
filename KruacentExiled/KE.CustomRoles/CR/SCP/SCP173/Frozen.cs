using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System.Linq;
using UnityEngine;
using Item = Exiled.API.Features.Items.Item;
using Player = LabApi.Features.Wrappers.Player;

namespace KE.CustomRoles.CR.SCP.SCP173
{
    public class Frozen : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Instead of Tantrum you drop a SCP-244.\nKilling anyone with hypothermia gives Hume Shield";
        public override string PublicName { get; set; } = "Frozen SCP-173";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 0;
        public override int MaxHealth { get; set; } = 4500;

        public override RoleTypeId Role => RoleTypeId.Scp173;

        public Color32 Color => new Color32(120, 205, 255, 0);


        /*protected override void SubscribeEvents()
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
            Player player = ev.Player;

            Log.Info("daryh");

            ScpDamageHandler scp = ev.DamageHandler as ScpDamageHandler;
            Log.Info(scp != null);
            Log.Info(player.TryGetEffect<Hypothermia>(out var ef));
            Log.Info(ef.IsEnabled);

            Log.Debug(Check(Player.Get(scp?.Attacker.Hub)));

            



            if (ev.DamageHandler is ScpDamageHandler scpDamageHandler && player.TryGetEffect<Hypothermia>(out var playerEffect) && playerEffect.IsEnabled)
            {
                Log.Info("damage");
                Player attacker = Player.Get(scpDamageHandler.Attacker.Hub);

                if (Check(attacker))
                {
                    Log.Info("checked");
                    attacker.HumeShield = Mathf.Min(attacker.MaxHumeShield, attacker.HumeShield + 400f);
                }

            }
        }


        private void OnCreatedTantrum(Scp173CreatedTantrumEventArgs ev)
        {

            TantrumHazard tantrum = ev.Tantrum;
            float time = tantrum.LiveDuration;


            Vector3 position = tantrum.SyncedPosition + Vector3.up;
            tantrum.Destroy();

            Scp244 scp244 = (Scp244)Item.Create(ItemType.SCP244a);
            scp244.Scale = new Vector3(.01f, .01f, .01f);
            scp244.Primed = true;
            scp244.MaxDiameter = 10f;
            Exiled.API.Features.Pickups.Pickup pickup = scp244.CreatePickup(position);
            Timing.CallDelayed(time, () =>
            {
                pickup.Destroy();
            });


        }

        */
    }
}
