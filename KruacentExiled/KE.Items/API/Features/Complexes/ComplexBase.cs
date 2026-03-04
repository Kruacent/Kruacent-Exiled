using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;

namespace KE.Items.API.Features.Complexes
{
    public abstract class ComplexBase : IWorldSpace
    {
        private GameObject gameObject;
        public Vector3 Position
        {
            get
            {
                return gameObject.transform.localPosition;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return gameObject.transform.localRotation;
            }
        }

        private InteractableToy interactableToy;
        private Primitive debugprim;

        private Player player;


        public ComplexBase()
        {
            gameObject = new GameObject();
            CreateInteractable(gameObject);

        }

        private void CreateInteractable(GameObject gameObject)
        {
            interactableToy = InteractableToy.Create(networkSpawn: false);
            interactableToy.Transform.parent = gameObject.transform;
            interactableToy.Transform.localPosition = Vector3.zero;
            interactableToy.Scale = Vector3.one;
            interactableToy.Shape = AdminToys.InvisibleInteractableToy.ColliderShape.Box;
            interactableToy.InteractionDuration = 1f;
            interactableToy.OnSearched += OnSearched;

            CreateDebugPrim(interactableToy.GameObject);
        }
        private void CreateDebugPrim(GameObject gameObject)
        {
            debugprim = Primitive.Create(Vector3.zero, null, interactableToy.Scale, false);
            debugprim.GameObject.transform.parent = interactableToy.Transform;
            debugprim.Scale = gameObject.transform.localScale;
            debugprim.Transform.localPosition = Vector3.zero;
            debugprim.Collidable = false;
            debugprim.Visible = true;
        }

        private void OnSearched(LabApi.Features.Wrappers.Player ev)
        {
            Player player = Player.Get(ev);

            if (!AddPlayer(player))
            {
                RemovePlayer();
            }

        }

        public bool AddPlayer(Player player)
        {
            if (this.player is not null) return false;

            this.player = player;
            return true;
        }
        public bool RemovePlayer()
        {
            if (player is null) return false;
            player = null;
            return true;
        }

        public void Spawn(Vector3 position,Quaternion rotation)
        {
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;

            
            SpawnInteractable();
            LabApi.Events.Handlers.PlayerEvents.ShootingWeapon += OnShootingWeapon;
        }

        private void OnShootingWeapon(LabApi.Events.Arguments.PlayerEvents.PlayerShootingWeaponEventArgs ev)
        {
            Player player = Player.Get(ev.Player);
            if (player != this.player) return;
            OnShoot(player);


        }

        public void Unspawn()
        {
            LabApi.Events.Handlers.PlayerEvents.ShootingWeapon -= OnShootingWeapon;
            UnspawnInteractable();
        }

        public void Destroy()
        {
            interactableToy.OnSearched -= OnSearched;
            interactableToy.Destroy();
            debugprim.Destroy();
        }

        private void SpawnInteractable()
        {
            Log.Debug("spawn"+ interactableToy.Transform.position);
            Log.Debug("spawn"+ debugprim.Position);
            interactableToy.Spawn();
            debugprim.Spawn();
        }
        private void UnspawnInteractable()
        {
            NetworkServer.UnSpawn(interactableToy.GameObject);
            debugprim.UnSpawn();
        }




        protected abstract void OnShoot(Player player);


    }
}
