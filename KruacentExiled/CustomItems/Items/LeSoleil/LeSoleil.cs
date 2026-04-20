using AdminToys;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using KruacentExiled.CustomItems.API.Core.Upgrade;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using Scp914;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.LeSoleil
{ 
    public class LeSoleil : KECustomGrenade, IUpgradableCustomItem, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "The Sun",
                    [TranslationKeyDesc] = "Probably not the best idea to use it",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Le Soleil",
                    [TranslationKeyDesc] = "pas ouf",
                },
            };
        }

        public bool IsViolent { get; }
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "Le Soleil";
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
            Vector3 position = new Vector3(58.72f, 300, 20f);
            Primitive prim = Primitive.Create(position, null, null, false);
            prim.Flags = PrimitiveFlags.None;

            SoleilComp comp =prim.GameObject.AddComponent<SoleilComp>();

            comp.Init(prim);
        }


    }
}
