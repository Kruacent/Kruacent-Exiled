using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Features.SCPs;
using KruacentExiled.Map;
using KruacentExiled.Map.Heavy.GamblingZone.Events.EventArgs;
using LabApi.Events.Arguments.PlayerEvents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;

namespace KruacentExiled.Map.Heavy.GamblingZone
{
    public class GamblingRoom : AbstractGambling
    {
        private static readonly HashSet<GamblingRoom> _list = new HashSet<GamblingRoom>();
        public static IReadOnlyCollection<GamblingRoom> List => _list;



        private HashSet<Primitive> _model;
        public const float BasePickupTime = 5;
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


            if (MainPlugin.Configs.Debug)
            {

                _interact.OnInteracted += p => Log.Info($"{p.Nickname} interatcted"); // Runs if interactionduration is set to 0
                _interact.OnSearching += p => Log.Info($"{p.Nickname} OnSearching"); // runs when the player presses E & interactionduration != 0
                _interact.OnSearched += p => Log.Info($"{p.Nickname} OnSearched"); // Runs after searching is completed.
                _interact.OnSearchAborted += p => Log.Info($"{p.Nickname} OnSearchAborted"); // Runs after 
            }


        }


        private void CreateModel(Vector3 positionWithOffset)
        {


            Vector3 width = new Vector3(.1f, 1, .1f);


            _model = new HashSet<Primitive>()
            {
                Primitive.Create(PrimitiveType.Cube,positionWithOffset , null,new Vector3(1,.8f,1),true,Color.black),
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
            LabApi.Events.Handlers.PlayerEvents.SearchedToy -= OnPickup;
            _interact.Destroy();
            _list.Remove(this);
        }



        public void OnPickup(PlayerSearchedToyEventArgs ev)
        {
            Player player2 = ev.Player;
            if (ev.Interactable != _interact) return;
            if (player2 == null) return;
            if (SCPTeam.IsSCP(player2.ReferenceHub)) return;

            if (player2.CurrentItem == null) return;
            Item item = _lootTable.GetRandomItem();

            GamblingEventArgs ev1 = new GamblingEventArgs(player2, item, true);

            Events.Handlers.GamblingRoom.OnGambling(ev1);

            if (!ev1.IsAllowed)
            {
                return;
            }


            player2.CurrentItem.Destroy();

            player2.AddItem(item);
            if (!CanPickupOther(player2, item.Category))
            {
                player2.DropItem(item, false);
            }
            
        }

        private static bool CanPickupOther(Player player,ItemCategory category)
        {
            sbyte nbitem = 0;
            foreach(Item item in player.Items)
            {
                if(item.Category == category)
                {
                    nbitem++;
                }
            }

            return nbitem <= player.GetCategoryLimit(category);

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
