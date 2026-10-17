using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.ClientPrimitives;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.ItemEffects;
using ProjectMER.Commands.Modifying.Rotation;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerList;

namespace KruacentExiled.CustomItems.Items
{
    public class DeployableWall : KECustomKeycard, ILumosItem, ISwitchableEffect
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Deployable Wall",
                    [TranslationKeyDesc] = "Drop to deploy a wall",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Mur",
                    [TranslationKeyDesc] = "Lâcher pour faire un mur",
                },
            };
        }
        public override ItemType ItemType => ItemType.KeycardCustomManagement;
        public override string Name { get; set; } = "Deployable Wall";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.green;
        public override string KeycardLabel { get; set; } = "Wall";
        public override Color32? KeycardLabelColor { get; set; } = Color.black;
        public override Color32? KeycardPermissionsColor { get; set; } = Color.black;
        public override Color32? TintColor { get; set; } = new Color32(139, 127, 158, 255); // janitor card color

        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance=50,
                    Type = LockerType.RifleRack,
                },
            }

        };

        public DeployableWall()
        {
            Effect = new DeployableWallEffect();
        }


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangedItem += OnChangedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangedItem -= OnChangedItem;
            base.UnsubscribeEvents();
        }

        private void OnChangedItem(ChangedItemEventArgs ev)
        {
            Player player = ev.Player;
            Item item = ev.Item;
            bool hasComp = player.GameObject.TryGetComponent<SelectComponent>(out var comp);



            if (!Check(item))
            {

                if (hasComp)
                {
                    comp.Paused = true;
                }

            }
            else
            {
                if (!hasComp)
                {
                    comp = player.GameObject.AddComponent<SelectComponent>();
                    comp.Init(player, false);

                }
                else
                {
                    comp.Paused = false;
                }
            }
        }





        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            Player player = ev.Player;
            if (ev.IsThrown)
            {
                ev.IsAllowed = true;
                return;
            }
            
            ev.IsAllowed = false;
            player.RemoveItem(ev.Item);
            Effect.Effect(ev);

            if(player.GameObject.TryGetComponent<SelectComponent>(out var comp))
            {
                comp.Destroy();
            }

        }

        

    }


    public class SelectComponent : MonoBehaviour
    {

        public Player Player { get; private set; }
        private ClientSidePrimitive primitive = null;
        private bool debug;

        private bool _paused = false;
        public bool Paused
        {
            get => _paused;
            set
            {
                Log.Info("old=" + _paused);
                Log.Info("new=" + value);

                if (value && !_paused)
                {
                    DestroyPrimitive();
                }
                _paused = value;
            }
        }
        
        public void Init(Player player,bool debug = false)
        {
            Player = player;
            this.debug = debug;
            _paused = false;
        }



        private Color32 Color = new Color32(0, 0, 200, 150);
        private Vector3 lastPosition = Vector3.zero;
        private Quaternion lastRotation = Quaternion.identity;
        private void MoveOrCreatePrimitive(Vector3 hitPosition)
        {
            Quaternion rotation = Quaternion.Euler(0, Player.Rotation.eulerAngles.y, 0);
            if (primitive == null)
            {
                
                Vector3 scale = new Vector3(4, 4, 0.2f);
                primitive = new ClientSidePrimitive(hitPosition, rotation, scale, PrimitiveType.Cube, Color, AdminToys.PrimitiveFlags.Visible, Player,false);
                primitive.SpawnClientPrimitive();
            }
            else
            {
                bool flag = false;
                
                if (!lastPosition.Equals(hitPosition))
                {
                    primitive.Position = hitPosition;
                    lastPosition = hitPosition;
                    flag = true;
                }

                if (!lastRotation.Equals(rotation))
                {
                    primitive.Rotation = rotation;
                    lastRotation = rotation;
                    flag = true;
                }
                if (flag)
                {
                    primitive.Resync();
                }
            }



            
        }

        public void DestroyPrimitive()
        {
            Log.Info("destroy prim");
            primitive?.DestroyClientPrimitive();
            primitive = null;
        }

        public void Destroy()
        {
            DestroyPrimitive();
            Destroy(this);
        }

        private float counter = 1f;
        private const float TickTime = .1f;

        internal void Update()
        {
            counter -= Time.deltaTime;
            if(counter > 0)
            {
                return; 
            }

            if (Paused)
            {
                return;
            }

            counter = TickTime;
            try
            {

                Vector3 position = DeployableWallEffect.GetSpawnPosition(Player.Position,Player.CameraTransform.forward);
                MoveOrCreatePrimitive(position);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            

        }

    }

}
