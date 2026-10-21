using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{

    public class Tag : KECustomWeapon, IViolentItem
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "",
                    [TranslationKeyDesc] = "",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "",
                    [TranslationKeyDesc] = "",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.GunCOM15;
        public override string Name { get; set; } = "Tag";
        public override float Weight { get; set; } = 0.5f;
        public override SpawnProperties SpawnProperties { get; set; } = null;

        public bool IsViolent => false;

        private const float TagDuration = 60f;

        protected override void OnShooting(ShootingEventArgs ev)
        {
            KELog.Debug("TagIsShooted");
            if (!Check(ev.Player)) return;
            if (!ev.IsAllowed) return;

            Ray ray = new Ray(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 impactPoint = hit.point + (hit.normal * 0.03f);

                Primitive tagVisual = Primitive.Create(
                    impactPoint,
                    Vector3.one * 0.02f
                );

                tagVisual.Color = Color.red;


                Timing.CallDelayed(TagDuration, () =>
                {
                    if (tagVisual != null)
                    {
                        tagVisual.Destroy();
                    }
                });

                KELog.Debug($"Tag lumineux placé à {impactPoint} pour {TagDuration}s");
            }
        }
    }
}
