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
    public class LeSoleil : KECustomGrenade, IUpgradableCustomItem
    {
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "Le Soleil";
        public override string Description { get; set; } = "Probably not the best idea to use it";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime =>5f;
        public override bool ExplodeOnCollision =>true;

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade => new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            //[Scp914KnobSetting.OneToOne] = new UpgradeProperties(100, 1051)
        };
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {

        };


        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            CastTheSun();
        }

        private void CastTheSun()
        {
            Vector3 position = new(58.72f, 300, 20f);
            Primitive prim = Primitive.Create(position, null, null, false);
            prim.Flags = AdminToys.PrimitiveFlags.None;

            SoleilComp comp =prim.GameObject.AddComponent<SoleilComp>();

            comp.Init(prim);
        }


    }
}
