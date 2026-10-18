using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Map.Surface.SupplyDrops
{
    public class SupplyCollisionHandler : MonoBehaviour
    {
        private SupplyDrop supplyDrop;
        public static readonly TimeSpan TimeStaying = new TimeSpan(0, 1, 0);
        private float time;
        public void Init(SupplyDrop drop)
        {
            supplyDrop = drop;
        }

        private void Update()
        {
            time += Time.deltaTime;
            if(time > TimeStaying.TotalSeconds)
            {
                Destroy();
            }
        }


        private void OnTriggerEnter(Collider collider)
        {
            if(!Player.TryGet(collider,out Player player))
            {
                return;
            }
            supplyDrop.TryEffect(player);

        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}
