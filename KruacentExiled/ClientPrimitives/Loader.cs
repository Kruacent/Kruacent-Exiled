using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.ClientPrimitives
{
    public static class Loader
    {


        public static uint PrimitiveAssetId { get; private set; }
        public static uint TextToyAssetId { get; private set; }


        public static void Load()
        {

            foreach (GameObject prefab in NetworkClient.prefabs.Values)
            {
                
                if (prefab.TryGetComponent<PrimitiveObjectToy>(out _))
                {
                    PrimitiveAssetId = prefab.GetComponent<NetworkIdentity>().assetId;
                    
                }
                if (prefab.TryGetComponent<TextToy>(out _))
                {
                    TextToyAssetId = prefab.GetComponent<NetworkIdentity>().assetId;
                }
            }

        }
    }
}
