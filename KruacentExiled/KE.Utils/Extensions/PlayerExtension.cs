using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Extensions
{
    public static class PlayerExtension
    {
        public static void SetFakeInvis(this Player p, IEnumerable<Player> viewers)
        {
            Vector3 pos = p.Position;

            try
            {
                p.ReferenceHub.transform.localPosition = Vector3.zero;
                foreach (Player viewer in viewers)
                {
                    Server.SendSpawnMessage.Invoke(null, new object[2] { viewer.NetworkIdentity, viewer.Connection });
                }

                p.ReferenceHub.transform.localPosition = pos;
            }
            catch (Exception arg)
            {
                Log.Error(string.Format("{0}: {1}", "SetFakeInvis", arg));
            }
        }




        public static void AddLevelEffect(this Player p,EffectType type, int intensity)
        {
            


            if (p.TryGetEffect(type, out var effect))
            {
                byte newIntensity =(byte) Mathf.Clamp(effect.Intensity + intensity,byte.MinValue,byte.MaxValue);

                p.ChangeEffectIntensity(type, newIntensity);
            }
            else
            {
                byte newIntensity = (byte)Mathf.Clamp(intensity, byte.MinValue, byte.MaxValue);
                p.EnableEffect(type, newIntensity);
            }


        }



    }
}
