using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Quality.Enums;
using KE.Utils.Quality.Settings;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;
using QualitySettings = KE.Utils.Quality.Settings.QualitySettings;

namespace KE.Utils.Quality.Handlers
{
    public class QualityToysHandler
    {
        private HashSet<AdminToy> _pickupToys = new();
        private Dictionary<AdminToy, ModelQuality> _qualityToys = new();
        public readonly Vector3 Gulag = new Vector3(15000, 15000, 15000);
        public const float Delay = Timing.WaitForOneFrame;
        public bool IsPickupToy(AdminToy primitive)
        {
            return _pickupToys.Contains(primitive);
        }

        public bool IsQualityToy(AdminToy primitive)
        {
            return _qualityToys.ContainsKey(primitive);
        }


        public void SetAsPickup(AdminToy toy, bool autoSync = true)
        {
            if (!_pickupToys.Add(toy))
            {
                Log.Warn($"AdminToy {toy} is already a pickupToy");
                return;
            }
            if (autoSync)
                Sync(toy);
        }

        public void SetQuality(AdminToy toy, ModelQuality quality, bool autoSync = true)
        {
            if (toy == null) return;
            if (!_qualityToys.ContainsKey(toy))
                _qualityToys.Add(toy, quality);
            else
                _qualityToys[toy] = quality;
            
            if (autoSync)
                Sync(toy);
        }



        private void SendToFakePosition(AdminToy adminToy, Player player,Vector3 position)
        {
            //idk why but without delay it show all of the quality at once
            Timing.CallDelayed(Delay, () =>
            {
                if (adminToy is Light l)
                {
                    player.SendFakeSyncVar(l.Base.netIdentity, typeof(LightSourceToy), nameof(LightSourceToy.NetworkPosition), position);
                    return;
                }
                if (adminToy is Primitive p)
                {
                    player.SendFakeSyncVar(p.Base.netIdentity, typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.NetworkPosition), position);
                    return;
                }
            });
            
        }

        /// <summary>
        /// Sync every Players
        /// </summary>
        public void Sync()
        {
            foreach (Player p in Player.List)
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

            HashSet<AdminToy> pickupToRemove = [];
            HashSet<AdminToy> qualityToRemove = [];

                //hide pickup toys
                
            foreach (AdminToy pk in _pickupToys)
            {
                try
                {
                    if (pickup)
                    {
                        SendToFakePosition(pk, p, pk.Position);
                    }
                    else
                    {
                        SendToFakePosition(pk, p, Gulag);
                    }
                }
                catch (NullReferenceException)
                {
                    //Log.Warn("removing pickup");
                    pickupToRemove.Add(pk);
                }
                      
            }

            _pickupToys.RemoveWhere(p => pickupToRemove.Contains(p));


            foreach (var at in _qualityToys)
            {
                try
                {
                    if ((at.Value == quality || at.Value == ModelQuality.None) && (pickup || !_pickupToys.Contains(at.Key)))
                    {
                        SendToFakePosition(at.Key, p, at.Key.Position);
                    }
                    else
                    {
                        SendToFakePosition(at.Key, p, Gulag);
                    }
                }
                catch (NullReferenceException)
                {
                    //Log.Warn("removing quality" + _qualityToys.Count);
                    qualityToRemove.Add(at.Key);
                }


            }
            foreach(var at in qualityToRemove)
            {
                
                _qualityToys.Remove(at);
            }

        }

        /// <summary>
        /// Sync a specified AdminToy
        /// </summary>
        /// <param name="adminToy"></param>
        public void Sync(AdminToy adminToy)
        {
            //temporary fix
            Sync();

            if (IsPickupToy(adminToy))
            {

            }
            if (IsQualityToy(adminToy))
            {

            }
        }
    }
}
