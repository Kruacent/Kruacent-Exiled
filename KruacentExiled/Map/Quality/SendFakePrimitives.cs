using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Pickups;
using Mirror;
using PlayerRoles;
using PluginAPI.Roles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Map.Quality
{
    internal class SendFakePrimitives
    {
        public static void Fake()
        {
            Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            Primitive.Create(pos, null, null, true, Color.white);
            HashSet<Primitive> list = new HashSet<Primitive>();
            for (int i = 0; i < 1000; i++)
            {
                
                
                Primitive p = Primitive.Create(pos+new Vector3(.1f*i,0,0), null, new Vector3(.5f, 1.5f, .5f), true, Color.yellow);
                p.Collidable = false;
                list.Add(p);
                PrimitiveObjectToy primitive = p.Base;

                foreach (Player pl in Player.List)
                {

                    if (pl.Id % 2 == 0)
                    {
                        Log.Debug($"for player ={pl.Nickname}");
                        pl.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), new Vector3(15000,15000,15000));
                    }
                    else
                    {
                        Log.Debug($"back player ={pl.Nickname}");
                        pl.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), pos + Vector3.forward);
                    }


                    //pl.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), pos + Vector3.forward);
                }
            }
            
        }


        
        public static void Join()
        {
            //Fake();
            Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;

            var pick = Pickup.CreateAndSpawn(ItemType.Medkit, pos);

            //pick.Base.GetComponent<Rigidbody>().isKinematic = true;

            Collider collider = pick.GameObject.GetComponentInChildren<Collider>();

            


            
            //Pickup pickupFinal = Pickup.CreateAndSpawn(ItemType.KeycardGuard, p.Position);

            //if (!pickupFinal.IsLocked)
            //{
            //    PickupSyncInfo info = pickupFinal.Base.NetworkInfo;
            //    pickupFinal.Scale = new Vector3(10, 150, 10);
            //    pickupFinal.Base.NetworkInfo = info;
            //    pickupFinal.Base.GetComponent<Rigidbody>().isKinematic = true;
            //}

        }

        
    }
}
