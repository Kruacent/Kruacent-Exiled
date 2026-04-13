using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features.Hints;
using KE.GlobalEventFramework.GEFE.API.Enums;
using Exiled.API.Features.Doors;
namespace KE.GlobalEventFramework.Examples.GE
{
    public class Kaboom : GlobalEvent, IEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1052;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Kaboom";
        ///<inheritdoc/>
        public override string Description { get; } = "Les portes sont piegés attention!";

        public override string[] AltDescription => 
        [
            "La guerrilla est présente"
        ];
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 2;

        public override ImpactLevel ImpactLevel => ImpactLevel.Medium;

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
            Door door = ev.Door;

            if (!door.IsOpen) return;


            Log.Debug($"i love debugging random value : {random}");
            if ((door.IsElevator && random < .05f) ||
                (door.IsGate && random < .5f) ||
                (door.IsDamageable && random < .1f))
            {
                ExplosiveGrenade grenade = ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE));
                grenade.ScpDamageMultiplier = 0.5f;


                grenade.SpawnActive(ev.Player.Position);
            }
        }
    }
}
