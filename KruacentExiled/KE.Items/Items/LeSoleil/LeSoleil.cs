using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Core.Models;
using KE.Items.API.Core.Upgrade;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.Items.PickupModels;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.LeSoleil
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class LeSoleil : KECustomGrenade, IUpgradableCustomItem
    {
        public override uint Id { get; set; } = 9999;
        public override string Name { get; set; } = "Le Soleil";
        public override string Description { get; set; } = "How can you even carry that?";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade => new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            //[Scp914KnobSetting.OneToOne] = new UpgradeProperties(100, 1051)
        };
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {

        };
        protected override void SubscribeEvents()
        {

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {

            base.UnsubscribeEvents();
        }



        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            CastTheSun(ev.Position);
        }

        private void CastTheSun(Vector3 position)
        {
            Primitive prim = Primitive.Create(position, null, null, false);
            prim.Flags = AdminToys.PrimitiveFlags.None;

            SoleilComp comp =prim.GameObject.AddComponent<SoleilComp>();

            comp.Init(prim);
        }


    }
}
