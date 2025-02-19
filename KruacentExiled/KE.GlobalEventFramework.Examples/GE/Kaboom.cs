using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoorType = Exiled.API.Enums.DoorType;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class Kaboom : GlobalEvent, IEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1052;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Kaboom";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Les portes sont piegés attention!";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;


        public const float BaseChanceElevator = .05f;
        private float _chanceElevator;
        public float ChanceElevator
        {
            get{ return _chanceElevator;}
            set
            {
                if (value >= 0 && value <= 1)
                    _chanceElevator = value;
                else
                    _chanceElevator = BaseChanceElevator;
            }
        }
        public const float BaseChanceGate = .25f;
        private float _chanceGate;

        public float ChanceGate
        {
            get { return _chanceGate;}
            set
            {
                if (value >= 0 && value <= 1)
                    _chanceGate = value;
                else
                    _chanceGate = BaseChanceGate;
            }
        }

        public const float BaseChanceDoor = .1f;

        private float _chanceDoor;
        public float ChanceDoor
        {
            get { return _chanceDoor; }
            set
            {
                if (value > 0 && value <= 1)
                    _chanceDoor = value;
                else
                    _chanceDoor= BaseChanceDoor;
            }
        }



        ///<inheritdoc/>
        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        }
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            float random = UnityEngine.Random.value;
            bool spawnGrenade = 
                (ev.Door.IsElevator && random < .05f) ||
                (ev.Door.IsGate && random < .5f) ||
                (ev.Door.IsDamageable && random <.1f);

            Log.Debug($"i love debugging random value : {random} ; Kaboom? {spawnGrenade}");
            if (spawnGrenade)
            {
                
                ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(ev.Door.Position);
            }
        }
    }
}
