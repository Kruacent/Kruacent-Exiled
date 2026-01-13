using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Server;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;

namespace KE.Map.Heavy.GamblingZone
{
    public class GamblingRoom : AbstractGambling
    {
        private static readonly HashSet<GamblingRoom> _list = new HashSet<GamblingRoom>();
        public static IReadOnlyCollection<GamblingRoom> List => _list;



        private HashSet<Primitive> _model;
        public const float BasePickupTime = 10;
        public float PickupTime
        {
            get
            {
                return _interact?.InteractionDuration ?? BasePickupTime;
            }
            set
            {
                if(value < 1)
                {
                    _interact.InteractionDuration = 1;
                }
                else
                {
                    _interact.InteractionDuration = value;
                }

            }
        }
        private InteractableToy _interact;
        private Vector3 _position;
        private LootTable _lootTable;



        internal GamblingRoom(Vector3 position, LootTable lootTable, Vector3? offset = null)
        {
            Init(position, lootTable, offset);
        }



        protected void Init(Vector3 position, LootTable lootTable, Vector3? offset = null)
        {



            _position = position + (offset ?? new Vector3());
            _list.Add(this);


            _interact = InteractableToy.Create(_position, networkSpawn: false);
            _interact.InteractionDuration = BasePickupTime;

            LabApi.Events.Handlers.PlayerEvents.SearchedToy += OnPickup;
            
            CreateModel(_position);
            _interact.Spawn();
            _lootTable = lootTable;


            _interact.OnInteracted += p => Log.Info($"{p.Nickname} interatcted"); // Runs if interactionduration is set to 0
            _interact.OnSearching += p => Log.Info($"{p.Nickname} OnSearching"); // runs when the player presses E & interactionduration != 0
            _interact.OnSearched += p => Log.Info($"{p.Nickname} OnSearched"); // Runs after searching is completed.
            _interact.OnSearchAborted += p => Log.Info($"{p.Nickname} OnSearchAborted"); // Runs after 

        }


        private void CreateModel(Vector3 positionWithOffset)
        {


            Vector3 width = new Vector3(.1f, 1, .1f);


            _model = new()
            {
                Primitive.Create(PrimitiveType.Cube,positionWithOffset , null,new(1,.8f,1),true,Color.black),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.forward/2,null, Vector3.right+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.back/2, null, Vector3.right+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.right/2, null, Vector3.forward+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.left/2, null, Vector3.forward+width,true,Color.white),

            };
            foreach (Primitive p in _model)
            {
                p.Collidable = false;
            }
        }




        public override void Destroy()
        {
            foreach (Primitive p in _model)
            {
                p.Destroy();
            }
            LabApi.Events.Handlers.PlayerEvents.SearchedToy += OnPickup;
            _interact.Destroy();
            _list.Remove(this);
        }


        public void OnPickup(PlayerSearchedToyEventArgs ev)
        {
            Player player = ev.Player;
            if (ev.Interactable != _interact) return;

            Player player2 = Player.Get(player);
            if (player2 == null) return;
            if (player2.IsScp) return;

            if (player2.CurrentItem == null) return;
            Item item = _lootTable.GetRandomItem();
            player2.CurrentItem.Destroy();
            player2.AddItem(item);

            player2.DropItem(item, false);
        }
        public static void DestroyAll()
        {
            foreach (GamblingRoom gamblingRoom in List.ToList())
            {
                gamblingRoom.Destroy();
            }
        }

        public static void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public static void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            DestroyAll();
        }


        private static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            DestroyAll();
        }
    }
}
