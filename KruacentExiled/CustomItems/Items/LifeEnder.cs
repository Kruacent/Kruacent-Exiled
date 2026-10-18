using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using System.Collections.Generic;

namespace KruacentExiled.CustomItems.Items
{

    public class LifeEnder : KECustomWeapon, IViolentItem, IHiddenName
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
        
        public override ItemType ItemType => ItemType.GunCom45;
        public override string Name { get; set; } = "LifeEnder";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; } = null;

        public bool IsViolent => true;

        public bool IsHiddenName => true;

        protected override void OnShooting(ShootingEventArgs ev)
        {

            KELog.Debug("LifeEnder is shooted");
            Player player = ev.Player;
            if (!Check(player)) return;
            if (!ev.IsAllowed) return;

            player.Kill("The Life Ender always win");
        }
    }
}
