using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Usables.Scp244;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
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
using Player = LabApi.Features.Wrappers.Player;
using Scp244Pickup = Exiled.API.Features.Pickups.Scp244Pickup;

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
            Player player = ev.Player;



            Log.Info("daryh");
            if (ev.DamageHandler is ScpDamageHandler scpDamageHandler)
            {
                Log.Info("damage");
                Player attacker = Player.Get(scpDamageHandler.Attacker.Hub);

                if (Check(attacker))
                {

                    Vector3 oldpos = ev.OldPosition;

                    bool isInAPrimed = _primed.Any(pickup => pickup.FogPercentForPoint(oldpos) > 0);

                    if (isInAPrimed)
                    {
                        Log.Info("checked");
                        attacker.HumeShield = Mathf.Min(attacker.MaxHumeShield, attacker.HumeShield + 400f);
                    }

                }

            }
        }

        private HashSet<Scp244DeployablePickup> _primed = new();


        private void OnCreatedTantrum(Scp173CreatedTantrumEventArgs ev)
        {

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
