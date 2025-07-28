using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Quality.Enums;
using KE.Utils.Quality.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;
using QualitySettings = KE.Utils.Quality.Settings.QualitySettings;

namespace KE.Utils.Quality
{
    public  class QualityToysHandler
    {
        private  HashSet<AdminToy> _pickupToys = new();
        private  Dictionary<AdminToy, ModelQuality> _qualityToys = new();
        public  readonly Vector3 Gulag = new Vector3(15000,15000,15000);

        public  bool IsPickupToy(AdminToy primitive)
        {
            return _pickupToys.Contains(primitive);
        }

        public  bool IsQualityToy(AdminToy primitive) 
        { 
            return _qualityToys.ContainsKey(primitive); 
        }


        public void SetAsPickup(AdminToy toy)
        {
            if (!_pickupToys.Add(toy))
            {
                Log.Warn($"AdminToy {toy} is already a pickupToy");
                return;
            }
        }

        public void SetQuality(AdminToy toy,ModelQuality quality)
        {
            if (toy == null) return;
            if (!_qualityToys.ContainsKey(toy))
                _qualityToys.Add(toy,quality);
            else
                _qualityToys[toy] = quality;
        }



        private void SendToShadowRealm(AdminToy adminToy, Player player)
        {
            if(adminToy is Light l)
            {
                player.SendFakeSyncVar(l.Base.netIdentity, typeof(LightSourceToy), nameof(LightSourceToy.NetworkPosition), Gulag);
                return;
            }
            if(adminToy is Primitive p)
            {
                player.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), Gulag);
                return;
            }
        }

        private void SetTruePosition(AdminToy adminToy, Player player)
        {
            if (adminToy is Light l)
            {
                player.SendFakeSyncVar(l.Base.netIdentity, typeof(LightSourceToy), nameof(LightSourceToy.NetworkPosition), l.Position);
                return;
            }
            if (adminToy is Primitive p)
            {
                player.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), p.Position);
                return;
            }
        }

        /// <summary>
        /// Sync every Players
        /// </summary>
        public  void Sync()
        {
            foreach(Player p in Player.List)
            {
                Sync(p);
            }
        }

        /// <summary>
        /// Sync a specified Player
        /// </summary>
        /// <param name="p"></param>
        public void Sync(Player p)
        {
            var quality = QualitySettings.Get(p);
            var pickup = QualitySettings.PickmodelActivated(p);

            if (!pickup)
            {
                //hide pickup toys
                foreach (AdminToy pk in _pickupToys)
                {
                    SendToShadowRealm(pk, p);
                }
            }
            else
            {
                //show pickup toys
                foreach (AdminToy pk in _pickupToys)
                {
                    SetTruePosition(pk, p);
                }
            }
            

            

            foreach(var at in _qualityToys)
            {
                if(at.Value == quality)
                {
                    if (pickup || !_pickupToys.Contains(at.Key))
                    {
                        SetTruePosition(at.Key, p);
                    }
                }
                else
                {
                    SendToShadowRealm(at.Key, p);
                }
                
            }

        }

        /// <summary>
        /// Sync a specified AdminToy
        /// </summary>
        /// <param name="adminToy"></param>
        public void Sync(AdminToy adminToy)
        {
            if (IsPickupToy(adminToy))
            {

            }
            if (IsQualityToy(adminToy))
            {

            }
        }
    }
}
